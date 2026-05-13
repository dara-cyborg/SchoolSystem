namespace SchoolSystem.Desktop.Forms.Admin {
    partial class ucSubjectManagement {
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
            dgvSubjects = new DataGridView();
            btnAddSubject = new Button();
            label1 = new Label();
            btnDeleteSubject = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).BeginInit();
            SuspendLayout();
            // 
            // dgvSubjects
            // 
            dgvSubjects.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSubjects.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSubjects.Location = new Point(21, 116);
            dgvSubjects.Name = "dgvSubjects";
            dgvSubjects.RowHeadersWidth = 51;
            dgvSubjects.Size = new Size(735, 350);
            dgvSubjects.TabIndex = 0;
            dgvSubjects.CellContentClick += dgvSubjects_CellContentClick;
            // 
            // btnAddSubject
            // 
            btnAddSubject.Location = new Point(19, 62);
            btnAddSubject.Name = "btnAddSubject";
            btnAddSubject.Size = new Size(115, 37);
            btnAddSubject.TabIndex = 1;
            btnAddSubject.Text = "&Add";
            btnAddSubject.UseVisualStyleBackColor = true;
            btnAddSubject.Click += btnAddSubject_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 25);
            label1.Name = "label1";
            label1.Size = new Size(150, 20);
            label1.TabIndex = 2;
            label1.Text = "Subject Management";
            // 
            // btnDeleteSubject
            // 
            btnDeleteSubject.Location = new Point(149, 62);
            btnDeleteSubject.Name = "btnDeleteSubject";
            btnDeleteSubject.Size = new Size(110, 39);
            btnDeleteSubject.TabIndex = 3;
            btnDeleteSubject.Text = "&Delete";
            btnDeleteSubject.UseVisualStyleBackColor = true;
            btnDeleteSubject.Click += btnDeleteSubject_Click;
            // 
            // ucSubjectManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnDeleteSubject);
            Controls.Add(label1);
            Controls.Add(btnAddSubject);
            Controls.Add(dgvSubjects);
            Name = "ucSubjectManagement";
            Size = new Size(779, 486);
            Load += ucSubjectManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSubjects).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvSubjects;
        private Button btnAddSubject;
        private Label label1;
        private Button btnDeleteSubject;
    }
}
