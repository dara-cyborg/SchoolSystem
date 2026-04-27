namespace SchoolSystem.Desktop.Forms.Auth {
    partial class frmLogin {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtName = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            lblContactSupport = new LinkLabel();
            lblError = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label1.Location = new Point(113, 9);
            label1.Name = "label1";
            label1.Size = new Size(132, 35);
            label1.TabIndex = 0;
            label1.Text = "Welcome!";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(36, 59);
            label2.Name = "label2";
            label2.Size = new Size(60, 23);
            label2.TabIndex = 1;
            label2.Text = "Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(12, 98);
            label3.Name = "label3";
            label3.Size = new Size(84, 23);
            label3.TabIndex = 2;
            label3.Text = "Password:";
            // 
            // txtName
            // 
            txtName.Location = new Point(122, 58);
            txtName.Name = "txtName";
            txtName.Size = new Size(199, 27);
            txtName.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(122, 97);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(199, 27);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Segoe UI", 10F);
            btnLogin.Location = new Point(122, 145);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(104, 43);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            // 
            // lblContactSupport
            // 
            lblContactSupport.AutoSize = true;
            lblContactSupport.Location = new Point(223, 209);
            lblContactSupport.Name = "lblContactSupport";
            lblContactSupport.Size = new Size(115, 20);
            lblContactSupport.TabIndex = 6;
            lblContactSupport.TabStop = true;
            lblContactSupport.Text = "Contact support";
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new Point(12, 213);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 20);
            lblError.TabIndex = 7;
            lblError.Visible = false;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 242);
            Controls.Add(lblError);
            Controls.Add(lblContactSupport);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtName);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "frmLogin";
            Text = "Login Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtName;
        private TextBox txtPassword;
        private Button btnLogin;
        private LinkLabel lblContactSupport;
        private Label lblError;
    }
}