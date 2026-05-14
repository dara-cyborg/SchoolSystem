namespace SchoolSystem.Desktop.Forms.Admin
{
    partial class frmUserDialog
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
            cboSex = new ComboBox();
            dtpDob = new DateTimePicker();
            txtContact = new TextBox();
            txtPassword = new TextBox();
            clbRoles = new CheckedListBox();
            chkIsActive = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(22, 24);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "Enter User Name";
            txtName.Size = new Size(451, 27);
            txtName.TabIndex = 0;
            // 
            // cboSex
            // 
            cboSex.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSex.FormattingEnabled = true;
            cboSex.Location = new Point(22, 66);
            cboSex.Name = "cboSex";
            cboSex.Size = new Size(207, 28);
            cboSex.TabIndex = 1;
            // 
            // dtpDob
            // 
            dtpDob.Format = DateTimePickerFormat.Short;
            dtpDob.Location = new Point(251, 67);
            dtpDob.Name = "dtpDob";
            dtpDob.Size = new Size(222, 27);
            dtpDob.TabIndex = 2;
            // 
            // txtContact
            // 
            txtContact.Location = new Point(22, 111);
            txtContact.Name = "txtContact";
            txtContact.PlaceholderText = "Enter Email Contact";
            txtContact.Size = new Size(451, 27);
            txtContact.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(22, 160);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Enter Password";
            txtPassword.Size = new Size(451, 27);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // clbRoles
            // 
            clbRoles.CheckOnClick = true;
            clbRoles.FormattingEnabled = true;
            clbRoles.Location = new Point(143, 210);
            clbRoles.Name = "clbRoles";
            clbRoles.Size = new Size(330, 26);
            clbRoles.TabIndex = 5;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.Location = new Point(22, 212);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(118, 24);
            chkIsActive.TabIndex = 6;
            chkIsActive.Text = "Active status ";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Location = new Point(22, 253);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(113, 44);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Location = new Point(162, 253);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(110, 44);
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmUserDialog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(501, 329);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(chkIsActive);
            Controls.Add(clbRoles);
            Controls.Add(txtPassword);
            Controls.Add(txtContact);
            Controls.Add(dtpDob);
            Controls.Add(cboSex);
            Controls.Add(txtName);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmUserDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmUserDialog";
            Load += frmUserDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private ComboBox cboSex;
        private DateTimePicker dtpDob;
        private TextBox txtContact;
        private TextBox txtPassword;
        private CheckedListBox clbRoles;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Button btnCancel;
    }
}