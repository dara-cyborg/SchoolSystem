using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.DTOs.Score;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Teacher
{
    public partial class ucScoreSubmit : UserControl
    {
        private List<ClassSubjectResponseDto> _classSubjects = new();
        private List<StudentDto> _students = new();

        public ucScoreSubmit()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void SetupGrid()
        {
            dgvScores.Columns.Clear();

            dgvScores.AutoGenerateColumns = false;
            dgvScores.AllowUserToAddRows = false;
            dgvScores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvScores.MultiSelect = false;

            dgvScores.Columns.Add("colStudentId", "Student ID");
            dgvScores.Columns.Add("colStudentName", "Student");
            dgvScores.Columns.Add("colScore", "Score");
            dgvScores.Columns.Add("colLocked", "Locked");

            dgvScores.Columns["colStudentId"].Visible = false;
            dgvScores.Columns["colLocked"].Visible = false;

            dgvScores.Columns["colStudentName"].ReadOnly = true;

            dgvScores.Columns["colScore"].Width = 120;
        }

        private async void ucScoreSubmit_Load(object sender, EventArgs e)
        {
            nudMonth.Minimum = 1;
            nudMonth.Maximum = 12;
            nudMonth.Value = DateTime.Now.Month;

            nudYear.Minimum = 2024;
            nudYear.Maximum = 2100;
            nudYear.Value = DateTime.Now.Year;

            pgbScoreProgress.Visible = false;

            await LoadClassSubjectsAsync();
        }

        private async Task LoadClassSubjectsAsync()
        {

            try
            {
                var result = await ApiClient.Instance.GetAsync<PagedResult<ClassSubjectResponseDto>>(
                    "/api/academics/class-subjects?page=1&pageSize=100");

                if (result == null)
                {
                    MessageBox.Show("API returned null. Check your endpoint or backend response.");
                    return;
                }

                if (result.Items == null || result.Items.Count == 0)
                {
                    MessageBox.Show("No class subjects found.");
                    cboClassSubject.DataSource = null;
                    return;
                }

                _classSubjects = result.Items;

                cboClassSubject.DataSource = null;
                cboClassSubject.DisplayMember = "";
                cboClassSubject.ValueMember = "";

                cboClassSubject.DataSource = _classSubjects;
                cboClassSubject.DisplayMember = "SubjectName";
                cboClassSubject.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load class subject failed:\n{ex.Message}");
            }
        }

        private async void cboClassSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClassSubject.SelectedItem is not ClassSubjectResponseDto selected)
                return;

            await LoadStudentsAsync(selected.ClassId);
        }

        private async Task LoadStudentsAsync(int classId)
        {
            try
            {
                _students = await ApiClient.Instance.GetAsync<List<StudentDto>>(
                    $"/api/academics/classes/{classId}/students");

                dgvScores.Rows.Clear();

                foreach (var student in _students)
                {
                    dgvScores.Rows.Add(
                        student.Id,
                        student.Name,
                        "",
                        false
                    );
                }

                await LoadExistingScoresAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Load students failed:\n{ex.Message}");
            }
        }

        private async Task LoadExistingScoresAsync()
        {
            if (cboClassSubject.SelectedValue == null)
                return;

            try
            {
                int classSubjectId = Convert.ToInt32(cboClassSubject.SelectedValue);
                short month = (short)nudMonth.Value;
                short year = (short)nudYear.Value;

                var scores = await ApiClient.Instance.GetAsync<List<MonthlyScoreResponseDto>>(
                    $"/api/scores/monthly/class-subject/{classSubjectId}?month={month}&schoolYear={year}");

                foreach (DataGridViewRow row in dgvScores.Rows)
                {
                    if (row.IsNewRow) continue;

                    int studentId = Convert.ToInt32(row.Cells["colStudentId"].Value);

                    var found = scores.FirstOrDefault(x => x.StudentId == studentId);

                    if (found != null)
                    {
                        row.Cells["colScore"].Value = found.FinalScore;
                        row.Cells["colLocked"].Value = found.IsLocked;

                        if (found.IsLocked)
                        {
                            row.ReadOnly = true;
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                            row.DefaultCellStyle.ForeColor = Color.DarkGray;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private async void btnSubmitScores_Click(object sender, EventArgs e)
        {
            if (cboClassSubject.SelectedValue == null)
            {
                MessageBox.Show("Please select class subject.");
                return;
            }

            if (dgvScores.Rows.Count == 0)
            {
                MessageBox.Show("No student list.");
                return;
            }

            try
            {
                int classSubjectId = Convert.ToInt32(cboClassSubject.SelectedValue);
                short month = (short)nudMonth.Value;
                short year = (short)nudYear.Value;

                pgbScoreProgress.Visible = true;
                pgbScoreProgress.Minimum = 0;
                pgbScoreProgress.Maximum = dgvScores.Rows.Count;
                pgbScoreProgress.Value = 0;

                foreach (DataGridViewRow row in dgvScores.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    bool locked = row.Cells["colLocked"].Value != null &&
                                  Convert.ToBoolean(row.Cells["colLocked"].Value);

                    if (locked)
                    {
                        pgbScoreProgress.Value++;
                        continue;
                    }

                    if (!decimal.TryParse(row.Cells["colScore"].Value?.ToString(), out decimal score))
                    {
                        pgbScoreProgress.Value++;
                        continue;
                    }

                    int studentId = Convert.ToInt32(row.Cells["colStudentId"].Value);

                    var dto = new CreateMonthlyScoreDto
                    {
                        StudentId = studentId,
                        ClassSubjectId = classSubjectId,
                        Month = month,
                        SchoolYear = year,
                        FinalScore = score
                    };

                    await ApiClient.Instance.PostAsync<MonthlyScoreResponseDto>(
                        "/api/scores/monthly", dto);

                    row.Cells["colLocked"].Value = true;
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;

                    pgbScoreProgress.Value++;
                }

                pgbScoreProgress.Visible = false;

                MessageBox.Show("Submit successful.");
            }
            catch (Exception ex)
            {
                pgbScoreProgress.Visible = false;
                MessageBox.Show($"Submit failed:\n{ex.Message}");
            }
        }

        private async void nudMonth_ValueChanged(object sender, EventArgs e)
        {
            await LoadExistingScoresAsync();
        }

        private async void nudYear_ValueChanged(object sender, EventArgs e)
        {
            await LoadExistingScoresAsync();
        }

        private void dgvScores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgvScores.Rows[e.RowIndex];

            if (row.Cells["colLocked"].Value != null &&
                Convert.ToBoolean(row.Cells["colLocked"].Value))
            {
                e.CellStyle.BackColor = Color.LightGray;
                e.CellStyle.ForeColor = Color.DarkGray;
            }
        }

        private void dgvScores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void pgbScoreProgress_Click(object sender, EventArgs e)
        {
        }
    }
}
