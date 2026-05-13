using SchoolSystem.Core.DTOs.Subject;
using SchoolSystem.Core.Models;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class frmSubjectDialog : Form
    {
        private readonly SubjectResponseDto? _subject;
        public frmSubjectDialog()
        {
            InitializeComponent();
            this.Text = "Add New Subject";
        }

        private async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            string subjectName = txtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(subjectName))
            {
                MessageBox.Show("Subject name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_subject == null)
                {
                    // Match the API: POST /api/subjects
                    var createDto = new CreateSubjectDto { Name = subjectName };
                    await ApiClient.Instance.PostAsync<object>("/api/subjects", createDto);
                }
                else
                {
                    // Match the API: PUT /api/subjects/{id}
                    var updateDto = new UpdateSubjectDto { Name = subjectName };
                    await ApiClient.Instance.PutAsync<object>($"/api/subjects/{_subject.Id}", updateDto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmSubjectDialog_Load(object sender, EventArgs e)
        {
            if (_subject != null)
            {
                txtName.Text = _subject.Name;
            }
        }

        private void txtSubjectName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}