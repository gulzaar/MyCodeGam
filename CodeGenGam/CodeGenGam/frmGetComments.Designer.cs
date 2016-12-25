namespace CodeGenGam
{
    partial class frmGetComments
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Sno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommenFor = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.IsMethod = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnSaveComments = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(28, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 234);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Comments";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sno,
            this.txtFileName,
            this.CommenFor,
            this.IsMethod,
            this.Comment,
            this.Valid});
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(612, 209);
            this.dataGridView1.TabIndex = 0;
            // 
            // Sno
            // 
            this.Sno.HeaderText = "Sno";
            this.Sno.Name = "Sno";
            // 
            // txtFileName
            // 
            this.txtFileName.HeaderText = "FileName";
            this.txtFileName.Name = "txtFileName";
            // 
            // CommenFor
            // 
            this.CommenFor.HeaderText = "CommentFor";
            this.CommenFor.Name = "CommenFor";
            this.CommenFor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CommenFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // IsMethod
            // 
            this.IsMethod.HeaderText = "IsMethod";
            this.IsMethod.Name = "IsMethod";
            this.IsMethod.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsMethod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Comment
            // 
            this.Comment.HeaderText = "Comment";
            this.Comment.Name = "Comment";
            // 
            // Valid
            // 
            this.Valid.HeaderText = "Acceptable";
            this.Valid.Name = "Valid";
            // 
            // btnSaveComments
            // 
            this.btnSaveComments.Location = new System.Drawing.Point(577, 246);
            this.btnSaveComments.Name = "btnSaveComments";
            this.btnSaveComments.Size = new System.Drawing.Size(75, 23);
            this.btnSaveComments.TabIndex = 1;
            this.btnSaveComments.Text = "Save Comment";
            this.btnSaveComments.UseVisualStyleBackColor = true;
            this.btnSaveComments.Click += new System.EventHandler(this.btnSaveComments_Click);
            // 
            // frmGetComments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 274);
            this.Controls.Add(this.btnSaveComments);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmGetComments";
            this.Text = "GetComments";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSaveComments;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sno;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtFileName;
        private System.Windows.Forms.DataGridViewComboBoxColumn CommenFor;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
        private System.Windows.Forms.DataGridViewImageColumn Valid;
    }
}