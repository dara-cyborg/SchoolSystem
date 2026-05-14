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

        private void dgvAttendance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvAttendance.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                var row = dgvAttendance.Rows[e.RowIndex];

                row.DefaultCellStyle.BackColor = Color.White;

                if (status == AttendanceStatus.Present.ToString())
                    row.DefaultCellStyle.BackColor = Color.LightGreen;

                else if (status == AttendanceStatus.InformedAbsent.ToString())
                    row.DefaultCellStyle.BackColor = Color.Khaki;

                else if (status == AttendanceStatus.UninformedAbsent.ToString())
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
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

            dgvAttendance.Refresh();
        }

        private async void btnSubmitAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboClassSubject.SelectedValue == null)
                {
                    MessageBox.Show("Please select class subject.");
                    return;
                }

                int classSubjectId = Convert.ToInt32(cboClassSubject.SelectedValue);

                BulkAttendanceDto bulkDto = new BulkAttendanceDto();

                foreach (DataGridViewRow row in dgvAttendance.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells["StudentId"].Value == null) continue;

                    int studentId = Convert.ToInt32(row.Cells["StudentId"].Value);

                    if (!Enum.TryParse(
                        row.Cells["Status"].Value?.ToString(),
                        out AttendanceStatus status))
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
                    MessageBox.Show("No records.");
                    return;
                }

                await ApiClient.Instance.PostAsync<object>(
                    "/api/attendance/bulk",
                    bulkDto);

                MessageBox.Show("Attendance saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Submit failed:\n" + ex.Message);
            }
        }

        private async void ucAttendance_Load(object sender, EventArgs e)
        {
            await LoadClassSubjectsAsync();
            SetupGrid();
            if (cboClassSubject.Items.Count > 0)
            {
                cboClassSubject.SelectedIndex = 0;
                LoadStudentsToGrid();
            }
        }

        private async Task LoadClassSubjectsAsync()
        {
            try
            {
                cboClassSubject.DisplayMember = "DisplayText";
                cboClassSubject.ValueMember = "Id";

                var result = await ApiClient.Instance
                    .GetAsync<List<ClassSubjectWithStudentsDto>>("/api/teachers/my-classsubjects");

                if (result == null)
                    return;

                _classSubjects = result;

                var items = _classSubjects.Select(x => new
                {
                    x.Id,
                    DisplayText = $"{x.ClassName} - {x.SubjectName}"
                }).ToList();

                cboClassSubject.DataSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load subjects failed: " + ex.Message);
            }
        }
        private void SetupGrid()
        {
            dgvAttendance.AutoGenerateColumns = false;
            dgvAttendance.Columns.Clear();

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
                Width = 180
            };

            statusColumn.Items.AddRange(Enum.GetNames(typeof(AttendanceStatus)));
            dgvAttendance.Columns.Add(statusColumn);
        }

       
        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentsToGrid();
        }
        private void LoadStudentsToGrid()
        {
            try
            {
                dgvAttendance.Rows.Clear();

                if (cboClassSubject.SelectedValue == null)
                    return;

                int classSubjectId = Convert.ToInt32(cboClassSubject.SelectedValue);

                var selected = _classSubjects.FirstOrDefault(x => x.Id == classSubjectId);

                if (selected == null)
                    return;

                foreach (var student in selected.Students)
                {
                    dgvAttendance.Rows.Add(
                        student.Id,
                        student.Name,
                        student.Sex,
                        AttendanceStatus.Present.ToString()
                    );
                }
            }
            catch
            {
            }
        }
        private void cboClassSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentsToGrid();
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvAttendance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}