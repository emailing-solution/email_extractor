using MailKit;
using MailKit.Net.Imap;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace imap_extractor
{
    public partial class EXTRACTOR : Form
    {
        public EXTRACTOR()
        {
            InitializeComponent();
        }

        private void Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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
       
        private void Extract_Click(object sender, EventArgs e)
        {

            try
            {
                string dir = "./" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm");

                extract.Enabled = false;
                extract.Text = "SEARCHING ...";

                string imap_connection = imap.Text;
                if (string.IsNullOrEmpty(imap_connection))
                {
                    MessageBox.Show("ENTER IMAP CONNECTION", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int port_connection = int.Parse(port.Text);
                if (port_connection == 0)
                {
                    MessageBox.Show("ENTER IMAP PORT", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
                        var unique = new List<string>();
                        using var client = new ImapClient();
                        client.Connect(imap_connection, port_connection, true);
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

                extract_froms.Enabled = false;
                extract_froms.Text = "SEARCHING ...";

                string imap_connection = imap.Text;
                if (string.IsNullOrEmpty(imap_connection))
                {
                    MessageBox.Show("ENTER IMAP CONNECTION", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int port_connection = int.Parse(port.Text);
                if (port_connection == 0)
                {
                    MessageBox.Show("ENTER IMAP PORT", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
                        var addresses = new List<string>();

                        using var client = new ImapClient();
                        client.Connect(imap_connection, port_connection, true);
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

        private void Isp_SelectedValueChanged(object sender, EventArgs e)
        {
            var isps = new Dictionary<string, string>()
            {
                {"windsteam.net", "imap.windstream.net"},
                {"optonline.net", "mail.optonline.net"},
                {"cox.net", "imap.cox.net"},
                {"mail.com", "imap.mail.com"},
                {"charter.net", "mobile.charter.net"},
                {"suddenlink.net", "imap.suddenlink.net"},
                {"ntlworld.com", "imap.ntlworld.com"},
                {"hawaiiantel.net", "mail.hawaiiantel.net"},
                {"bellnet.ca", "imap.bellnet.ca"},
                {"roadrunner.com", "mail.twc.com"},
                {"tds.net", "mail.tds.net"},
                {"sasktel.net", "mail.sasktel.net"},
                {"videotron.ca", "imap.videotron.ca"},
                {"ptd.net", "promail.ptd.net"},
                {"earthlink.net", "imap.earthlink.net"},
                {"zoominternet.net", "imap.zoominternet.net"},
                {"hughes.net", "imap.hughes.net"},
                {"optimum.net", "mail.optimum.net"},
                {"usa.com", "imap.mail.com"},
                {"embarqmail.com", "mail.centurylink.net"},
                {"ntelos.net", "mail.ntelos.net"},
                {"hargray.com", "secure28.carrierzone.com"},
            };


            imap.Text = isps.GetValueOrDefault(isp.Text, "NA");
        }
    }
}