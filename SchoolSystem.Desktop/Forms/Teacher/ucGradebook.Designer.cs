namespace SchoolSystem.Desktop.Forms.Teacher {
    partial class ucGradebook {
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
        private void InitializeComponent() {
            label2 = new Label();
            cboClassSubject = new ComboBox();
            label3 = new Label();
            txtEntryLabel = new TextBox();
            label4 = new Label();
            nudEntryScore = new NumericUpDown();
            nudEntryMaxScore = new NumericUpDown();
            label6 = new Label();
            dgvGradebook = new DataGridView();
            btnAddEntry = new Button();
            btnEditEntry = new Button();
            btnDeleteEntry = new Button();
            label1 = new Label();
            cboStudent = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)nudEntryScore).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudEntryMaxScore).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvGradebook).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 15);
            label2.Name = "label2";
            label2.Size = new Size(104, 20);
            label2.TabIndex = 1;
            label2.Text = "Class/Subject :";
            // 
            // cboClassSubject
            // 
            cboClassSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClassSubject.FormattingEnabled = true;
            cboClassSubject.Location = new Point(17, 38);
            cboClassSubject.Name = "cboClassSubject";
            cboClassSubject.Size = new Size(151, 28);
            cboClassSubject.TabIndex = 2;
            cboClassSubject.SelectedIndexChanged += cboClassSubject_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(206, 15);
            label3.Name = "label3";
            label3.Size = new Size(89, 20);
            label3.TabIndex = 3;
            label3.Text = "Entity Label:";
            // 
            // txtEntryLabel
            // 
            txtEntryLabel.Location = new Point(206, 38);
            txtEntryLabel.Name = "txtEntryLabel";
            txtEntryLabel.Size = new Size(133, 27);
            txtEntryLabel.TabIndex = 4;
            txtEntryLabel.TextChanged += txtEntryLabel_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(374, 15);
            label4.Name = "label4";
            label4.Size = new Size(49, 20);
            label4.TabIndex = 5;
            label4.Text = "Score:";
            // 
            // nudEntryScore
            // 
            nudEntryScore.DecimalPlaces = 2;
            nudEntryScore.Location = new Point(374, 39);
            nudEntryScore.Name = "nudEntryScore";
            nudEntryScore.Size = new Size(150, 27);
            nudEntryScore.TabIndex = 6;
            nudEntryScore.ValueChanged += nudEntryScore_ValueChanged;
            // 
            // nudEntryMaxScore
            // 
            nudEntryMaxScore.Location = new Point(556, 39);
            nudEntryMaxScore.Name = "nudEntryMaxScore";
            nudEntryMaxScore.Size = new Size(150, 27);
            nudEntryMaxScore.TabIndex = 7;
            nudEntryMaxScore.ValueChanged += nudEntryMaxScore_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(556, 15);
            label6.Name = "label6";
            label6.Size = new Size(81, 20);
            label6.TabIndex = 9;
            label6.Text = "Max Score:";
            // 
            // dgvGradebook
            // 
            dgvGradebook.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvGradebook.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGradebook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGradebook.Location = new Point(17, 173);
            dgvGradebook.Name = "dgvGradebook";
            dgvGradebook.RowHeadersWidth = 51;
            dgvGradebook.Size = new Size(784, 367);
            dgvGradebook.TabIndex = 10;
            dgvGradebook.CellContentClick += dgvGradebook_CellContentClick;
            // 
            // btnAddEntry
            // 
            btnAddEntry.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddEntry.Location = new Point(138, 557);
            btnAddEntry.Name = "btnAddEntry";
            btnAddEntry.Size = new Size(114, 42);
            btnAddEntry.TabIndex = 11;
            btnAddEntry.Text = "Add Entity";
            btnAddEntry.UseVisualStyleBackColor = true;
            btnAddEntry.Click += btnAddEntry_Click;
            // 
            // btnEditEntry
            // 
            btnEditEntry.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditEntry.Location = new Point(17, 557);
            btnEditEntry.Name = "btnEditEntry";
            btnEditEntry.Size = new Size(115, 42);
            btnEditEntry.TabIndex = 12;
            btnEditEntry.Text = "Edit Entity";
            btnEditEntry.UseVisualStyleBackColor = true;
            btnEditEntry.Click += btnEditEntry_Click;
            // 
            // btnDeleteEntry
            // 
            btnDeleteEntry.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteEntry.Location = new Point(258, 557);
            btnDeleteEntry.Name = "btnDeleteEntry";
            btnDeleteEntry.Size = new Size(113, 42);
            btnDeleteEntry.TabIndex = 13;
            btnDeleteEntry.Text = "Delete Entity";
            btnDeleteEntry.UseVisualStyleBackColor = true;
            btnDeleteEntry.Click += btnDeleteEntry_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 93);
            label1.Name = "label1";
            label1.Size = new Size(63, 20);
            label1.TabIndex = 14;
            label1.Text = "Student:";
            // 
            // cboStudent
            // 
            cboStudent.FormattingEnabled = true;
            cboStudent.Location = new Point(17, 116);
            cboStudent.Name = "cboStudent";
            cboStudent.Size = new Size(151, 28);
            cboStudent.TabIndex = 15;
            // 
            // ucGradebook
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cboStudent);
            Controls.Add(label1);
            Controls.Add(btnDeleteEntry);
            Controls.Add(btnEditEntry);
            Controls.Add(btnAddEntry);
            Controls.Add(dgvGradebook);
            Controls.Add(label6);
            Controls.Add(nudEntryMaxScore);
            Controls.Add(nudEntryScore);
            Controls.Add(label4);
            Controls.Add(txtEntryLabel);
            Controls.Add(label3);
            Controls.Add(cboClassSubject);
            Controls.Add(label2);
            Name = "ucGradebook";
            Size = new Size(821, 634);
            Load += ucGradebook_Load;
            ((System.ComponentModel.ISupportInitialize)nudEntryScore).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudEntryMaxScore).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvGradebook).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private ComboBox cboClassSubject;
        private Label label3;
        private TextBox txtEntryLabel;
        private Label label4;
        private NumericUpDown nudEntryScore;
        private NumericUpDown nudEntryMaxScore;
        private Label label6;
        private DataGridView dgvGradebook;
        private Button btnAddEntry;
        private Button btnEditEntry;
        private Button btnDeleteEntry;
        private Label label1;
        private ComboBox cboStudent;
    }
}
