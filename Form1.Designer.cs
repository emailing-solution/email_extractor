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
            this.label5 = new System.Windows.Forms.Label();
            this.search = new System.Windows.Forms.TextBox();
            this.user_pass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.extract = new System.Windows.Forms.Button();
            this.from = new System.Windows.Forms.CheckBox();
            this.extract_froms = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.search2 = new System.Windows.Forms.TextBox();
            this.extract_listid = new System.Windows.Forms.Button();
            this.extract_links = new System.Windows.Forms.Button();
            this.extract_message_id = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.feildTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 276);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "SEARCH 1:";
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(86, 273);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(275, 23);
            this.search.TabIndex = 8;
            // 
            // user_pass
            // 
            this.user_pass.Location = new System.Drawing.Point(86, 12);
            this.user_pass.Multiline = true;
            this.user_pass.Name = "user_pass";
            this.user_pass.Size = new System.Drawing.Size(275, 255);
            this.user_pass.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "USER PASS:";
            // 
            // extract
            // 
            this.extract.Location = new System.Drawing.Point(86, 362);
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
            this.from.Location = new System.Drawing.Point(87, 337);
            this.from.Name = "from";
            this.from.Size = new System.Drawing.Size(109, 19);
            this.from.TabIndex = 13;
            this.from.Text = "save multi from";
            this.from.UseVisualStyleBackColor = true;
            // 
            // extract_froms
            // 
            this.extract_froms.Location = new System.Drawing.Point(86, 391);
            this.extract_froms.Name = "extract_froms";
            this.extract_froms.Size = new System.Drawing.Size(275, 23);
            this.extract_froms.TabIndex = 14;
            this.extract_froms.Text = "EXTRACT FROMS";
            this.extract_froms.UseVisualStyleBackColor = true;
            this.extract_froms.Click += new System.EventHandler(this.Extract_froms_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 20;
            this.label4.Text = "SEARCH 2:";
            // 
            // search2
            // 
            this.search2.Location = new System.Drawing.Point(86, 304);
            this.search2.Name = "search2";
            this.search2.Size = new System.Drawing.Size(275, 23);
            this.search2.TabIndex = 19;
            // 
            // extract_listid
            // 
            this.extract_listid.Location = new System.Drawing.Point(87, 449);
            this.extract_listid.Name = "extract_listid";
            this.extract_listid.Size = new System.Drawing.Size(275, 23);
            this.extract_listid.TabIndex = 21;
            this.extract_listid.Text = "EXTRACT LIST ID";
            this.extract_listid.UseVisualStyleBackColor = true;
            this.extract_listid.Click += new System.EventHandler(this.extract_listid_Click);
            // 
            // extract_links
            // 
            this.extract_links.Location = new System.Drawing.Point(86, 478);
            this.extract_links.Name = "extract_links";
            this.extract_links.Size = new System.Drawing.Size(275, 23);
            this.extract_links.TabIndex = 22;
            this.extract_links.Text = "EXTRACT LINKS";
            this.extract_links.UseVisualStyleBackColor = true;
            this.extract_links.Click += new System.EventHandler(this.extract_links_Click);
            // 
            // extract_message_id
            // 
            this.extract_message_id.Location = new System.Drawing.Point(86, 420);
            this.extract_message_id.Name = "extract_message_id";
            this.extract_message_id.Size = new System.Drawing.Size(275, 23);
            this.extract_message_id.TabIndex = 23;
            this.extract_message_id.Text = "EXTRACT MESSAGE ID";
            this.extract_message_id.UseVisualStyleBackColor = true;
            this.extract_message_id.Click += new System.EventHandler(this.extract_message_id_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(208, 507);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "EXTRACT FEILD";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // feildTxt
            // 
            this.feildTxt.Location = new System.Drawing.Point(87, 507);
            this.feildTxt.Name = "feildTxt";
            this.feildTxt.Size = new System.Drawing.Size(115, 23);
            this.feildTxt.TabIndex = 25;
            // 
            // EXTRACTOR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 567);
            this.Controls.Add(this.feildTxt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.extract_message_id);
            this.Controls.Add(this.extract_links);
            this.Controls.Add(this.extract_listid);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.search2);
            this.Controls.Add(this.extract_froms);
            this.Controls.Add(this.from);
            this.Controls.Add(this.extract);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.user_pass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.search);
            this.MaximizeBox = false;
            this.Name = "EXTRACTOR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EXTRACTOR BY MALOHTIE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label5;
        private TextBox search;
        private TextBox user_pass;
        private Label label3;
        private Button extract;
        private CheckBox from;
        private Button extract_froms;
        private Label label4;
        private TextBox search2;
        private Button extract_listid;
        private Button extract_links;
        private Button extract_message_id;
        private Button button1;
        private TextBox feildTxt;
    }
}