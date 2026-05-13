using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.Grade;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class ucClassManagement : UserControl
    {
        // Statement: Guard flag to prevent LoadClasses from firing while we are setting up ComboBoxes
        private bool _isInitializing = true;

        public ucClassManagement()
        {
            InitializeComponent();
        }

        private async void ucClassManagement_Load(object sender, EventArgs e)
        {
            _isInitializing = true;
            await LoadInitialData();
            _isInitializing = false;

            // Now that filters are loaded, fetch the classes
            await LoadClasses();
        }

        private async Task LoadInitialData()
        {
            try
            {
                // Set default year to current
                nudSchoolYear.Value = DateTime.Now.Year;

                // Load Grades for the Filter ComboBox
                var gradeResult = await ApiClient.Instance.GetAsync<PagedResult<GradeResponseDto>>("/api/grades?pageSize=100");

                if (gradeResult?.Items != null)
                {
                    var filterList = gradeResult.Items.ToList();
                    filterList.Insert(0, new GradeResponseDto { Id = 0, Name = "All Grades" });

                    cboGradeFilter.DataSource = filterList;
                    cboGradeFilter.DisplayMember = "Name";
                    cboGradeFilter.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load filters: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadClasses()
        {
            // Statement: Guard against calls during setup or if no value is selected
            if (_isInitializing || cboGradeFilter.SelectedValue == null) return;

            try
            {
                // Statement: Safe cast the SelectedValue to ensure it's an int
                int selectedValue = 0;
                if (cboGradeFilter.SelectedValue is int intVal) selectedValue = intVal;

                int? gradeId = selectedValue == 0 ? null : (int?)selectedValue;
                short schoolYear = (short)nudSchoolYear.Value;

                string url = $"/api/classes?schoolYear={schoolYear}&page=1&pageSize=100";
                if (gradeId.HasValue) url += $"&gradeId={gradeId}";

                var result = await ApiClient.Instance.GetAsync<PagedResult<ClassResponseDto>>(url);

                dgvClasses.DataSource = null; // Statement: Clear binding before refreshing
                if (result?.Items != null)
                {
                    dgvClasses.DataSource = result.Items;
                    FormatGrid(); // Helper to hide ID columns
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading classes: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatGrid()
        {
            // Statement: Ensure technical columns are hidden after data bind
            string[] hiddenFields = { "Id", "GradeId", "HomeroomUserId", "CreatedAt" };
            foreach (var field in hiddenFields)
            {
                if (dgvClasses.Columns.Contains(field))
                    dgvClasses.Columns[field].Visible = false;
            }

            if (dgvClasses.Columns.Contains("HomeroomTeacherName"))
                dgvClasses.Columns["HomeroomTeacherName"].HeaderText = "Homeroom Teacher";
        }

        private async void btnAddClass_Click(object sender, EventArgs e)
        {
            using var frm = new frmClassDialog();
            if (frm.ShowDialog() == DialogResult.OK) await LoadClasses();
        }

        private async void btnEditClass_Click(object sender, EventArgs e)
        {
            if (dgvClasses.CurrentRow?.DataBoundItem is ClassResponseDto item)
            {
                using var frm = new frmClassDialog(item);
                if (frm.ShowDialog() == DialogResult.OK) await LoadClasses();
            }
            else
            {
                MessageBox.Show("Please select a class to edit.");
            }
        }

        private async void btnDeleteClass_Click(object sender, EventArgs e)
        {
            if (dgvClasses.CurrentRow?.DataBoundItem is ClassResponseDto item)
            {
                var confirm = MessageBox.Show($"Delete class {item.Name}?\nThis cannot be undone.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        await ApiClient.Instance.DeleteAsync($"/api/classes/{item.Id}");
                        await LoadClasses();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Delete failed: {ex.Message}");
                    }
                }
            }
        }

        private void btnViewClassStudents_Click(object sender, EventArgs e)
        {
            // Statement: Logic to handle viewing students for the selected class
            if (dgvClasses.CurrentRow?.DataBoundItem is ClassResponseDto item)
            {
                MessageBox.Show($"Functionality to view students for Class: {item.Name} (ID: {item.Id}) will be implemented in the Student Roster module.", "Information");
                // Here you would typically navigate to a student list filtered by this ClassId
            }
        }

        private async void cboGradeFilter_SelectedIndexChanged(object sender, EventArgs e) => await LoadClasses();
        private async void nudSchoolYear_ValueChanged(object sender, EventArgs e) => await LoadClasses();
        private async void btnRefresh_Click(object sender, EventArgs e) => await LoadClasses();
        private void dgvClasses_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}