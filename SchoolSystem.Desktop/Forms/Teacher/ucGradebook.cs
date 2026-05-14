using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.DTOs.Gradebook;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Teacher
{

    public partial class ucGradebook : UserControl
    {
        private List<ClassSubjectWithStudentsDto> _classSubjects = new();
        private int _selectedStudentId = 0;
        public ucGradebook()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void SetupGrid()
        {
            dgvGradebook.AutoGenerateColumns = false;
            dgvGradebook.Columns.Clear();

            dgvGradebook.Columns.Add("colId", "ID");
            dgvGradebook.Columns.Add("colStudentId", "Student ID");
            dgvGradebook.Columns.Add("colLabel", "Entry");
            dgvGradebook.Columns.Add("colScore", "Score");
            dgvGradebook.Columns.Add("colMax", "Max Score");

            dgvGradebook.Columns["colId"].Visible = false;
            dgvGradebook.Columns["colStudentId"].Visible = false;

            dgvGradebook.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGradebook.MultiSelect = false;
        }

        private async void btnAddEntry_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId == 0)
            {
                MessageBox.Show("No student selected.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEntryLabel.Text))
            {
                MessageBox.Show("Please enter entry label.");
                return;
            }

            try
            {
                var dto = new CreateGradebookEntryDto
                {
                    StudentId = _selectedStudentId,
                    ClassSubjectId = Convert.ToInt32(cboClassSubject.SelectedValue),
                    Label = txtEntryLabel.Text.Trim(),
                    Score = nudEntryScore.Value,
                    MaxScore = nudEntryMaxScore.Value
                };

                await ApiClient.Instance.PostAsync<GradebookEntryDto>("/api/gradebook", dto);

                await LoadGradebook(_selectedStudentId, dto.ClassSubjectId);

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}");
            }
        }

        private async void btnDeleteEntry_Click(object sender, EventArgs e)
        {
            if (dgvGradebook.SelectedRows.Count == 0)
                return;

            var confirm = MessageBox.Show(
                "Delete this entry?",
                "Confirm",
                MessageBoxButtons.YesNo);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                int entryId = Convert.ToInt32(dgvGradebook.SelectedRows[0].Cells["colId"].Value);

                await ApiClient.Instance.DeleteAsync($"/api/gradebook/{entryId}");

                await LoadGradebook(_selectedStudentId, Convert.ToInt32(cboClassSubject.SelectedValue));

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}");
            }
        }

        private async void btnEditEntry_Click(object sender, EventArgs e)
        {
            if (dgvGradebook.SelectedRows.Count == 0)
                return;

            try
            {
                int entryId = Convert.ToInt32(dgvGradebook.SelectedRows[0].Cells["colId"].Value);

                var dto = new UpdateGradebookEntryDto
                {
                    Label = txtEntryLabel.Text.Trim(),
                    Score = nudEntryScore.Value,
                    MaxScore = nudEntryMaxScore.Value
                };

                await ApiClient.Instance.PutAsync<GradebookEntryDto>(
                    $"/api/gradebook/{entryId}",
                    dto);

                await LoadGradebook(_selectedStudentId, Convert.ToInt32(cboClassSubject.SelectedValue));

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}");
            }
        }

        private async Task LoadGradebook(int studentId, int classSubjectId)
        {
            try
            {
                var entries = await ApiClient.Instance.GetAsync<List<GradebookEntryDto>>(
                    $"/api/gradebook/student/{studentId}/class-subject/{classSubjectId}");

                dgvGradebook.Rows.Clear();

                foreach (var entry in entries)
                {
                    dgvGradebook.Rows.Add(
                        entry.Id,
                        entry.StudentId,
                        entry.Label,
                        entry.Score,
                        entry.MaxScore
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load failed: {ex.Message}");
            }
        }

        private async void ucGradebook_Load(object sender, EventArgs e)
        {
            await LoadClassSubjects();
        }
        private async Task LoadClassSubjects()
        {
            try
            {
                var result = await ApiClient.Instance.GetAsync<List<ClassSubjectWithStudentsDto>>("/api/teachers/my-class-subjects");

                if (result == null)
                {
                    MessageBox.Show("API returned NULL");
                    return;
                }

                MessageBox.Show($"Loaded {result.Count} class subjects");

                _classSubjects = result;

                cboClassSubject.DataSource = null;
                cboClassSubject.DisplayMember = "SubjectName";
                cboClassSubject.ValueMember = "Id";
                cboClassSubject.DataSource = _classSubjects;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private async void cboClassSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClassSubject.SelectedItem is not ClassSubjectWithStudentsDto selected)
                return;

            if (selected.Students.Count == 0)
            {
                dgvGradebook.Rows.Clear();
                return;
            }

            _selectedStudentId = selected.Students[0].Id;

            await LoadGradebook(_selectedStudentId, selected.Id);
        }

        private void txtEntryLabel_TextChanged(object sender, EventArgs e)
        {

        }

        private void nudEntryScore_ValueChanged(object sender, EventArgs e)
        {
            if (nudEntryScore.Value > nudEntryMaxScore.Value)
            {
                MessageBox.Show("Score cannot exceed Max Score.");
                nudEntryScore.Value = nudEntryMaxScore.Value;
            }
        }

        private void nudEntryMaxScore_ValueChanged(object sender, EventArgs e)
        {

        }

        private async void dgvGradebook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgvGradebook.Rows[e.RowIndex];

            txtEntryLabel.Text = row.Cells["colLabel"].Value?.ToString();
            nudEntryScore.Value = Convert.ToDecimal(row.Cells["colScore"].Value);
            nudEntryMaxScore.Value = Convert.ToDecimal(row.Cells["colMax"].Value);

            _selectedStudentId = Convert.ToInt32(row.Cells["colStudentId"].Value);
        }
        private void ClearInputs()
        {
            txtEntryLabel.Clear();
            nudEntryScore.Value = 0;
            nudEntryMaxScore.Value = 100;
        }
    }
}