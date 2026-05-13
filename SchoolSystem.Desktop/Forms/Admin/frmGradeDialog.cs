using SchoolSystem.Core.DTOs.Grade;
using SchoolSystem.Core.Models;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class frmGradeDialog : Form
    {
        private readonly GradeResponseDto? _grade;
        public frmGradeDialog(GradeResponseDto? grade = null)
        {
            InitializeComponent();
            _grade = grade;

            // Set the Window Title
            this.Text = _grade == null ? "Add New Grade" : "Edit Grade";
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            string gradeName = txtName.Text.Trim();

            try
            {
                if (_grade == null)
                {
                    // Case: ADD NEW GRADE (POST)
                    var createDto = new CreateGradeDto { Name = gradeName };
                    await ApiClient.Instance.PostAsync<object>("/api/grades", createDto);
                }
                else
                {
                    // Case: UPDATE EXISTING GRADE (PUT)
                    var updateDto = new UpdateGradeDto { Name = gradeName };
                    await ApiClient.Instance.PutAsync<object>($"/api/grades/{_grade.Id}", updateDto);
                }

                // If we reach here, it was successful
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Show the error message from the Backend (e.g., "Grade name is required")
                MessageBox.Show($"Error: {ex.Message}", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmGradeDialog_Load(object sender, EventArgs e)
        {
            if (_grade != null)
            {
                txtName.Text = _grade.Name;
            }

            // Check if the button should be enabled immediately
            UpdateButtonStatus();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void UpdateButtonStatus()
        {
            // The Save button is only clickable if the user typed something
            btnSave.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
        }
    }
}