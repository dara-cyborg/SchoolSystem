namespace SchoolSystem.Desktop.Forms.Admin {
    partial class ucClassManagement {
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
            label1 = new Label();
            dgvClasses = new DataGridView();
            cboGradeFilter = new ComboBox();
            nudSchoolYear = new NumericUpDown();
            btnAddClass = new Button();
            btnEditClass = new Button();
            btnDeleteClass = new Button();
            btnViewClassStudents = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClasses).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudSchoolYear).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(27, 23);
            label1.Name = "label1";
            label1.Size = new Size(134, 20);
            label1.TabIndex = 0;
            label1.Text = "Class Management";
            // 
            // dgvClasses
            // 
            dgvClasses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClasses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClasses.Location = new Point(27, 109);
            dgvClasses.Name = "dgvClasses";
            dgvClasses.RowHeadersWidth = 51;
            dgvClasses.Size = new Size(682, 254);
            dgvClasses.TabIndex = 1;
            dgvClasses.CellContentClick += dgvClasses_CellContentClick;
            // 
            // cboGradeFilter
            // 
            cboGradeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGradeFilter.FormattingEnabled = true;
            cboGradeFilter.Location = new Point(27, 60);
            cboGradeFilter.Name = "cboGradeFilter";
            cboGradeFilter.Size = new Size(151, 28);
            cboGradeFilter.TabIndex = 2;
            cboGradeFilter.SelectedIndexChanged += cboGradeFilter_SelectedIndexChanged;
            // 
            // nudSchoolYear
            // 
            nudSchoolYear.Location = new Point(198, 61);
            nudSchoolYear.Maximum = new decimal(new int[] { 2090, 0, 0, 0 });
            nudSchoolYear.Minimum = new decimal(new int[] { 2020, 0, 0, 0 });
            nudSchoolYear.Name = "nudSchoolYear";
            nudSchoolYear.Size = new Size(150, 27);
            nudSchoolYear.TabIndex = 3;
            nudSchoolYear.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            nudSchoolYear.ValueChanged += nudSchoolYear_ValueChanged;
            // 
            // btnAddClass
            // 
            btnAddClass.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddClass.Location = new Point(28, 380);
            btnAddClass.Name = "btnAddClass";
            btnAddClass.Size = new Size(109, 37);
            btnAddClass.TabIndex = 4;
            btnAddClass.Text = "Add";
            btnAddClass.UseVisualStyleBackColor = true;
            btnAddClass.Click += btnAddClass_Click;
            // 
            // btnEditClass
            // 
            btnEditClass.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditClass.Location = new Point(153, 380);
            btnEditClass.Name = "btnEditClass";
            btnEditClass.Size = new Size(106, 37);
            btnEditClass.TabIndex = 5;
            btnEditClass.Text = "Edit";
            btnEditClass.UseVisualStyleBackColor = true;
            btnEditClass.Click += btnEditClass_Click;
            // 
            // btnDeleteClass
            // 
            btnDeleteClass.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteClass.Location = new Point(275, 380);
            btnDeleteClass.Name = "btnDeleteClass";
            btnDeleteClass.Size = new Size(114, 37);
            btnDeleteClass.TabIndex = 6;
            btnDeleteClass.Text = "Delete";
            btnDeleteClass.UseVisualStyleBackColor = true;
            btnDeleteClass.Click += btnDeleteClass_Click;
            // 
            // btnViewClassStudents
            // 
            btnViewClassStudents.Location = new Point(594, 61);
            btnViewClassStudents.Name = "btnViewClassStudents";
            btnViewClassStudents.Size = new Size(120, 29);
            btnViewClassStudents.TabIndex = 7;
            btnViewClassStudents.Text = "View Class";
            btnViewClassStudents.UseVisualStyleBackColor = true;
            btnViewClassStudents.Click += btnViewClassStudents_Click;
            // 
            // ucClassManagement
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnViewClassStudents);
            Controls.Add(btnDeleteClass);
            Controls.Add(btnEditClass);
            Controls.Add(btnAddClass);
            Controls.Add(nudSchoolYear);
            Controls.Add(cboGradeFilter);
            Controls.Add(dgvClasses);
            Controls.Add(label1);
            Name = "ucClassManagement";
            Size = new Size(738, 439);
            Load += ucClassManagement_Load;
            ((System.ComponentModel.ISupportInitialize)dgvClasses).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudSchoolYear).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dgvClasses;
        private ComboBox cboGradeFilter;
        private NumericUpDown nudSchoolYear;
        private Button btnAddClass;
        private Button btnEditClass;
        private Button btnDeleteClass;
        private Button btnViewClassStudents;
    }
}
