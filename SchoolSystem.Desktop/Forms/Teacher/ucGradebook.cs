using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.DTOs.Gradebook;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Teacher {
    public partial class ucGradebook : UserControl {
        private List<ClassSubjectWithStudentsDto> _classSubjects = new();
        private int _selectedStudentId = 0;

        public ucGradebook() {
            InitializeComponent();
            SetupGrid();

            dgvGradebook.SelectionChanged += dgvGradebook_SelectionChanged;

            var studentCombo = GetStudentComboBox();
            if (studentCombo != null) {
                studentCombo.SelectedIndexChanged += cboStudent_SelectedIndexChanged;
            }
        }

        private ComboBox? GetStudentComboBox() {
            return Controls.Find("cboStudent", true).OfType<ComboBox>().FirstOrDefault();
        }

        private void SetupGrid() {
            dgvGradebook.AutoGenerateColumns = false;
            dgvGradebook.Columns.Clear();

            dgvGradebook.Columns.Add("colId", "ID");
            dgvGradebook.Columns.Add("colStudentId", "Student ID");
            dgvGradebook.Columns.Add("colLabel", "Label");
            dgvGradebook.Columns.Add("colScore", "Score");
            dgvGradebook.Columns.Add("colMax", "Max");

            dgvGradebook.Columns["colId"].Visible = false;
            dgvGradebook.Columns["colStudentId"].Visible = false;

            dgvGradebook.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGradebook.MultiSelect = false;
        }

        private async void ucGradebook_Load(object sender, EventArgs e) {
            await LoadClassSubjects();
        }

        private async Task LoadClassSubjects() {
            try {
                var result = await ApiClient.Instance.GetAsync<List<ClassSubjectWithStudentsDto>>("/api/teachers/my-class-subjects");

                _classSubjects = result ?? new List<ClassSubjectWithStudentsDto>();

                var studentCombo = GetStudentComboBox();
                if (_classSubjects.Count == 0) {
                    cboClassSubject.DataSource = null;
                    cboClassSubject.Enabled = false;

                    if (studentCombo != null) {
                        studentCombo.DataSource = null;
                        studentCombo.Enabled = false;
                    }

                    dgvGradebook.Rows.Clear();
                    _selectedStudentId = 0;
                    return;
                }

                cboClassSubject.Enabled = true;
                cboClassSubject.DisplayMember = "SubjectName";
                cboClassSubject.ValueMember = "Id";
                cboClassSubject.DataSource = null;
                cboClassSubject.DataSource = _classSubjects;

                if (cboClassSubject.Items.Count > 0) {
                    cboClassSubject.SelectedIndex = 0;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private async void cboClassSubject_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboClassSubject.SelectedItem is not ClassSubjectWithStudentsDto selected) {
                return;
            }

            var studentCombo = GetStudentComboBox();
            dgvGradebook.Rows.Clear();
            _selectedStudentId = 0;

            if (selected.Students == null || selected.Students.Count == 0) {
                if (studentCombo != null) {
                    studentCombo.DataSource = null;
                    studentCombo.Enabled = false;
                }

                ClearInputs();
                return;
            }

            if (studentCombo == null) {
                _selectedStudentId = selected.Students[0].Id;
                await LoadGradebook(_selectedStudentId, selected.Id);
                return;
            }

            studentCombo.Enabled = true;
            studentCombo.DisplayMember = "Name";
            studentCombo.ValueMember = "Id";
            studentCombo.DataSource = null;
            studentCombo.DataSource = selected.Students;
            studentCombo.SelectedIndex = 0;
        }

        private async void cboStudent_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboClassSubject.SelectedItem is not ClassSubjectWithStudentsDto selectedClassSubject) {
                return;
            }

            var studentCombo = GetStudentComboBox();
            if (studentCombo?.SelectedItem is not StudentInClassDto selectedStudent) {
                return;
            }

            _selectedStudentId = selectedStudent.Id;
            await LoadGradebook(_selectedStudentId, selectedClassSubject.Id);
        }

        private async Task LoadGradebook(int studentId, int classSubjectId) {
            try {
                var entries = await ApiClient.Instance.GetAsync<List<GradebookEntryDto>>($"/api/gradebook/student/{studentId}/class-subject/{classSubjectId}");

                dgvGradebook.Rows.Clear();

                if (entries == null || entries.Count == 0) {
                    return;
                }

                foreach (var entry in entries) {
                    dgvGradebook.Rows.Add(entry.Id, entry.StudentId, entry.Label, entry.Score, entry.MaxScore);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnAddEntry_Click(object sender, EventArgs e) {
            if (_selectedStudentId == 0) {
                MessageBox.Show("Please select a student.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEntryLabel.Text)) {
                MessageBox.Show("Please enter an entry label.");
                return;
            }

            if (cboClassSubject.SelectedItem is not ClassSubjectWithStudentsDto selectedClassSubject) {
                MessageBox.Show("Please select a class subject.");
                return;
            }

            try {
                var dto = new CreateGradebookEntryDto {
                    StudentId = _selectedStudentId,
                    ClassSubjectId = selectedClassSubject.Id,
                    Label = txtEntryLabel.Text.Trim(),
                    Score = nudEntryScore.Value,
                    MaxScore = nudEntryMaxScore.Value
                };

                await ApiClient.Instance.PostAsync<GradebookEntryDto>("/api/gradebook", dto);

                await LoadGradebook(_selectedStudentId, selectedClassSubject.Id);
                ClearInputs();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnEditEntry_Click(object sender, EventArgs e) {
            if (dgvGradebook.SelectedRows.Count == 0) {
                MessageBox.Show("Please select a gradebook entry.");
                return;
            }

            if (cboClassSubject.SelectedItem is not ClassSubjectWithStudentsDto selectedClassSubject) {
                MessageBox.Show("Please select a class subject.");
                return;
            }

            try {
                var selectedRow = dgvGradebook.SelectedRows[0];
                var entryId = Convert.ToInt32(selectedRow.Cells["colId"].Value);

                var dto = new UpdateGradebookEntryDto {
                    Label = txtEntryLabel.Text.Trim(),
                    Score = nudEntryScore.Value,
                    MaxScore = nudEntryMaxScore.Value
                };

                await ApiClient.Instance.PutAsync<GradebookEntryDto>($"/api/gradebook/{entryId}", dto);

                await LoadGradebook(_selectedStudentId, selectedClassSubject.Id);
                ClearInputs();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDeleteEntry_Click(object sender, EventArgs e) {
            if (dgvGradebook.SelectedRows.Count == 0) {
                MessageBox.Show("Please select a gradebook entry.");
                return;
            }

            var confirm = MessageBox.Show("Delete this entry?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) {
                return;
            }

            if (cboClassSubject.SelectedItem is not ClassSubjectWithStudentsDto selectedClassSubject) {
                MessageBox.Show("Please select a class subject.");
                return;
            }

            try {
                var selectedRow = dgvGradebook.SelectedRows[0];
                var entryId = Convert.ToInt32(selectedRow.Cells["colId"].Value);

                await ApiClient.Instance.DeleteAsync($"/api/gradebook/{entryId}");

                await LoadGradebook(_selectedStudentId, selectedClassSubject.Id);
                ClearInputs();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvGradebook_SelectionChanged(object sender, EventArgs e) {
            if (dgvGradebook.SelectedRows.Count == 0) {
                return;
            }

            var selectedRow = dgvGradebook.SelectedRows[0];
            txtEntryLabel.Text = selectedRow.Cells["colLabel"].Value?.ToString() ?? string.Empty;

            if (selectedRow.Cells["colScore"].Value != null) {
                nudEntryScore.Value = Convert.ToDecimal(selectedRow.Cells["colScore"].Value);
            }

            if (selectedRow.Cells["colMax"].Value != null) {
                nudEntryMaxScore.Value = Convert.ToDecimal(selectedRow.Cells["colMax"].Value);
            }

            if (selectedRow.Cells["colStudentId"].Value != null) {
                _selectedStudentId = Convert.ToInt32(selectedRow.Cells["colStudentId"].Value);
            }
        }

        private void dgvGradebook_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            dgvGradebook_SelectionChanged(sender, EventArgs.Empty);
        }

        private void nudEntryScore_ValueChanged(object sender, EventArgs e) {
            ClampScoreToMax();
        }

        private void nudEntryMaxScore_ValueChanged(object sender, EventArgs e) {
            ClampScoreToMax();
        }

        private void ClampScoreToMax() {
            if (nudEntryScore.Value > nudEntryMaxScore.Value) {
                nudEntryScore.Value = nudEntryMaxScore.Value;
            }
        }

        private void txtEntryLabel_TextChanged(object sender, EventArgs e) {
        }

        private void ClearInputs() {
            txtEntryLabel.Clear();
            nudEntryScore.Value = 0;
            nudEntryMaxScore.Value = 100;
        }
    }
}