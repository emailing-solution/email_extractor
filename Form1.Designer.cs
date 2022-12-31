namespace imap_extractor
{
    partial class EXTRACTOR
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.imap = new System.Windows.Forms.TextBox();
            this.port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.user_pass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.extract = new System.Windows.Forms.Button();
            this.from = new System.Windows.Forms.CheckBox();
            this.extract_froms = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.isp = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "IMAP:";
            // 
            // imap
            // 
            this.imap.Location = new System.Drawing.Point(86, 42);
            this.imap.Name = "imap";
            this.imap.Size = new System.Drawing.Size(275, 23);
            this.imap.TabIndex = 1;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(86, 71);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(275, 23);
            this.port.TabIndex = 2;
            this.port.Text = "993";
            this.port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Port_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "PORT:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "SEARCH:";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(86, 100);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(275, 23);
            this.search.TabIndex = 8;
            // 
            // user_pass
            // 
            this.user_pass.Location = new System.Drawing.Point(86, 156);
            this.user_pass.Multiline = true;
            this.user_pass.Name = "user_pass";
            this.user_pass.Size = new System.Drawing.Size(275, 231);
            this.user_pass.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "USER PASS:";
            // 
            // extract
            // 
            this.extract.Location = new System.Drawing.Point(86, 393);
            this.extract.Name = "extract";
            this.extract.Size = new System.Drawing.Size(275, 23);
            this.extract.TabIndex = 12;
            this.extract.Text = "EXTRACT EMAILS";
            this.extract.UseVisualStyleBackColor = true;
            this.extract.Click += new System.EventHandler(this.Extract_Click);
            // 
            // from
            // 
            this.from.AutoSize = true;
            this.from.Location = new System.Drawing.Point(86, 129);
            this.from.Name = "from";
            this.from.Size = new System.Drawing.Size(109, 19);
            this.from.TabIndex = 13;
            this.from.Text = "save multi from";
            this.from.UseVisualStyleBackColor = true;
            // 
            // extract_froms
            // 
            this.extract_froms.Location = new System.Drawing.Point(86, 422);
            this.extract_froms.Name = "extract_froms";
            this.extract_froms.Size = new System.Drawing.Size(275, 23);
            this.extract_froms.TabIndex = 14;
            this.extract_froms.Text = "EXTRACT FROMS";
            this.extract_froms.UseVisualStyleBackColor = true;
            this.extract_froms.Click += new System.EventHandler(this.Extract_froms_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "ISP:";
            // 
            // isp
            // 
            this.isp.FormattingEnabled = true;
            this.isp.Items.AddRange(new object[] {
            "windsteam.net",
            "optonline.net",
            "cox.net",
            "mail.com",
            "charter.net",
            "suddenlink.net",
            "ntlworld.com",
            "hawaiiantel.net",
            "bellnet.ca",
            "roadrunner.com",
            "tds.net",
            "sasktel.net",
            "videotron.ca",
            "ptd.net",
            "earthlink.net",
            "zoominternet.net",
            "hughes.net",
            "optimum.net",
            "usa.com",
            "ntlworld.com",
            "embarqmail.com",
            "ntelos.net",
            "hargray.com"});
            this.isp.Location = new System.Drawing.Point(87, 11);
            this.isp.Name = "isp";
            this.isp.Size = new System.Drawing.Size(274, 23);
            this.isp.TabIndex = 18;
            this.isp.SelectedValueChanged += new System.EventHandler(this.Isp_SelectedValueChanged);
            // 
            // EXTRACTOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 457);
            this.Controls.Add(this.isp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.extract_froms);
            this.Controls.Add(this.from);
            this.Controls.Add(this.extract);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.user_pass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.search);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.port);
            this.Controls.Add(this.imap);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "EXTRACTOR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EXTRACTOR BY MALOHTIE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox imap;
        private TextBox port;
        private Label label2;
        private Label label5;
        private TextBox search;
        private TextBox user_pass;
        private Label label3;
        private Button extract;
        private CheckBox from;
        private Button extract_froms;
        private Label label6;
        private ComboBox isp;
    }
}