namespace SchoolSystem.Desktop.Forms.Homeroom
{
    partial class frmStudentDialog
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
            cboSex = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            btnSave = new Button();
            btnCancel = new Button();
            cboClass = new ComboBox();
            dtpDob = new DateTimePicker();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 19);
            label1.Name = "label1";
            label1.Size = new Size(130, 20);
            label1.TabIndex = 0;
            label1.Text = "Student full name ";
            // 
            // txtName
            // 
            txtName.Location = new Point(21, 42);
            txtName.Name = "txtName";
            txtName.Size = new Size(350, 27);
            txtName.TabIndex = 1;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 89);
            label2.Name = "label2";
            label2.Size = new Size(95, 20);
            label2.TabIndex = 2;
            label2.Text = "Sex selection";
            // 
            // cboSex
            // 
            cboSex.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSex.FormattingEnabled = true;
            cboSex.Location = new Point(21, 112);
            cboSex.Name = "cboSex";
            cboSex.Size = new Size(164, 28);
            cboSex.TabIndex = 3;
            cboSex.SelectedIndexChanged += cboSex_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(201, 89);
            label3.Name = "label3";
            label3.Size = new Size(94, 20);
            label3.TabIndex = 4;
            label3.Text = "Date of birth";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(21, 158);
            label4.Name = "label4";
            label4.Size = new Size(121, 20);
            label4.TabIndex = 6;
            label4.Text = "Class assignment";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Location = new Point(201, 230);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(170, 34);
            btnSave.TabIndex = 8;
            btnSave.Text = "Save Student";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCancel.Location = new Point(21, 230);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(170, 34);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cboClass
            // 
            cboClass.DropDownStyle = ComboBoxStyle.DropDownList;
            cboClass.FormattingEnabled = true;
            cboClass.Location = new Point(21, 181);
            cboClass.Name = "cboClass";
            cboClass.Size = new Size(350, 28);
            cboClass.TabIndex = 10;
            cboClass.SelectedIndexChanged += cboClass_SelectedIndexChanged;
            // 
            // dtpDob
            // 
            dtpDob.Format = DateTimePickerFormat.Short;
            dtpDob.Location = new Point(204, 113);
            dtpDob.Name = "dtpDob";
            dtpDob.Size = new Size(167, 27);
            dtpDob.TabIndex = 11;
            dtpDob.ValueChanged += dtpDob_ValueChanged;
            // 
            // frmStudentDialog
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(397, 292);
            Controls.Add(dtpDob);
            Controls.Add(cboClass);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(cboSex);
            Controls.Add(label2);
            Controls.Add(txtName);
            Controls.Add(label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmStudentDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmStudentDialog";
            Load += frmStudentDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private ComboBox cboSex;
        private Label label3;
        private Label label4;
        private Button btnSave;
        private Button btnCancel;
        private ComboBox cboClass;
        private DateTimePicker dtpDob;
    }
}