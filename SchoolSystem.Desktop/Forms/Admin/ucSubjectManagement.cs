using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Subject;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class ucSubjectManagement : UserControl
    {
        public ucSubjectManagement()
        {
            InitializeComponent();
        }

        private async void ucSubjectManagement_Load(
            object sender,
            EventArgs e)
        {
            await LoadSubjects();
        }

        private async Task LoadSubjects()
        {
            try
            {
               
                var result = await ApiClient.Instance.GetAsync<PagedResult<SubjectResponseDto>>("/api/subjects?pageSize=100");

                dgvSubjects.DataSource = null;
                if (result?.Items != null)
                {
                    dgvSubjects.DataSource = result.Items.ToList();
                    FormatGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"កំហុសពេលទាញទិន្នន័យ៖ {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormatGrid()
        {
            if (dgvSubjects.Columns.Count == 0) return;

            if (dgvSubjects.Columns.Contains("Id")) dgvSubjects.Columns["Id"].Visible = false;
            if (dgvSubjects.Columns.Contains("CreatedAt")) dgvSubjects.Columns["CreatedAt"].Visible = false;

            if (dgvSubjects.Columns.Contains("Name")) dgvSubjects.Columns["Name"].HeaderText = "Subject Name";
            if (dgvSubjects.Columns.Contains("ClassSubjectCount")) dgvSubjects.Columns["ClassSubjectCount"].HeaderText = "Linked Classes";

            dgvSubjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSubjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSubjects.ReadOnly = true;
            dgvSubjects.AllowUserToAddRows = false;
            dgvSubjects.RowHeadersVisible = false;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadSubjects();
        }
       

        private async void btnAddSubject_Click(object sender, EventArgs e)
        {
            using var frm = new frmSubjectDialog(); 
            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadSubjects();
            }
        }

        private async void btnDeleteSubject_Click(object sender, EventArgs e)
        {
            if (dgvSubjects.CurrentRow?.DataBoundItem is SubjectResponseDto subject)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete '{subject.Name}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        await ApiClient.Instance.DeleteAsync($"/api/subjects/{subject.Id}");
                        await LoadSubjects();
                    }
                    catch (Exception ex)
                    {
                        // Likely a Foreign Key error if classes are using this subject
                        MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a subject to delete.", "Selection Required");
            }
        }

        private void dgvSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}