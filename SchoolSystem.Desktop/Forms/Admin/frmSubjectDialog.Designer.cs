namespace SchoolSystem.Desktop.Forms.Admin
{
    partial class frmSubjectDialog
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
            txtName = new TextBox();
            label1 = new Label();
            txtSubjectName = new TextBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(-146, 19);
            txtName.Name = "txtName";
            txtName.Size = new Size(99, 27);
            txtName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 27);
            label1.Name = "label1";
            label1.Size = new Size(99, 20);
            label1.TabIndex = 1;
            label1.Text = "Subject name";
            // 
            // txtSubjectName
            // 
            txtSubjectName.Location = new Point(126, 27);
            txtSubjectName.Name = "txtSubjectName";
            txtSubjectName.Size = new Size(225, 27);
            txtSubjectName.TabIndex = 2;
            txtSubjectName.TextChanged += txtSubjectName_TextChanged;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(92, 77);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 32);
            btnSave.TabIndex = 3;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(203, 79);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmSubjectDialog
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(382, 133);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(txtSubjectName);
            Controls.Add(label1);
            Controls.Add(txtName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSubjectDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmSubjectDialog";
            Load += frmSubjectDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private Label label1;
        private TextBox txtSubjectName;
        private Button btnSave;
        private Button btnCancel;
    }
}