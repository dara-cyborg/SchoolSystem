using SchoolSystem.Core.DTOs.Auth;
using SchoolSystem.Core.DTOs.User;
using SchoolSystem.Core.Enums;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CreateUserDto = SchoolSystem.Core.DTOs.User.CreateUserDto;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class frmUserDialog : Form
    {
        private readonly UserResponseDto _user;

        public frmUserDialog(UserResponseDto user) : this()
        {
            _user = user;
        }

       
        public frmUserDialog()
        {
            InitializeComponent();
        }

       
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UserName { get { return txtName.Text; } set { txtName.Text = value; } }

        private void frmUserDialog_Load(object sender, EventArgs e)
        {
            cboSex.Items.Clear();
            cboSex.Items.Add("Male");
            cboSex.Items.Add("Female");

            clbRoles.Items.Clear();
            foreach (var role in Enum.GetNames(typeof(RoleName)))
            {
                clbRoles.Items.Add(role);
            }

          
            if (_user != null)
            {
                this.Text = "Edit User";
                txtName.Text = _user.Name;
                cboSex.Text = _user.Sex.ToString();
                
                dtpDob.Value = _user.Dob ?? DateTime.Now;
                txtContact.Text = _user.Contact;
                txtPassword.PlaceholderText = "Leave blank to keep current password";

                chkIsActive.Checked = _user.IsActive;
            }
        }
       
        private async void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter name");
                    return;
                }

                if (_user == null && string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter password");
                    return;
                }
                var dto = new CreateUserDto
                {
                    Name = txtName.Text.Trim(),
                    Sex = cboSex.Text == "Male" ? SexType.Male : SexType.Female,
                    Dob = dtpDob.Value,
                    Contact = txtContact.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    IsActive = chkIsActive.Checked,
                    RoleIds = GetSelectedRoleIds()
                };
                if (_user == null)
                {
                    await ApiClient.Instance.PostAsync<object>("/api/users", dto);
                    MessageBox.Show("User added successfully");
                }
                else
                {
                    var updateDto = new SchoolSystem.Core.DTOs.User.UpdateUserDto
                    {
                        Name = txtName.Text.Trim(),
                        Sex = cboSex.Text == "Male" ? SexType.Male : SexType.Female,
                        Dob = dtpDob.Value,
                        Contact = txtContact.Text.Trim(),
                        IsActive = chkIsActive.Checked
                    };

                    await ApiClient.Instance.PutAsync<object>($"/api/users/{_user.Id}", updateDto);
                    MessageBox.Show("User updated successfully");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private List<int> GetSelectedRoleIds()
        {
            var roleIds = new List<int>();

            foreach (var item in clbRoles.CheckedItems)
            {
                if (Enum.TryParse<RoleName>(item.ToString(), out var role))
                {
                    roleIds.Add((int)role);
                }
            }

            return roleIds;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        
    }
}