using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Core.Enums;
using SchoolSystem.Core.Models;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Homeroom
{
    public partial class frmStudentDialog : Form
    {
        private readonly StudentDto? _student;
        private bool _isInitializing = true;

        public frmStudentDialog(StudentDto? student = null)
        {
            InitializeComponent();
            _student = student;
            this.Text = _student == null ? "Add New Student" : "Edit Student";
        }

        private async void frmStudentDialog_Load(object sender, EventArgs e)
        {
            _isInitializing = true;

            // 1. Setup Gender (SexType Enum)
            cboSex.DataSource = Enum.GetValues(typeof(SexType));

            // 2. Load Classes from AcademicService via API
            await LoadClasses();

            // 3. Fill data if in Edit Mode
            if (_student != null)
            {
                txtName.Text = _student.Name;
                cboSex.SelectedItem = _student.Sex;
                dtpDob.Value = _student.Dob ?? DateTime.Now;
                // If you add txtContact later, uncomment this:
                // txtContact.Text = _student.Contact;
                cboClass.SelectedValue = _student.ClassId;
            }

            _isInitializing = false;

            // Run validation once at the end to set button state correctly
            ValidateForm();
        }

        private async Task LoadClasses()
        {
            try
            {
                var result = await ApiClient.Instance.GetAsync<PagedResult<ClassResponseDto>>("/api/classes?pageSize=100");
                if (result?.Items != null)
                {
                    cboClass.DataSource = result.Items.ToList();
                    cboClass.DisplayMember = "Name";
                    cboClass.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load classes: {ex.Message}");
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter student name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Prepare DTO based on mode
                if (_student == null)
                {
                    var createDto = new CreateStudentDto
                    {
                        Name = txtName.Text.Trim(),
                        Sex = (SexType)cboSex.SelectedItem,
                        Dob = dtpDob.Value,
                        ClassId = (int)cboClass.SelectedValue
                    };
                    await ApiClient.Instance.PostAsync<StudentDto>("/api/students", createDto);
                }
                else
                {
                    var updateDto = new UpdateStudentDto
                    {
                        Name = txtName.Text.Trim(),
                        Sex = (SexType)cboSex.SelectedItem,
                        Dob = dtpDob.Value,
                        ClassId = (int)cboClass.SelectedValue
                    };
                    await ApiClient.Instance.PutAsync<object>($"/api/students/{_student.Id}", updateDto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Event Handlers for Real-time Validation ---

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void dtpDob_ValueChanged(object sender, EventArgs e)
        {
            // Optional: Prevent selecting a future date
            if (dtpDob.Value > DateTime.Now)
            {
                dtpDob.Value = DateTime.Now;
            }
        }

        private void cboSex_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Just for completeness
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- Helper Methods ---

        private void ValidateForm()
        {
            // Do not validate if we are still loading data (prevents NullReference errors)
            if (_isInitializing) return;

            bool isNameFilled = !string.IsNullOrWhiteSpace(txtName.Text);
            bool isClassSelected = cboClass.SelectedValue != null;

            btnSave.Enabled = isNameFilled && isClassSelected;
        }
    }
}