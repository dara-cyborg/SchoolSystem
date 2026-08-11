namespace SchoolSystem.Desktop.Forms.Teacher {
    partial class ucScoreSubmit {
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
            cboClassSubject = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            nudMonth = new NumericUpDown();
            nudYear = new NumericUpDown();
            dgvScores = new DataGridView();
            btnSubmitScores = new Button();
            pgbScoreProgress = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)nudMonth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudYear).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvScores).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(113, 21);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 0;
            label1.Text = "Class/Subject ";
            // 
            // cboClassSubject
            // 
            cboClassSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClassSubject.FormattingEnabled = true;
            cboClassSubject.Location = new Point(22, 49);
            cboClassSubject.Name = "cboClassSubject";
            cboClassSubject.Size = new Size(305, 28);
            cboClassSubject.TabIndex = 1;
            cboClassSubject.SelectedIndexChanged += cboClassSubject_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(423, 21);
            label2.Name = "label2";
            label2.Size = new Size(52, 20);
            label2.TabIndex = 2;
            label2.Text = "Month";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(559, 21);
            label3.Name = "label3";
            label3.Size = new Size(86, 20);
            label3.TabIndex = 3;
            label3.Text = "School year";
            // 
            // nudMonth
            // 
            nudMonth.Location = new Point(373, 49);
            nudMonth.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            nudMonth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudMonth.Name = "nudMonth";
            nudMonth.Size = new Size(150, 27);
            nudMonth.TabIndex = 4;
            nudMonth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            nudMonth.ValueChanged += nudMonth_ValueChanged;
            // 
            // nudYear
            // 
            nudYear.Location = new Point(538, 50);
            nudYear.Name = "nudYear";
            nudYear.Size = new Size(150, 27);
            nudYear.TabIndex = 5;
            nudYear.ValueChanged += nudYear_ValueChanged;
            // 
            // dgvScores
            // 
            dgvScores.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvScores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvScores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvScores.EnableHeadersVisualStyles = false;
            dgvScores.Location = new Point(21, 93);
            dgvScores.Name = "dgvScores";
            dgvScores.RowHeadersWidth = 51;
            dgvScores.Size = new Size(667, 276);
            dgvScores.TabIndex = 6;
            dgvScores.CellContentClick += dgvScores_CellContentClick;
            dgvScores.CellFormatting += dgvScores_CellFormatting;
            // 
            // btnSubmitScores
            // 
            btnSubmitScores.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSubmitScores.Location = new Point(21, 411);
            btnSubmitScores.Name = "btnSubmitScores";
            btnSubmitScores.Size = new Size(151, 48);
            btnSubmitScores.TabIndex = 7;
            btnSubmitScores.Text = "Submit Score";
            btnSubmitScores.UseVisualStyleBackColor = true;
            btnSubmitScores.Click += btnSubmitScores_Click;
            // 
            // pgbScoreProgress
            // 
            pgbScoreProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pgbScoreProgress.Location = new Point(21, 384);
            pgbScoreProgress.Name = "pgbScoreProgress";
            pgbScoreProgress.Size = new Size(667, 10);
            pgbScoreProgress.Style = ProgressBarStyle.Continuous;
            pgbScoreProgress.TabIndex = 8;
            pgbScoreProgress.Visible = false;
            pgbScoreProgress.Click += pgbScoreProgress_Click;
            // 
            // ucScoreSubmit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pgbScoreProgress);
            Controls.Add(btnSubmitScores);
            Controls.Add(dgvScores);
            Controls.Add(nudYear);
            Controls.Add(nudMonth);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cboClassSubject);
            Controls.Add(label1);
            Name = "ucScoreSubmit";
            Size = new Size(706, 473);
            Load += ucScoreSubmit_Load;
            ((System.ComponentModel.ISupportInitialize)nudMonth).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudYear).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvScores).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cboClassSubject;
        private Label label2;
        private Label label3;
        private NumericUpDown nudMonth;
        private NumericUpDown nudYear;
        private DataGridView dgvScores;
        private Button btnSubmitScores;
        private ProgressBar pgbScoreProgress;
    }
}
