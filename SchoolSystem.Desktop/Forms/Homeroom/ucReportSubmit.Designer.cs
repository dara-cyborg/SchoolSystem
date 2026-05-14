namespace SchoolSystem.Desktop.Forms.Homeroom {
    partial class ucReportSubmit {
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
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            label1 = new Label();
            cboClass = new ComboBox();
            label3 = new Label();
            cboReportType = new ComboBox();
            label2 = new Label();
            nudMonth = new NumericUpDown();
            nudYear = new NumericUpDown();
            label4 = new Label();
            cboSemester = new ComboBox();
            btnSubmitReport = new Button();
            dgvReportResult = new DataGridView();
            StudentName = new DataGridViewTextBoxColumn();
            TotalScore = new DataGridViewTextBoxColumn();
            Rank = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)nudMonth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudYear).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvReportResult).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 21);
            label1.Name = "label1";
            label1.Size = new Size(114, 20);
            label1.TabIndex = 0;
            label1.Text = "Class Selection :";
            // 
            // cboClass
            // 
            cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClass.FormattingEnabled = true;
            cboClass.Location = new Point(16, 51);
            cboClass.Name = "cboClass";
            cboClass.Size = new Size(329, 28);
            cboClass.TabIndex = 1;
            cboClass.SelectedIndexChanged += cboClass_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(364, 21);
            label3.Name = "label3";
            label3.Size = new Size(98, 20);
            label3.TabIndex = 3;
            label3.Text = "Report type : ";
            // 
            // cboReportType
            // 
            cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboReportType.FormattingEnabled = true;
            cboReportType.Location = new Point(364, 51);
            cboReportType.Name = "cboReportType";
            cboReportType.Size = new Size(340, 28);
            cboReportType.TabIndex = 4;
            cboReportType.SelectedIndexChanged += cboReportType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 93);
            label2.Name = "label2";
            label2.Size = new Size(145, 20);
            label2.TabIndex = 5;
            label2.Text = "Period(Month/Year) :";
            // 
            // nudMonth
            // 
            nudMonth.Location = new Point(16, 121);
            nudMonth.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            nudMonth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudMonth.Name = "nudMonth";
            nudMonth.Size = new Size(142, 27);
            nudMonth.TabIndex = 6;
            nudMonth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudMonth.ValueChanged += nudMonth_ValueChanged;
            // 
            // nudYear
            // 
            nudYear.Location = new Point(164, 121);
            nudYear.Maximum = new decimal(new int[] { 2050, 0, 0, 0 });
            nudYear.Minimum = new decimal(new int[] { 2020, 0, 0, 0 });
            nudYear.Name = "nudYear";
            nudYear.Size = new Size(181, 27);
            nudYear.TabIndex = 7;
            nudYear.Value = new decimal(new int[] { 2024, 0, 0, 0 });
            nudYear.ValueChanged += nudYear_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(364, 93);
            label4.Name = "label4";
            label4.Size = new Size(81, 20);
            label4.TabIndex = 8;
            label4.Text = "Semester : ";
            // 
            // cboSemester
            // 
            cboSemester.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSemester.FormattingEnabled = true;
            cboSemester.Location = new Point(364, 120);
            cboSemester.Name = "cboSemester";
            cboSemester.Size = new Size(340, 28);
            cboSemester.TabIndex = 9;
            cboSemester.SelectedIndexChanged += cboSemester_SelectedIndexChanged;
            // 
            // btnSubmitReport
            // 
            btnSubmitReport.Location = new Point(558, 163);
            btnSubmitReport.Name = "btnSubmitReport";
            btnSubmitReport.Size = new Size(146, 44);
            btnSubmitReport.TabIndex = 10;
            btnSubmitReport.Text = "Submit Report";
            btnSubmitReport.UseVisualStyleBackColor = true;
            btnSubmitReport.Click += btnSubmitReport_Click;
            // 
            // dgvReportResult
            // 
            dgvReportResult.AllowUserToAddRows = false;
            dgvReportResult.AllowUserToDeleteRows = false;
            dgvReportResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvReportResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReportResult.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReportResult.Columns.AddRange(new DataGridViewColumn[] { StudentName, TotalScore, Rank });
            dgvReportResult.Location = new Point(16, 226);
            dgvReportResult.Name = "dgvReportResult";
            dgvReportResult.ReadOnly = true;
            dgvReportResult.RowHeadersWidth = 51;
            dgvReportResult.Size = new Size(688, 241);
            dgvReportResult.TabIndex = 11;
            dgvReportResult.CellContentClick += dgvReportResult_CellContentClick;
            // 
            // StudentName
            // 
            StudentName.HeaderText = "Student Name";
            StudentName.MinimumWidth = 6;
            StudentName.Name = "StudentName";
            StudentName.ReadOnly = true;
            StudentName.Visible = false;
            // 
            // TotalScore
            // 
            dataGridViewCellStyle9.Format = "N2";
            TotalScore.DefaultCellStyle = dataGridViewCellStyle9;
            TotalScore.HeaderText = "Total Score";
            TotalScore.MinimumWidth = 6;
            TotalScore.Name = "TotalScore";
            TotalScore.ReadOnly = true;
            TotalScore.Visible = false;
            // 
            // Rank
            // 
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Rank.DefaultCellStyle = dataGridViewCellStyle10;
            Rank.HeaderText = "Rank";
            Rank.MinimumWidth = 6;
            Rank.Name = "Rank";
            Rank.ReadOnly = true;
            Rank.Visible = false;
            // 
            // ucReportSubmit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvReportResult);
            Controls.Add(btnSubmitReport);
            Controls.Add(cboSemester);
            Controls.Add(label4);
            Controls.Add(nudYear);
            Controls.Add(nudMonth);
            Controls.Add(label2);
            Controls.Add(cboReportType);
            Controls.Add(label3);
            Controls.Add(cboClass);
            Controls.Add(label1);
            Name = "ucReportSubmit";
            Size = new Size(719, 482);
            Load += ucReportSubmit_Load;
            ((System.ComponentModel.ISupportInitialize)nudMonth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudYear).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvReportResult).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cboClass;
        private Label label3;
        private ComboBox cboReportType;
        private Label label2;
        private NumericUpDown nudMonth;
        private NumericUpDown nudYear;
        private Label label4;
        private ComboBox cboSemester;
        private Button btnSubmitReport;
        private DataGridView dgvReportResult;
        private DataGridViewTextBoxColumn StudentName;
        private DataGridViewTextBoxColumn TotalScore;
        private DataGridViewTextBoxColumn Rank;
    }
}
