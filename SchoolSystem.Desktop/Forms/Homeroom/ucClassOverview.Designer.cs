namespace SchoolSystem.Desktop.Forms.Homeroom {
    partial class ucClassOverview {
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
            cboClass = new ComboBox();
            dgvStudents = new DataGridView();
            btnAddStudent = new Button();
            btnEditStudent = new Button();
            btnDeleteStudent = new Button();
            btnLinkParent = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            SuspendLayout();
            // 
            // cboClass
            // 
            cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClass.FormattingEnabled = true;
            cboClass.Location = new Point(126, 29);
            cboClass.Name = "cboClass";
            cboClass.Size = new Size(151, 28);
            cboClass.TabIndex = 0;
            cboClass.SelectedIndexChanged += cboClass_SelectedIndexChanged;
            // 
            // dgvStudents
            // 
            dgvStudents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Location = new Point(23, 74);
            dgvStudents.Name = "dgvStudents";
            dgvStudents.RowHeadersWidth = 51;
            dgvStudents.Size = new Size(783, 323);
            dgvStudents.TabIndex = 1;
            dgvStudents.CellContentClick += dgvStudents_CellContentClick;
            // 
            // btnAddStudent
            // 
            btnAddStudent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddStudent.Location = new Point(23, 417);
            btnAddStudent.Name = "btnAddStudent";
            btnAddStudent.Size = new Size(102, 38);
            btnAddStudent.TabIndex = 2;
            btnAddStudent.Text = "Add";
            btnAddStudent.UseVisualStyleBackColor = true;
            btnAddStudent.Click += btnAddStudent_Click;
            // 
            // btnEditStudent
            // 
            btnEditStudent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditStudent.Location = new Point(141, 417);
            btnEditStudent.Name = "btnEditStudent";
            btnEditStudent.Size = new Size(105, 38);
            btnEditStudent.TabIndex = 3;
            btnEditStudent.Text = "Edit";
            btnEditStudent.UseVisualStyleBackColor = true;
            btnEditStudent.Click += btnEditStudent_Click;
            // 
            // btnDeleteStudent
            // 
            btnDeleteStudent.AllowDrop = true;
            btnDeleteStudent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteStudent.Location = new Point(266, 417);
            btnDeleteStudent.Name = "btnDeleteStudent";
            btnDeleteStudent.Size = new Size(102, 38);
            btnDeleteStudent.TabIndex = 4;
            btnDeleteStudent.Text = "Delete";
            btnDeleteStudent.UseVisualStyleBackColor = true;
            btnDeleteStudent.Click += btnDeleteStudent_Click;
            // 
            // btnLinkParent
            // 
            btnLinkParent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLinkParent.Location = new Point(676, 417);
            btnLinkParent.Name = "btnLinkParent";
            btnLinkParent.Size = new Size(130, 38);
            btnLinkParent.TabIndex = 5;
            btnLinkParent.Text = "Link Parents";
            btnLinkParent.UseVisualStyleBackColor = true;
            btnLinkParent.Click += btnLinkParent_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 32);
            label1.Name = "label1";
            label1.Size = new Size(97, 20);
            label1.TabIndex = 6;
            label1.Text = "Select Class : ";
            // 
            // ucClassOverview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label1);
            Controls.Add(btnLinkParent);
            Controls.Add(btnDeleteStudent);
            Controls.Add(btnEditStudent);
            Controls.Add(btnAddStudent);
            Controls.Add(dgvStudents);
            Controls.Add(cboClass);
            Name = "ucClassOverview";
            Size = new Size(827, 480);
            Load += ucClassOverview_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cboClass;
        private DataGridView dgvStudents;
        private Button btnAddStudent;
        private Button btnEditStudent;
        private Button btnDeleteStudent;
        private Button btnLinkParent;
        private Label label1;
    }
}
