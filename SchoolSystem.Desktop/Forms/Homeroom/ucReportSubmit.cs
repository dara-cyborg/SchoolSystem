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
            dgvReportResult.Columns.Add("TotalScore", "Total Score");

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

            nudMonth.Value = DateTime.Now.Month;
            nudYear.Value = DateTime.Now.Year;

            _isInitializing = false;
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
                DisplayResults(result.Entries.Select(e => new ReportEntryDto
                {
                    Rank = (int)e.Rank,
                    StudentId = (int)e.StudentId,
                    TotalScore = (decimal)e.TotalScore
                }).ToList());
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
                DisplayResults(result.Entries.Select(e => new ReportEntryDto
                {
                    Rank = (int)e.Rank,
                    StudentId = (int)e.StudentId,
                    TotalScore = (decimal)e.TotalScore
                }).ToList());
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
                DisplayResults(result.Entries.Select(e => new ReportEntryDto
                {
                    Rank = (int)e.Rank,
                    StudentId = (int)e.StudentId,
                    TotalScore = (decimal)e.TotalScore
                }).ToList());
            }
        }

        private void DisplayResults(List<ReportEntryDto> entries)
        {
            dgvReportResult.Rows.Clear();
            foreach (var entry in entries)
            {
                dgvReportResult.Rows.Add(entry.Rank, entry.StudentId, entry.TotalScore);
            }

            dgvReportResult.Sort(dgvReportResult.Columns["Rank"], System.ComponentModel.ListSortDirection.Ascending);
        }

        private void cboReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            string type = cboReportType.Text;
            nudMonth.Enabled = (type == "Monthly");
            cboSemester.Enabled = (type == "Semester");
        }

        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvReportResult.Rows.Clear();
        }

        private void nudMonth_ValueChanged(object sender, EventArgs e)
        {
            if (nudYear.Value == DateTime.Now.Year && nudMonth.Value > DateTime.Now.Month)
            {
                nudMonth.Value = DateTime.Now.Month;
            }
        }

        private void nudYear_ValueChanged(object sender, EventArgs e)
        {
            if (nudYear.Value > DateTime.Now.Year)
            {
                nudYear.Value = DateTime.Now.Year;
            }
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvReportResult.Rows.Clear();
        }

        private void dgvReportResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}