using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Auth;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.Grade;
using SchoolSystem.Core.Enums;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class frmClassDialog : Form
    {
        private readonly ClassResponseDto? _class;

        public frmClassDialog(ClassResponseDto? classItem = null)
        {
            InitializeComponent();
            _class = classItem;
            this.Text = _class == null ? "Add New Class" : "Edit Class";
        }

        private async void frmClassDialog_Load(object sender, EventArgs e)
        {
            await LoadDropdowns();

            if (_class != null)
            {
                txtName.Text = _class.Name;
                cboGrade.SelectedValue = _class.GradeId;
                nudSchoolYear.Value = _class.SchoolYear;
                cboHomeroom.SelectedValue = _class.HomeroomUserId ?? 0;
            }
        }

        private async Task LoadDropdowns()
        {
            try
            {
                // 1. Fetch Grades - Matches AcademicService.GetGradesAsync
                var gradeData = await ApiClient.Instance.GetAsync<PagedResult<GradeResponseDto>>("/api/grades?pageSize=100");
                if (gradeData?.Items != null)
                {
                    cboGrade.DataSource = gradeData.Items.ToList();
                    cboGrade.DisplayMember = "Name";
                    cboGrade.ValueMember = "Id";
                }

                // 2. Fetch Users and filter for Teachers
                var userData = await ApiClient.Instance.GetAsync<PagedResult<UserResponseDto>>("/api/users?page=1&pageSize=100");
                if (userData?.Items != null)
                {
                    var teacherList = userData.Items
                        .Where(u => u.Roles != null && u.Roles.Contains(RoleName.Teacher))
                        .ToList();

                    teacherList.Insert(0, new UserResponseDto { Id = 0, Name = "-- Select Homeroom Teacher --" });

                    cboHomeroom.DataSource = teacherList;
                    cboHomeroom.DisplayMember = "Name";
                    cboHomeroom.ValueMember = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            if (cboGrade.SelectedValue == null || (int)cboGrade.SelectedValue == 0)
            {
                MessageBox.Show("Please select a grade.");
                return;
            }

            try
            {
                int? homeroomId = (int)cboHomeroom.SelectedValue == 0 ? null : (int?)cboHomeroom.SelectedValue;

                if (_class == null) // POST /api/classes
                {
                    var dto = new CreateClassDto
                    {
                        Name = txtName.Text.Trim(),
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PostAsync<ClassResponseDto>("/api/classes", dto);
                }
                else // PUT /api/classes/{id}
                {
                    // Matches your UpdateClassDto in the backend
                    var dto = new UpdateClassDto
                    {
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        Name = txtName.Text.Trim(),
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PutAsync<object>($"/api/classes/{_class.Id}", dto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // If AcademicService throws InvalidOperationException, it shows here
                MessageBox.Show($"Save failed: {ex.Message}");
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            if (cboGrade.SelectedValue == null || (int)cboGrade.SelectedValue == 0)
            {
                MessageBox.Show("Please select a grade.");
                return;
            }

            try
            {
                int? homeroomId = (int)cboHomeroom.SelectedValue == 0 ? null : (int?)cboHomeroom.SelectedValue;

                if (_class == null) // POST /api/classes
                {
                    var dto = new CreateClassDto
                    {
                        Name = txtName.Text.Trim(),
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PostAsync<ClassResponseDto>("/api/classes", dto);
                }
                else // PUT /api/classes/{id}
                {
                    // Matches your UpdateClassDto in the backend
                    var dto = new UpdateClassDto
                    {
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        Name = txtName.Text.Trim(),
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PutAsync<object>($"/api/classes/{_class.Id}", dto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // If AcademicService throws InvalidOperationException, it shows here
                MessageBox.Show($"Save failed: {ex.Message}");
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            if (cboGrade.SelectedValue == null || (int)cboGrade.SelectedValue == 0)
            {
                MessageBox.Show("Please select a grade.");
                return;
            }

            try
            {
                int? homeroomId = (int)cboHomeroom.SelectedValue == 0 ? null : (int?)cboHomeroom.SelectedValue;

                if (_class == null) // POST /api/classes
                {
                    var dto = new CreateClassDto
                    {
                        Name = txtName.Text.Trim(),
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PostAsync<ClassResponseDto>("/api/classes", dto);
                }
                else // PUT /api/classes/{id}
                {
                    // Matches your UpdateClassDto in the backend
                    var dto = new UpdateClassDto
                    {
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        Name = txtName.Text.Trim(),
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PutAsync<object>($"/api/classes/{_class.Id}", dto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // If AcademicService throws InvalidOperationException, it shows here
                MessageBox.Show($"Save failed: {ex.Message}");
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            if (cboGrade.SelectedValue == null || (int)cboGrade.SelectedValue == 0)
            {
                MessageBox.Show("Please select a grade.");
                return;
            }

            try
            {
                int? homeroomId = (int)cboHomeroom.SelectedValue == 0 ? null : (int?)cboHomeroom.SelectedValue;

                if (_class == null) // POST /api/classes
                {
                    var dto = new CreateClassDto
                    {
                        Name = txtName.Text.Trim(),
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PostAsync<ClassResponseDto>("/api/classes", dto);
                }
                else // PUT /api/classes/{id}
                {
                    // Matches your UpdateClassDto in the backend
                    var dto = new UpdateClassDto
                    {
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        Name = txtName.Text.Trim(),
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PutAsync<object>($"/api/classes/{_class.Id}", dto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // If AcademicService throws InvalidOperationException, it shows here
                MessageBox.Show($"Save failed: {ex.Message}");
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a class name.");
                return;
            }

            if (cboGrade.SelectedValue == null || (int)cboGrade.SelectedValue == 0)
            {
                MessageBox.Show("Please select a grade.");
                return;
            }

            try
            {
                int? homeroomId = (int)cboHomeroom.SelectedValue == 0 ? null : (int?)cboHomeroom.SelectedValue;

                if (_class == null) // POST /api/classes
                {
                    var dto = new CreateClassDto
                    {
                        Name = txtName.Text.Trim(),
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PostAsync<ClassResponseDto>("/api/classes", dto);
                }
                else // PUT /api/classes/{id}
                {
                    // Matches your UpdateClassDto in the backend
                    var dto = new UpdateClassDto
                    {
                        GradeId = (int)cboGrade.SelectedValue,
                        SchoolYear = (short)nudSchoolYear.Value,
                        Name = txtName.Text.Trim(),
                        HomeroomUserId = homeroomId
                    };
                    await ApiClient.Instance.PutAsync<object>($"/api/classes/{_class.Id}", dto);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // If AcademicService throws InvalidOperationException, it shows here
                MessageBox.Show($"Save failed: {ex.Message}");
            }
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void cboGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (_class == null && string.IsNullOrWhiteSpace(txtName.Text))
            {
                if (cboGrade.SelectedItem is GradeResponseDto selectedGrade)
                {
                   
                    txtName.Text = $"{selectedGrade.Name}-";
                    txtName.Focus();
                    txtName.SelectionStart = txtName.Text.Length;
                }
            }
        }
        

        private void nudSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            int currentYear = DateTime.Now.Year;

            
            if (nudSchoolYear.Value < currentYear)
            {
                nudSchoolYear.ForeColor = Color.Red;
            }
            else
            {
                nudSchoolYear.ForeColor = Color.Black;
            }
        }

        private void cboHomeroom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboHomeroom.SelectedItem is UserResponseDto selectedTeacher)
            {
                int teacherId = selectedTeacher.Id;
                string teacherName = selectedTeacher.Name;

                // use teacherId if needed
            }
        }
        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}