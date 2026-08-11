using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SchoolSystem.Core.DTOs.Attendance;
using SchoolSystem.Core.DTOs.ClassSubject;
using SchoolSystem.Core.Enums;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Teacher
{
    public partial class ucAttendance : UserControl
    {
        private List<ClassSubjectWithStudentsDto> _classSubjects = new();

        public ucAttendance()
        {
            InitializeComponent();
        }

        private async void ucAttendance_Load(object sender, EventArgs e)
        {
            SetupGrid();
            dtpDate.Value = DateTime.Today;

            await LoadClassSubjectsAsync();

            // force trigger AFTER binding is ready
            if (cboClassSubject.SelectedItem is ComboItem item)
            {
                LoadStudentsToGrid(item.Id);
            }
        }

        private async Task LoadClassSubjectsAsync()
        {
            try
            {
                var result = await ApiClient.Instance
                    .GetAsync<List<ClassSubjectWithStudentsDto>>("/api/teachers/my-classsubjects");

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show("No class subjects found.");
                    return;
                }

                _classSubjects = result;

                var comboData = _classSubjects.Select(x => new ComboItem
                {
                    Id = x.Id,
                    DisplayText = $"{x.ClassName} - {x.SubjectName}"
                }).ToList();

                cboClassSubject.DisplayMember = nameof(ComboItem.DisplayText);
                cboClassSubject.ValueMember = nameof(ComboItem.Id);
                cboClassSubject.DataSource = comboData;

                cboClassSubject.DropDownStyle = ComboBoxStyle.DropDownList;

                if (comboData.Count > 0)
                    cboClassSubject.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load subjects failed:\n" + ex.Message);
            }
        }

        private void SetupGrid()
        {
            dgvAttendance.Columns.Clear();
            dgvAttendance.AutoGenerateColumns = false;
            dgvAttendance.AllowUserToAddRows = false;

            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StudentId",
                HeaderText = "Student ID",
                Visible = false
            });

            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StudentName",
                HeaderText = "Student Name",
                ReadOnly = true,
                Width = 220
            });

            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Sex",
                HeaderText = "Sex",
                ReadOnly = true,
                Width = 80
            });

            var statusColumn = new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 180,
                FlatStyle = FlatStyle.Flat
            };

            statusColumn.Items.AddRange(Enum.GetNames(typeof(AttendanceStatus)));
            dgvAttendance.Columns.Add(statusColumn);
        }

        private void cboClassSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClassSubject.SelectedItem is not ComboItem item)
                return;

            LoadStudentsToGrid(item.Id);
        }

        private void LoadStudentsToGrid(int classSubjectId)
        {
            try
            {
                dgvAttendance.Rows.Clear();

                var selected = _classSubjects.FirstOrDefault(x => x.Id == classSubjectId);

                if (selected?.Students == null)
                    return;

                foreach (var student in selected.Students)
                {
                    dgvAttendance.Rows.Add(
                        student.Id,
                        student.Name,
                        student.Sex.ToString(),
                        AttendanceStatus.Present.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load students failed:\n" + ex.Message);
            }
        }
        private void btnBulkSubmit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvAttendance.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["Status"].Value = AttendanceStatus.Present.ToString();
                }
            }
        }

        private async void btnSubmitAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                // CRITICAL: Commit any pending changes in the grid
                dgvAttendance.EndEdit();

                if (cboClassSubject.SelectedValue == null || !int.TryParse(cboClassSubject.SelectedValue.ToString(), out int classSubjectId))
                {
                    MessageBox.Show("Please select a class subject first.");
                    return;
                }

                BulkAttendanceDto bulkDto = new BulkAttendanceDto();

                foreach (DataGridViewRow row in dgvAttendance.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Safely extract StudentId
                    if (row.Cells["StudentId"].Value == null) continue;
                    int studentId = Convert.ToInt32(row.Cells["StudentId"].Value);

                    // Safely extract and parse Status
                    string statusStr = row.Cells["Status"].Value?.ToString();
                    if (string.IsNullOrEmpty(statusStr) || !Enum.TryParse(statusStr, out AttendanceStatus status))
                    {
                        continue;
                    }

                    bulkDto.Records.Add(new CreateAttendanceDto
                    {
                        StudentId = studentId,
                        ClassSubjectId = classSubjectId,
                        Date = dtpDate.Value.Date,
                        Status = status
                    });
                }

                if (bulkDto.Records.Count == 0)
                {
                    MessageBox.Show("No attendance data found in the list to submit.");
                    return;
                }

                await ApiClient.Instance.PostAsync<object>("/api/attendance/bulk", bulkDto);

                MessageBox.Show("Attendance submitted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Submit failed:\n" + ex.Message);
            }
        }

        private void dgvAttendance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (dgvAttendance.Columns[e.ColumnIndex].Name == "Status")
            {
                var value = e.Value?.ToString();

                if (value == AttendanceStatus.Present.ToString())
                    e.CellStyle.BackColor = Color.LightGreen;
                else if (value == AttendanceStatus.InformedAbsent.ToString())
                    e.CellStyle.BackColor = Color.Khaki;
                else if (value == AttendanceStatus.UninformedAbsent.ToString())
                    e.CellStyle.BackColor = Color.LightCoral;
            }
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dgvAttendance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        public class ComboItem
        {
            public int Id { get; set; }
            public string DisplayText { get; set; } = string.Empty;
        }

        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}