namespace SchoolSystem.Desktop.Forms.Teacher {
    partial class ucAttendance {
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
            cboClass = new ComboBox();
            label2 = new Label();
            cboClassSubject = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            dtpDate = new DateTimePicker();
            dgvAttendance = new DataGridView();
            btnSubmitAttendance = new Button();
            btnBulkSubmit = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 62);
            label1.Name = "label1";
            label1.Size = new Size(95, 20);
            label1.TabIndex = 0;
            label1.Text = "Select class : ";
            // 
            // cboClass
            // 
            cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClass.FormattingEnabled = true;
            cboClass.Location = new Point(110, 60);
            cboClass.Name = "cboClass";
            cboClass.Size = new Size(151, 28);
            cboClass.TabIndex = 1;
            cboClass.SelectedIndexChanged += cboClass_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(293, 63);
            label2.Name = "label2";
            label2.Size = new Size(65, 20);
            label2.TabIndex = 2;
            label2.Text = "Subject :";
            // 
            // cboClassSubject
            // 
            cboClassSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClassSubject.FormattingEnabled = true;
            cboClassSubject.Location = new Point(360, 60);
            cboClassSubject.Name = "cboClassSubject";
            cboClassSubject.Size = new Size(155, 28);
            cboClassSubject.TabIndex = 3;
            cboClassSubject.SelectedIndexChanged += cboClassSubject_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 18);
            label3.Name = "label3";
            label3.Size = new Size(138, 20);
            label3.TabIndex = 4;
            label3.Text = "Student attendance";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(552, 65);
            label4.Name = "label4";
            label4.Size = new Size(52, 20);
            label4.TabIndex = 5;
            label4.Text = "Date : ";
            // 
            // dtpDate
            // 
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(601, 61);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(176, 27);
            dtpDate.TabIndex = 6;
            dtpDate.ValueChanged += dtpDate_ValueChanged;
            // 
            // dgvAttendance
            // 
            dgvAttendance.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAttendance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAttendance.Location = new Point(23, 103);
            dgvAttendance.Name = "dgvAttendance";
            dgvAttendance.RowHeadersWidth = 51;
            dgvAttendance.Size = new Size(1008, 289);
            dgvAttendance.TabIndex = 7;
            dgvAttendance.CellContentClick += dgvAttendance_CellContentClick;
            dgvAttendance.CellFormatting += dgvAttendance_CellFormatting;
            // 
            // btnSubmitAttendance
            // 
            btnSubmitAttendance.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSubmitAttendance.Location = new Point(854, 415);
            btnSubmitAttendance.Name = "btnSubmitAttendance";
            btnSubmitAttendance.Size = new Size(177, 42);
            btnSubmitAttendance.TabIndex = 8;
            btnSubmitAttendance.Text = "Submit Attendence";
            btnSubmitAttendance.UseVisualStyleBackColor = true;
            btnSubmitAttendance.Click += btnSubmitAttendance_Click;
            // 
            // btnBulkSubmit
            // 
            btnBulkSubmit.Location = new Point(854, 58);
            btnBulkSubmit.Name = "btnBulkSubmit";
            btnBulkSubmit.Size = new Size(177, 29);
            btnBulkSubmit.TabIndex = 9;
            btnBulkSubmit.Text = "Blunk mark present";
            btnBulkSubmit.UseVisualStyleBackColor = true;
            btnBulkSubmit.Click += btnBulkSubmit_Click;
            // 
            // ucAttendance
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnBulkSubmit);
            Controls.Add(btnSubmitAttendance);
            Controls.Add(dgvAttendance);
            Controls.Add(dtpDate);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(cboClassSubject);
            Controls.Add(label2);
            Controls.Add(cboClass);
            Controls.Add(label1);
            Name = "ucAttendance";
            Size = new Size(1053, 484);
            Load += ucAttendance_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAttendance).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cboClass;
        private Label label2;
        private ComboBox cboClassSubject;
        private Label label3;
        private Label label4;
        private DateTimePicker dtpDate;
        private DataGridView dgvAttendance;
        private Button btnSubmitAttendance;
        private Button btnBulkSubmit;
    }
}
