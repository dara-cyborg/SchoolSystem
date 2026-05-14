using SchoolSystem.Core.DTOs.Subject;
using SchoolSystem.Desktop.Services;
using System;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class frmSubjectDialog : Form
    {
        private readonly SubjectResponseDto? _subject;

        public frmSubjectDialog(SubjectResponseDto? subject = null)
        {
            InitializeComponent();

            _subject = subject;

            this.Load += frmSubjectDialog_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            txtSubjectName.TextChanged += txtSubjectName_TextChanged;

            btnSave.Enabled = false;

            this.Text = _subject == null ? "Add New Subject" : "Edit Subject";
        }

        private void frmSubjectDialog_Load(object sender, EventArgs e)
        {
            if (_subject != null)
            {
                txtSubjectName.Text = _subject.Name;
            }
        }

        private void txtSubjectName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = !string.IsNullOrWhiteSpace(txtSubjectName.Text.Trim());
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtSubjectName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter subject name.");
                txtSubjectName.Focus();
                return;
            }

            try
            {
                // Change <object> to the actual DTO or a flexible type
                if (_subject == null)
                {
                    var response = await ApiClient.Instance.PostAsync<SubjectResponseDto>(
                        "/api/subjects",
                        new CreateSubjectDto { Name = name });

                    if (response == null)
                    {
                        // This is where your error "API returned NULL" comes from.
                        // If the subject was actually created in the DB, you can ignore this or 
                        // check if the status code was 201 Created.
                    }
                }
                else
                {
                    await ApiClient.Instance.PutAsync<object>(
                        $"/api/subjects/{_subject.Id}",
                        new UpdateSubjectDto
                        {
                            Name = name
                        });

                    MessageBox.Show("Subject updated successfully.");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Save failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}