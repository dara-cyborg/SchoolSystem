namespace SchoolSystem.Desktop.Forms.Admin {
    partial class ucUserManagement {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dgvUsers = new DataGridView();
            bsUsers = new BindingSource(components);
            label1 = new Label();
            pnlToolbar = new Panel();
            txtSearch = new TextBox();
            btnRefreshUsers = new Button();
            btnDeleteUser = new Button();
            btnEditUser = new Button();
            btnAddUser = new Button();
            tmrSearch = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bsUsers).BeginInit();
            pnlToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsers
            // 
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Location = new Point(26, 185);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersWidth = 51;
            dgvUsers.Size = new Size(724, 297);
            dgvUsers.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 19);
            label1.Name = "label1";
            label1.Size = new Size(136, 20);
            label1.TabIndex = 1;
            label1.Text = "User Managements";
            // 
            // pnlToolbar
            // 
            pnlToolbar.BorderStyle = BorderStyle.FixedSingle;
            pnlToolbar.Controls.Add(txtSearch);
            pnlToolbar.Controls.Add(btnRefreshUsers);
            pnlToolbar.Controls.Add(btnDeleteUser);
            pnlToolbar.Controls.Add(btnEditUser);
            pnlToolbar.Controls.Add(btnAddUser);
            pnlToolbar.Location = new Point(26, 54);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(724, 110);
            pnlToolbar.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 65);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search User....";
            txtSearch.Size = new Size(491, 27);
            txtSearch.TabIndex = 4;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnRefreshUsers
            // 
            btnRefreshUsers.Location = new Point(402, 15);
            btnRefreshUsers.Name = "btnRefreshUsers";
            btnRefreshUsers.Size = new Size(105, 39);
            btnRefreshUsers.TabIndex = 3;
            btnRefreshUsers.Text = "Refresh";
            btnRefreshUsers.UseVisualStyleBackColor = true;
            btnRefreshUsers.Click += btnRefreshUsers_Click;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.Location = new Point(268, 15);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(109, 38);
            btnDeleteUser.TabIndex = 2;
            btnDeleteUser.Text = "Delete";
            btnDeleteUser.UseVisualStyleBackColor = true;
            btnDeleteUser.Click += btnDeleteUser_Click;
            // 
            // btnEditUser
            // 
            btnEditUser.Location = new Point(140, 15);
            btnEditUser.Name = "btnEditUser";
            btnEditUser.Size = new Size(108, 39);
            btnEditUser.TabIndex = 1;
            btnEditUser.Text = "Edit User";
            btnEditUser.UseVisualStyleBackColor = true;
            btnEditUser.Click += btnEditUser_Click;
            // 
            // btnAddUser
            // 
            btnAddUser.Location = new Point(14, 15);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(107, 39);
            btnAddUser.TabIndex = 0;
            btnAddUser.Text = "Add User";
            btnAddUser.UseVisualStyleBackColor = true;
            btnAddUser.Click += btnAddUser_Click;
            // 
            // tmrSearch
            // 
            tmrSearch.Tick += SearchTimer_Tick;
            // 
            // ucUserManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlToolbar);
            Controls.Add(label1);
            Controls.Add(dgvUsers);
            Name = "ucUserManagement";
            Size = new Size(776, 501);
            Load += ucUserManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            ((System.ComponentModel.ISupportInitialize)bsUsers).EndInit();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvUsers;
        private BindingSource bsUsers;
        private Label label1;
        private Panel pnlToolbar;
        private Button btnEditUser;
        private Button btnAddUser;
        private TextBox txtSearch;
        private Button btnRefreshUsers;
        private Button btnDeleteUser;
        private System.Windows.Forms.Timer tmrSearch;
    }
}
