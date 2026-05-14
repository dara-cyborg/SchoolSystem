namespace SchoolSystem.Desktop.Forms.Admin
{
    partial class frmClassDialog
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
            label1 = new Label();
            txtName = new TextBox();
            label2 = new Label();
            cboGrade = new ComboBox();
            nudSchoolYear = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            cboHomeroom = new ComboBox();
            btnSave = new Button();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)nudSchoolYear).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 27);
            label1.Name = "label1";
            label1.Size = new Size(121, 20);
            label1.TabIndex = 0;
            label1.Text = "Class name input";
            // 
            // txtName
            // 
            txtName.Location = new Point(24, 50);
            txtName.Name = "txtName";
            txtName.Size = new Size(399, 27);
            txtName.TabIndex = 1;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 92);
            label2.Name = "label2";
            label2.Size = new Size(112, 20);
            label2.TabIndex = 2;
            label2.Text = "Grade selection";
            // 
            // cboGrade
            // 
            cboGrade.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGrade.FormattingEnabled = true;
            cboGrade.Location = new Point(24, 115);
            cboGrade.Name = "cboGrade";
            cboGrade.Size = new Size(399, 28);
            cboGrade.TabIndex = 3;
            cboGrade.SelectedIndexChanged += cboGrade_SelectedIndexChanged;
            // 
            // nudSchoolYear
            // 
            nudSchoolYear.Location = new Point(25, 183);
            nudSchoolYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            nudSchoolYear.Minimum = new decimal(new int[] { 2000, 0, 0, 0 });
            nudSchoolYear.Name = "nudSchoolYear";
            nudSchoolYear.Size = new Size(398, 27);
            nudSchoolYear.TabIndex = 4;
            nudSchoolYear.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            nudSchoolYear.ValueChanged += nudSchoolYear_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 160);
            label3.Name = "label3";
            label3.Size = new Size(86, 20);
            label3.TabIndex = 5;
            label3.Text = "School year";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(25, 230);
            label4.Name = "label4";
            label4.Size = new Size(197, 20);
            label4.TabIndex = 6;
            label4.Text = "Homeroom teache selection";
            // 
            // cboHomeroom
            // 
            cboHomeroom.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHomeroom.FormattingEnabled = true;
            cboHomeroom.Location = new Point(25, 253);
            cboHomeroom.Name = "cboHomeroom";
            cboHomeroom.Size = new Size(399, 28);
            cboHomeroom.TabIndex = 7;
            cboHomeroom.SelectedIndexChanged += cboHomeroom_SelectedIndexChanged;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Location = new Point(24, 295);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(121, 39);
            btnSave.TabIndex = 8;
            btnSave.Text = "&Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Location = new Point(151, 295);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(120, 39);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmClassDialog
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(453, 362);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(cboHomeroom);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(nudSchoolYear);
            Controls.Add(cboGrade);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmClassDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmClassDialog";
            Load += frmClassDialog_Load;
            ((System.ComponentModel.ISupportInitialize)nudSchoolYear).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private ComboBox cboGrade;
        private NumericUpDown nudSchoolYear;
        private Label label3;
        private Label label4;
        private ComboBox cboHomeroom;
        private Button btnSave;
        private Button btnCancel;
    }
}