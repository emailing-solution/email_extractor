using MailKit;
using MailKit.Net.Imap;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace imap_extractor
{
    public partial class EXTRACTOR : Form
    {
        Dictionary<string, Tuple<string, int>> ?data = null;
        public EXTRACTOR()
        {
            InitializeComponent();
            data = LoadCSVFile("./data.csv");
        }

        static async Task WriteFileAsync(string dir, string file, string content)
        {
            if (!Path.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using StreamWriter outputFile = new(Path.Combine(dir, file));
            await outputFile.WriteAsync(content);
        }

        public static string ExtractEmail(string text)
        {
            const string MatchEmailPattern =
              @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
              + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
              + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(text);
            if (matches.Count > 0)
            {
                return matches[0].Value.ToString();
            }
            return string.Empty;
        }

        public Dictionary<string, Tuple<string, int>>? LoadCSVFile(string filePath)
        {
            try
            {
                // Open the file
                using var reader = new StreamReader(filePath);
                var result = new Dictionary<string, Tuple<string, int>>();

                // Loop through the rows
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Split the line by the semicolon delimiter
                    var fields = line.Split(';');

                    if (fields.Length != 3)
                    {
                        Console.WriteLine($"Row with incorrect number of columns found, it will be skipped.");
                        continue;
                    }

                    // Get the first field as the key
                    var key = fields[0];

                    // Get the second field as the string value
                    var stringValue = fields[1];

                    // Get the third field as the int value
                    int intValue;
                    if (!int.TryParse(fields[2], out intValue))
                    {
                        Console.WriteLine($"Failed to parse integer from third field in row with key '{key}', it will be skipped.");
                        continue;
                    }

                    // Add to dictionary
                    if (result.ContainsKey(key))
                    {
                        // Handle duplicate keys if necessary
                        Console.WriteLine($"Duplicate key '{key}' found. Only the first occurrence will be used.");
                    }
                    else
                    {
                        result.Add(key, new Tuple<string, int>(stringValue, intValue));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        private void Extract_Click(object sender, EventArgs e)
        {

            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");

                extract.Enabled = false;
                extract.Text = "SEARCHING ...";
               
                string search_connection = search.Text;
                if (string.IsNullOrEmpty(search_connection))
                {
                    MessageBox.Show("ENTER SEARCH TERM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string seach_connection2 = search2.Text;
                bool multi_from = from.Checked;
                var users = user_pass.Text.Split(Environment.NewLine).Select(x => x.Trim().Split(":")).Where(x => x.Length == 2).ToArray();
                if (users.Length < 1)
                {
                    MessageBox.Show("ENTER AT LEAST 1 USER AND PASS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int found = 0;
                Parallel.ForEach(users, user =>
                {
                try
                {
                        var config = user[0].Split("@");
                        if (config.Length != 2)
                        {
                            return;
                        }
                        var domain = config[1];
                        if(!data.ContainsKey(domain))
                        {
                            return;
                        }
                        var imap = data.GetValueOrDefault(domain);
                        var unique = new List<string>();
                        using var client = new ImapClient();
                        client.Connect(imap.Item1, imap.Item2, true);
                        client.Authenticate(user[0], user[1]);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);                      
                        Debug.WriteLine("Total messages: {0} in {1}", inbox.Count, user[0]);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);

                            var name = message.From[0].Name.Replace(' ', '_');

                            var email = message.ToString();
                            if (email.ToLower().Contains(search_connection.ToLower()))
                            {
                                if(!string.IsNullOrEmpty(seach_connection2))
                                {
                                    if (!email.ToLower().Contains(seach_connection2.ToLower())) continue;
                                }
                                
                                if (unique.Contains(name) && !multi_from) continue;

                                unique.Add(name);
                                string file = user[0].Split("@")[0] + "_" + name + "_" + Guid.NewGuid().ToString("N") + ".txt";
                                Task asyncTask = WriteFileAsync(dir, file, email);
                                Interlocked.Increment(ref found);
                            }
                        }

                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {                       
                        Debug.WriteLine(ex.ToString());
                    }

                });

                MessageBox.Show($"FOUND {found} EMAILS", "ENDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {

                extract.Text = "EXTRACT EMAILS";
                extract.Enabled = true;
            }
        }

        private void Extract_froms_Click(object sender, EventArgs e)
        {

            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");
              
                string search_connection = search.Text;
                if (string.IsNullOrEmpty(search_connection))
                {
                    MessageBox.Show("ENTER SEARCH TERM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string seach_connection2 = search2.Text;
                bool multi_from = from.Checked;
                var users = user_pass.Text.Split(Environment.NewLine).Select(x => x.Trim().Split(":")).Where(x => x.Length == 2).ToArray();
                if (users.Length < 1)
                {
                    MessageBox.Show("ENTER AT LEAST 1 USER AND PASS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int found = 0;
                Parallel.ForEach(users, user =>
                {
                    try
                    {
                        var config = user[0].Split("@");
                        if (config.Length != 2)
                        {
                            return;
                        }
                        var domain = config[1];
                        if (!data.ContainsKey(domain))
                        {
                            return;
                        }
                        var imap = data.GetValueOrDefault(domain);
                        var addresses = new List<string>();

                        using var client = new ImapClient();
                        client.Connect(imap.Item1, imap.Item2, true);
                        client.Authenticate(user[0], user[1]);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);                   
                        Debug.WriteLine("Total messages: {0} in {1}", inbox.Count, user[0]);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);
                            var name = message.From[0].Name.Replace(' ', '_');
                            var address = ExtractEmail(message.From.ToString()).ToLower();
                            if (string.IsNullOrEmpty(address)) continue;

                            var email = message.ToString();
                            if (email.ToLower().Contains(search_connection.ToLower()))
                            {
                                if (!string.IsNullOrEmpty(seach_connection2))
                                {
                                    if (!email.ToLower().Contains(seach_connection2.ToLower())) continue;
                                }

                                if (addresses.Contains(address) && !multi_from) continue;
                                addresses.Add(address);
                                Interlocked.Increment(ref found);
                            }                          
                         
                        }

                        string file = user[0].Split("@")[0] + "_FROMS_" + Guid.NewGuid().ToString("N") + ".txt";
                        Task asyncTask = WriteFileAsync(dir, file, string.Join(Environment.NewLine, addresses.ToArray()));

                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {                        
                        Debug.WriteLine(ex.ToString());
                    }

                });

                MessageBox.Show($"FOUND {found} FROMS", "ENDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

                extract_froms.Enabled = true;
                extract_froms.Text = "EXTRACT FROMS";
            }

        }

        private void extract_listid_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");

                extract_listid.Enabled = false;
                extract_listid.Text = "SEARCHING ...";
               
                string search_connection = search.Text;              
                string seach_connection2 = search2.Text;
                bool multi_from = from.Checked;
                var users = user_pass.Text.Split(Environment.NewLine).Select(x => x.Trim().Split(":")).Where(x => x.Length == 2).ToArray();
                if (users.Length < 1)
                {
                    MessageBox.Show("ENTER AT LEAST 1 USER AND PASS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int found = 0;
                Parallel.ForEach(users, user =>
                {
                    try
                    {
                        var config = user[0].Split("@");
                        if (config.Length != 2)
                        {
                            return;
                        }
                        var domain = config[1];
                        if (!data.ContainsKey(domain))
                        {
                            return;
                        }
                        var imap = data.GetValueOrDefault(domain);
                        var listIds = new List<string>();


                        using var client = new ImapClient();
                        client.Connect(imap.Item1, imap.Item2);
                        client.Authenticate(user[0], user[1]);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        Debug.WriteLine("Total messages: {0} in {1}", inbox.Count, user[0]);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);

                            var listId = message.Headers[MimeKit.HeaderId.ListId];
                            if (string.IsNullOrEmpty(listId)) continue;

                            if(!string.IsNullOrEmpty(search_connection))
                            {
                                var email = message.ToString();
                                if (!email.ToLower().Contains(search_connection.ToLower()))
                                {
                                    continue;                                   
                                }

                                if (!string.IsNullOrEmpty(seach_connection2))
                                {
                                    if (!email.ToLower().Contains(seach_connection2.ToLower())) continue;
                                }
                            }


                            listId = listId.Replace("<", "").Replace(">", "");
                            listIds.Add(listId);
                            Interlocked.Increment(ref found);

                        }

                        string file = user[0].Split("@")[0] + "_LISTIDS_" + Guid.NewGuid().ToString("N") + ".txt";
                        Task asyncTask = WriteFileAsync(dir, file, string.Join(Environment.NewLine, listIds.ToArray()));

                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                });

                MessageBox.Show($"FOUND {found} LIST ID", "ENDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

                extract_listid.Enabled = true;
                extract_listid.Text = "EXTRACT LIST IDS";
            }
        }

        private void extract_links_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");

                extract_links.Enabled = false;
                extract_links.Text = "SEARCHING ...";

               
                string search_connection = search.Text;
                string seach_connection2 = search2.Text;
                bool multi_from = from.Checked;
                var users = user_pass.Text.Split(Environment.NewLine).Select(x => x.Trim().Split(":")).Where(x => x.Length == 2).ToArray();
                if (users.Length < 1)
                {
                    MessageBox.Show("ENTER AT LEAST 1 USER AND PASS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int found = 0;
                Parallel.ForEach(users, user =>
                {
                    try
                    {
                        var config = user[0].Split("@");
                        if (config.Length != 2)
                        {
                            return;
                        }
                        var domain = config[1];
                        if (!data.ContainsKey(domain))
                        {
                            return;
                        }
                        var imap = data.GetValueOrDefault(domain);

                        var links = new List<string>();

                        using var client = new ImapClient();
                        client.Connect(imap.Item1, imap.Item2, true);
                        client.Authenticate(user[0], user[1]);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        Debug.WriteLine("Total messages: {0} in {1}", inbox.Count, user[0]);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);
                            var email = message.ToString();
                            var body = message.Body.ToString();

                            if (!string.IsNullOrEmpty(search_connection))
                            {
                                
                                if (!email.ToLower().Contains(search_connection.ToLower()))
                                {
                                    continue;
                                }

                                if (!string.IsNullOrEmpty(seach_connection2))
                                {
                                    if (!email.ToLower().Contains(seach_connection2.ToLower())) continue;
                                }
                            }

                            MatchCollection collection = Regex.Matches(body, @"(https?:\/\/?[^\s.]+\.[\w][^\s]+)", RegexOptions.Multiline);
                            foreach (Match link in collection)
                            {

                                if (!links.Contains(link.Value))
                                {
                                    Debug.WriteLine("NEW LINK: {0}", link.Value);
                                    links.Add(link.Value);
                                    Interlocked.Increment(ref found);
                                }
                            }                          
                        }

                        string file = user[0].Split("@")[0] + "_LINKS_" + Guid.NewGuid().ToString("N") + ".txt";
                        Task asyncTask = WriteFileAsync(dir, file, string.Join(Environment.NewLine, links.ToArray()));

                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                });

                MessageBox.Show($"FOUND {found} LINKS", "ENDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

                extract_links.Enabled = true;
                extract_links.Text = "EXTRACT LINKS";
            }
        }

        private void extract_message_id_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");

                extract_message_id.Enabled = false;
                extract_message_id.Text = "SEARCHING ...";
               
                string search_connection = search.Text;
                string seach_connection2 = search2.Text;
                bool multi_from = from.Checked;
                var users = user_pass.Text.Split(Environment.NewLine).Select(x => x.Trim().Split(":")).Where(x => x.Length == 2).ToArray();
                if (users.Length < 1)
                {
                    MessageBox.Show("ENTER AT LEAST 1 USER AND PASS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int found = 0;
                Parallel.ForEach(users, user =>
                {
                    try
                    {
                        var config = user[0].Split("@");
                        if (config.Length != 2)
                        {
                            return;
                        }
                        var domain = config[1];
                        if (!data.ContainsKey(domain))
                        {
                            return;
                        }
                        var imap = data.GetValueOrDefault(domain);

                        var messageIds = new List<string>();


                        using var client = new ImapClient();
                        client.Connect(imap.Item1, imap.Item2, true);
                        client.Authenticate(user[0], user[1]);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        Debug.WriteLine("Total messages: {0} in {1}", inbox.Count, user[0]);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);

                            var messageId = message?.MessageId.ToString();
                            if (string.IsNullOrEmpty(messageId)) continue;

                            if (!string.IsNullOrEmpty(search_connection))
                            {
                                var email = message.ToString();
                                if (!email.ToLower().Contains(search_connection.ToLower()))
                                {
                                    continue;
                                }

                                if (!string.IsNullOrEmpty(seach_connection2))
                                {
                                    if (!email.ToLower().Contains(seach_connection2.ToLower())) continue;
                                }
                            }

                            messageIds.Add(messageId);
                            Interlocked.Increment(ref found);

                        }

                        string file = user[0].Split("@")[0] + "_MESSAGEIDS_" + Guid.NewGuid().ToString("N") + ".txt";
                        Task asyncTask = WriteFileAsync(dir, file, string.Join(Environment.NewLine, messageIds.ToArray()));

                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                });

                MessageBox.Show($"FOUND {found} MESSAGE IDS", "ENDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

                extract_message_id.Enabled = true;
                extract_message_id.Text = "EXTRACT MESSAGE IDS";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");

                button1.Enabled = false;
                button1.Text = "SEARCHING ...";

                string search_connection = search.Text;
                string seach_connection2 = search2.Text;
                bool multi_from = from.Checked;
                var users = user_pass.Text.Split(Environment.NewLine).Select(x => x.Trim().Split(":")).Where(x => x.Length == 2).ToArray();
                if (users.Length < 1)
                {
                    MessageBox.Show("ENTER AT LEAST 1 USER AND PASS", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string feild = feildTxt.Text;
                if(string.IsNullOrEmpty(feild) )
                {
                    MessageBox.Show("ENTER FEILD TO SEARCH", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int found = 0;
                Parallel.ForEach(users, user =>
                {
                    try
                    {
                        var config = user[0].Split("@");
                        if (config.Length != 2)
                        {
                            return;
                        }
                        var domain = config[1];
                        if (!data.ContainsKey(domain))
                        {
                            return;
                        }
                        var imap = data.GetValueOrDefault(domain);
                        var listIds = new List<string>();


                        using var client = new ImapClient();
                        client.Connect(imap.Item1, imap.Item2);
                        client.Authenticate(user[0], user[1]);

                        // The Inbox folder is always available on all IMAP servers...
                        var inbox = client.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);
                        Debug.WriteLine("Total messages: {0} in {1}", inbox.Count, user[0]);

                        for (int i = 0; i < inbox.Count; i++)
                        {
                            var message = inbox.GetMessage(i);
                            var body = message.ToString();

                            string pattern = @"^.*" + Regex.Escape(feild) + @".*$";
                            Regex regex = new Regex(pattern, RegexOptions.Multiline);
                            Match match = regex.Match(body);

                            if (match.Success)
                            {
                                string foundLine = match.Value;
                                listIds.Add(foundLine);                                
                                Interlocked.Increment(ref found);
                            }                          
                        }

                        string file = user[0].Split("@")[0] + "_"+ feild + "_" + Guid.NewGuid().ToString("N") + ".txt";
                        Task asyncTask = WriteFileAsync(dir, file, string.Join(Environment.NewLine, listIds.ToArray()));

                        client.Disconnect(true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                });

                MessageBox.Show($"FOUND {found}", "ENDED", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {

                button1.Enabled = true;
                button1.Text = "EXTRACT FEILD";
            }
        }
    }
}