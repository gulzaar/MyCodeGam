namespace CodeGenGam
{
    partial class Screen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Screen));
            this.cmbComponent = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtHierarchy = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.lstSelectBox = new System.Windows.Forms.ListBox();
            this.chkCustomComment = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txttableAlias = new System.Windows.Forms.TextBox();
            this.cmbConnection = new System.Windows.Forms.ComboBox();
            this.lblSaveMin = new System.Windows.Forms.Label();
            this.lblSaveText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnMoveFiles = new System.Windows.Forms.Button();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCommentConfig = new System.Windows.Forms.TextBox();
            this.btnGetTfsPath = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSetTfsPath = new System.Windows.Forms.Button();
            this.txtConfig = new System.Windows.Forms.TextBox();
            this.cmbDbOb = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbComponent
            // 
            this.cmbComponent.FormattingEnabled = true;
            this.cmbComponent.Items.AddRange(new object[] {
            "ByQuery",
            "ByCommand",
            "QueryResult",
            "QueryHandler",
            "CommandHandler",
            "Contract-RepositoryQuery",
            "Manager-RepositoryQuery",
            "Contract-RepositoryHandler",
            "Manager-RepositoryHandler",
            "All"});
            this.cmbComponent.Location = new System.Drawing.Point(83, 176);
            this.cmbComponent.Name = "cmbComponent";
            this.cmbComponent.Size = new System.Drawing.Size(256, 21);
            this.cmbComponent.TabIndex = 4;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(228, 204);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(112, 23);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "TableName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Connection";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtHierarchy
            // 
            this.txtHierarchy.Location = new System.Drawing.Point(84, 150);
            this.txtHierarchy.Name = "txtHierarchy";
            this.txtHierarchy.Size = new System.Drawing.Size(255, 20);
            this.txtHierarchy.TabIndex = 11;
            this.txtHierarchy.Text = "admin\\user";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Heirarchy";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(226, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 285);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightSalmon;
            this.tabPage1.Controls.Add(this.btnSaveConfig);
            this.tabPage1.Controls.Add(this.lstSelectBox);
            this.tabPage1.Controls.Add(this.chkCustomComment);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.txttableAlias);
            this.tabPage1.Controls.Add(this.cmbConnection);
            this.tabPage1.Controls.Add(this.lblSaveMin);
            this.tabPage1.Controls.Add(this.lblSaveText);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtHierarchy);
            this.tabPage1.Controls.Add(this.cmbComponent);
            this.tabPage1.Controls.Add(this.btnGenerate);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(353, 259);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Generate";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(162, 203);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(42, 23);
            this.btnSaveConfig.TabIndex = 24;
            this.btnSaveConfig.Text = "Save";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // lstSelectBox
            // 
            this.lstSelectBox.FormattingEnabled = true;
            this.lstSelectBox.Location = new System.Drawing.Point(85, 43);
            this.lstSelectBox.Name = "lstSelectBox";
            this.lstSelectBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelectBox.Size = new System.Drawing.Size(255, 69);
            this.lstSelectBox.TabIndex = 23;
            this.lstSelectBox.SelectedIndexChanged += new System.EventHandler(this.lstSelectBox_SelectedIndexChanged);
            // 
            // chkCustomComment
            // 
            this.chkCustomComment.AutoSize = true;
            this.chkCustomComment.Location = new System.Drawing.Point(18, 207);
            this.chkCustomComment.Name = "chkCustomComment";
            this.chkCustomComment.Size = new System.Drawing.Size(94, 17);
            this.chkCustomComment.TabIndex = 21;
            this.chkCustomComment.Text = "Custom Config";
            this.chkCustomComment.UseVisualStyleBackColor = true;
            this.chkCustomComment.CheckedChanged += new System.EventHandler(this.chkCustomComment_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 125);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Alias";
            // 
            // txttableAlias
            // 
            this.txttableAlias.Location = new System.Drawing.Point(85, 122);
            this.txttableAlias.Name = "txttableAlias";
            this.txttableAlias.Size = new System.Drawing.Size(255, 20);
            this.txttableAlias.TabIndex = 19;
            this.txttableAlias.Text = "BBtable";
            // 
            // cmbConnection
            // 
            this.cmbConnection.FormattingEnabled = true;
            this.cmbConnection.Location = new System.Drawing.Point(84, 18);
            this.cmbConnection.Name = "cmbConnection";
            this.cmbConnection.Size = new System.Drawing.Size(255, 21);
            this.cmbConnection.TabIndex = 17;
            this.cmbConnection.SelectedIndexChanged += new System.EventHandler(this.cmbConnection_SelectedIndexChanged);
            // 
            // lblSaveMin
            // 
            this.lblSaveMin.AutoSize = true;
            this.lblSaveMin.Location = new System.Drawing.Point(110, 236);
            this.lblSaveMin.Name = "lblSaveMin";
            this.lblSaveMin.Size = new System.Drawing.Size(24, 13);
            this.lblSaveMin.TabIndex = 16;
            this.lblSaveMin.Text = "Min";
            this.lblSaveMin.Visible = false;
            // 
            // lblSaveText
            // 
            this.lblSaveText.AutoSize = true;
            this.lblSaveText.Location = new System.Drawing.Point(16, 236);
            this.lblSaveText.Name = "lblSaveText";
            this.lblSaveText.Size = new System.Drawing.Size(88, 13);
            this.lblSaveText.TabIndex = 15;
            this.lblSaveText.Text = "You Just Saved :";
            this.lblSaveText.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Component";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightSalmon;
            this.tabPage2.Controls.Add(this.btnMoveFiles);
            this.tabPage2.Controls.Add(this.btnLoadConfig);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.txtCommentConfig);
            this.tabPage2.Controls.Add(this.btnGetTfsPath);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txtPass);
            this.tabPage2.Controls.Add(this.textBox4);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.btnSetTfsPath);
            this.tabPage2.Controls.Add(this.txtConfig);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(353, 259);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            // 
            // btnMoveFiles
            // 
            this.btnMoveFiles.Location = new System.Drawing.Point(128, 129);
            this.btnMoveFiles.Name = "btnMoveFiles";
            this.btnMoveFiles.Size = new System.Drawing.Size(95, 23);
            this.btnMoveFiles.TabIndex = 24;
            this.btnMoveFiles.Text = "Move";
            this.btnMoveFiles.UseVisualStyleBackColor = true;
            this.btnMoveFiles.Visible = false;
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(176, 75);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(58, 23);
            this.btnLoadConfig.TabIndex = 23;
            this.btnLoadConfig.Text = "Load";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Select Comment Config";
            // 
            // txtCommentConfig
            // 
            this.txtCommentConfig.Location = new System.Drawing.Point(129, 52);
            this.txtCommentConfig.Name = "txtCommentConfig";
            this.txtCommentConfig.ReadOnly = true;
            this.txtCommentConfig.Size = new System.Drawing.Size(194, 20);
            this.txtCommentConfig.TabIndex = 20;
            // 
            // btnGetTfsPath
            // 
            this.btnGetTfsPath.Location = new System.Drawing.Point(238, 101);
            this.btnGetTfsPath.Name = "btnGetTfsPath";
            this.btnGetTfsPath.Size = new System.Drawing.Size(24, 23);
            this.btnGetTfsPath.TabIndex = 19;
            this.btnGetTfsPath.Text = "...";
            this.btnGetTfsPath.UseVisualStyleBackColor = true;
            this.btnGetTfsPath.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 171);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "PassCode";
            this.label8.Visible = false;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(119, 168);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '@';
            this.txtPass.Size = new System.Drawing.Size(100, 20);
            this.txtPass.TabIndex = 17;
            this.txtPass.Text = "8225";
            // 
            // textBox4
            // 
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.Location = new System.Drawing.Point(119, 194);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(221, 34);
            this.textBox4.TabIndex = 16;
            this.textBox4.Text = "By:  Amol Gajbhiye";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Tfs Path";
            this.label6.Visible = false;
            // 
            // btnSetTfsPath
            // 
            this.btnSetTfsPath.Location = new System.Drawing.Point(264, 101);
            this.btnSetTfsPath.Name = "btnSetTfsPath";
            this.btnSetTfsPath.Size = new System.Drawing.Size(58, 23);
            this.btnSetTfsPath.TabIndex = 11;
            this.btnSetTfsPath.Text = "Set";
            this.btnSetTfsPath.UseVisualStyleBackColor = true;
            this.btnSetTfsPath.Visible = false;
            this.btnSetTfsPath.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtConfig
            // 
            this.txtConfig.Location = new System.Drawing.Point(128, 103);
            this.txtConfig.Name = "txtConfig";
            this.txtConfig.Size = new System.Drawing.Size(114, 20);
            this.txtConfig.TabIndex = 10;
            this.txtConfig.Visible = false;
            // 
            // cmbDbOb
            // 
            this.cmbDbOb.FormattingEnabled = true;
            this.cmbDbOb.Location = new System.Drawing.Point(-16, 248);
            this.cmbDbOb.Name = "cmbDbOb";
            this.cmbDbOb.Size = new System.Drawing.Size(255, 21);
            this.cmbDbOb.TabIndex = 18;
            this.cmbDbOb.SelectedIndexChanged += new System.EventHandler(this.cmbDbOb_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox3);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.cmbDbOb);
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(221, 285);
            this.panel2.TabIndex = 14;
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.Location = new System.Drawing.Point(0, 222);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(221, 61);
            this.textBox3.TabIndex = 15;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(11, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(207, 212);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Screen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BurlyWood;
            this.ClientSize = new System.Drawing.Size(581, 286);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(597, 325);
            this.MinimumSize = new System.Drawing.Size(597, 325);
            this.Name = "Screen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Global AM Code Generator";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbComponent;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtHierarchy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSetTfsPath;
        private System.Windows.Forms.TextBox txtConfig;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label lblSaveText;
        private System.Windows.Forms.Label lblSaveMin;
        private System.Windows.Forms.ComboBox cmbConnection;
        private System.Windows.Forms.ComboBox cmbDbOb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txttableAlias;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnGetTfsPath;
        private System.Windows.Forms.CheckBox chkCustomComment;
        private System.Windows.Forms.ListBox lstSelectBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCommentConfig;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnMoveFiles;

    }
}