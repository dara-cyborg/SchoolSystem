using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.Report;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Homeroom
{
    public class ReportEntryDto
    {
        public int Rank { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public decimal TotalScore { get; set; }
    }

    public partial class ucReportSubmit : UserControl
    {
        private bool _isInitializing = true;

        public ucReportSubmit()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void SetupGrid()
        {
            dgvReportResult.Columns.Clear();
            dgvReportResult.Columns.Add("Rank", "Rank");
            dgvReportResult.Columns.Add("StudentId", "Student ID");
            dgvReportResult.Columns.Add("StudentName", "Student Name");
            dgvReportResult.Columns.Add("TotalScore", "Total Score");

            dgvReportResult.Columns["StudentId"].Visible = false;

            dgvReportResult.ReadOnly = true;
            dgvReportResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReportResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private async void ucReportSubmit_Load(object sender, EventArgs e)
        {
            _isInitializing = true;

            await LoadClasses();

            cboReportType.Items.Clear();
            cboReportType.Items.AddRange(new string[] { "Monthly", "Semester", "Yearly" });
            cboReportType.SelectedIndex = 0;

            cboSemester.Items.Clear();
            cboSemester.Items.AddRange(new string[] { "1", "2" });
            cboSemester.SelectedIndex = 0;

            // Fix: Month range 1-12
            nudMonth.Minimum = 1;
            nudMonth.Maximum = 12;
            nudMonth.Value = DateTime.Now.Month;

            nudYear.Minimum = 2000;
            nudYear.Maximum = 2100;
            nudYear.Value = DateTime.Now.Year;

            _isInitializing = false;

            await LoadExistingReportAsync();
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
                MessageBox.Show($"Error loading classes: {ex.Message}");
            }
        }

        private async Task LoadExistingReportAsync()
        {
            if (_isInitializing) return;
            if (cboClass.SelectedValue == null) return;

            dgvReportResult.Rows.Clear();

            try
            {
                int classId = (int)cboClass.SelectedValue;
                string type = cboReportType.Text;

                if (type == "Monthly")
                {
                    short month = (short)nudMonth.Value;
                    short year = (short)nudYear.Value;

                    var result = await ApiClient.Instance.GetAsync<MonthlyReportResponseDto>(
                        $"/api/monthly-reports/by-class?classId={classId}&month={month}&schoolYear={year}");

                    if (result != null)
                    {
                        await DisplayResultsWithNames(result.Entries.Select(e => new ReportEntryDto
                        {
                            Rank = (int)e.Rank,
                            StudentId = (int)e.StudentId,
                            TotalScore = (decimal)e.TotalScore
                        }).ToList(), classId);
                    }
                }
                else if (type == "Semester")
                {
                    short semester = short.Parse(cboSemester.Text);
                    short year = (short)nudYear.Value;

                    var result = await ApiClient.Instance.GetAsync<SemesterReportResponseDto>(
                        $"/api/semester-reports/by-class?classId={classId}&semester={semester}&schoolYear={year}");

                    if (result != null)
                    {
                        await DisplayResultsWithNames(result.Entries.Select(e => new ReportEntryDto
                        {
                            Rank = (int)e.Rank,
                            StudentId = (int)e.StudentId,
                            TotalScore = (decimal)e.TotalScore
                        }).ToList(), classId);
                    }
                }
                else if (type == "Yearly")
                {
                    short year = (short)nudYear.Value;

                    var result = await ApiClient.Instance.GetAsync<YearlyReportResponseDto>(
                        $"/api/yearly-reports/by-class?classId={classId}&schoolYear={year}");

                    if (result != null)
                    {
                        await DisplayResultsWithNames(result.Entries.Select(e => new ReportEntryDto
                        {
                            Rank = (int)e.Rank,
                            StudentId = (int)e.StudentId,
                            TotalScore = (decimal)e.TotalScore
                        }).ToList(), classId);
                    }
                }
            }
            catch
            {
                // No existing report found — grid stays empty
            }
        }

        private async Task DisplayResultsWithNames(List<ReportEntryDto> entries, int classId)
        {
            try
            {
                // Load students for this class to get names
                var students = await ApiClient.Instance.GetAsync<List<StudentInReportDto>>(
                    $"/api/classes/{classId}/students");

                dgvReportResult.Rows.Clear();
                foreach (var entry in entries)
                {
                    var studentName = students?.FirstOrDefault(s => s.Id == entry.StudentId)?.Name ?? entry.StudentId.ToString();
                    dgvReportResult.Rows.Add(entry.Rank, entry.StudentId, studentName, entry.TotalScore);
                }

                dgvReportResult.Sort(dgvReportResult.Columns["Rank"], System.ComponentModel.ListSortDirection.Ascending);
            }
            catch
            {
                // Fallback to student ID if name fetch fails
                DisplayResults(entries);
            }
        }

        private async void btnSubmitReport_Click(object sender, EventArgs e)
        {
            if (cboClass.SelectedValue == null)
            {
                MessageBox.Show("Please select a class first.");
                return;
            }

            int classId = (int)cboClass.SelectedValue;
            string type = cboReportType.Text;

            try
            {
                switch (type)
                {
                    case "Monthly":
                        await SubmitMonthly(classId);
                        break;
                    case "Semester":
                        await SubmitSemester(classId);
                        break;
                    case "Yearly":
                        await SubmitYearly(classId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Submission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task SubmitMonthly(int classId)
        {
            var dto = new CreateMonthlyReportDto
            {
                ClassId = classId,
                Month = (short)nudMonth.Value,
                SchoolYear = (short)nudYear.Value
            };

            var result = await ApiClient.Instance.PostAsync<MonthlyReportResponseDto>("/api/monthly-reports/submit", dto);

            if (result != null)
            {
                MessageBox.Show($"Monthly Report for Month {result.Month} submitted and locked!");
                await DisplayResultsWithNames(result.Entries.Select(e => new ReportEntryDto
                {
                    Rank = (int)e.Rank,
                    StudentId = (int)e.StudentId,
                    TotalScore = (decimal)e.TotalScore
                }).ToList(), classId);
            }
        }

        private async Task SubmitSemester(int classId)
        {
            var dto = new CreateSemesterReportDto
            {
                ClassId = classId,
                Semester = short.Parse(cboSemester.Text),
                SchoolYear = (short)nudYear.Value
            };

            var result = await ApiClient.Instance.PostAsync<SemesterReportResponseDto>("/api/semester-reports/submit", dto);

            if (result != null)
            {
                MessageBox.Show($"Semester {result.Semester} Report generated successfully!");
                await DisplayResultsWithNames(result.Entries.Select(e => new ReportEntryDto
                {
                    Rank = (int)e.Rank,
                    StudentId = (int)e.StudentId,
                    TotalScore = (decimal)e.TotalScore
                }).ToList(), classId);
            }
        }

        private async Task SubmitYearly(int classId)
        {
            var dto = new CreateYearlyReportDto
            {
                ClassId = classId,
                SchoolYear = (short)nudYear.Value
            };

            var result = await ApiClient.Instance.PostAsync<YearlyReportResponseDto>("/api/yearly-reports/submit", dto);

            if (result != null)
            {
                MessageBox.Show("Yearly Report generated and rankings computed!");
                await DisplayResultsWithNames(result.Entries.Select(e => new ReportEntryDto
                {
                    Rank = (int)e.Rank,
                    StudentId = (int)e.StudentId,
                    TotalScore = (decimal)e.TotalScore
                }).ToList(), classId);
            }
        }

        private void DisplayResults(List<ReportEntryDto> entries)
        {
            dgvReportResult.Rows.Clear();
            foreach (var entry in entries)
            {
                dgvReportResult.Rows.Add(entry.Rank, entry.StudentId, entry.StudentId.ToString(), entry.TotalScore);
            }

            dgvReportResult.Sort(dgvReportResult.Columns["Rank"], System.ComponentModel.ListSortDirection.Ascending);
        }

        private async void cboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            string type = cboReportType.Text;

            // Show/hide controls based on report type
            nudMonth.Enabled = (type == "Monthly");
            cboSemester.Enabled = (type == "Semester");

            // Auto-adjust month range for semester
            if (type == "Semester")
            {
                UpdateMonthRangeForSemester();
            }
            else if (type == "Monthly")
            {
                nudMonth.Minimum = 1;
                nudMonth.Maximum = 12;
            }

            await LoadExistingReportAsync();
        }

        private void UpdateMonthRangeForSemester()
        {
            if (cboSemester.Text == "1")
            {
                nudMonth.Minimum = 1;
                nudMonth.Maximum = 6;
                nudMonth.Value = 1;
            }
            else
            {
                nudMonth.Minimum = 7;
                nudMonth.Maximum = 12;
                nudMonth.Value = 7;
            }
        }

        private async void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvReportResult.Rows.Clear();
            await LoadExistingReportAsync();
        }

        private async void nudMonth_ValueChanged(object sender, EventArgs e)
        {
            await LoadExistingReportAsync();
        }

        private async void nudYear_ValueChanged(object sender, EventArgs e)
        {
            await LoadExistingReportAsync();
        }

        private async void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            // Auto-adjust month range when semester changes
            UpdateMonthRangeForSemester();

            dgvReportResult.Rows.Clear();
            await LoadExistingReportAsync();
        }

        private void dgvReportResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }

    public class StudentInReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}