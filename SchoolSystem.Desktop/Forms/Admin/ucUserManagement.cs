using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Auth;
using SchoolSystem.Core.DTOs.User;
using SchoolSystem.Desktop.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Admin
{
    public partial class ucUserManagement : UserControl
    {
        private int _currentPage = 1;
        private int _pageSize = 100;
        private int _totalPages = 1;

        private System.Windows.Forms.Timer _searchTimer;

        public ucUserManagement()
        {
            InitializeComponent();

            _searchTimer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };
            _searchTimer.Tick += SearchTimer_Tick;

            btnRefreshUsers.Click += btnRefreshUsers_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
            btnSearch.Click += btnSearch_Click;
        }

        private async void ucUserManagement_Load(object sender, EventArgs e)
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                string search = txtSearch.Text.Trim();
                string url = $"/api/users?page={_currentPage}&pageSize={_pageSize}";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    url += $"&search={Uri.EscapeDataString(search)}";
                }

                var result = await ApiClient.Instance.GetAsync<PagedResult<UserResponseDto>>(url);

                dgvUsers.DataSource = null;

                if (result?.Items != null)
                {
                    dgvUsers.DataSource = result.Items.ToList();

                    _totalPages = result.TotalPages;

                    FormatUserGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "API Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatUserGrid()
        {
            if (dgvUsers.Columns.Count == 0) return;

            string[] hiddenFields = { "Id", "PasswordHash", "CreatedAt", "UpdatedAt" };

            foreach (var field in hiddenFields)
            {
                if (dgvUsers.Columns.Contains(field))
                    dgvUsers.Columns[field].Visible = false;
            }

            if (dgvUsers.Columns.Contains("Name"))
                dgvUsers.Columns["Name"].HeaderText = "Full Name";

            if (dgvUsers.Columns.Contains("Dob"))
                dgvUsers.Columns["Dob"].HeaderText = "Date of Birth";

            if (dgvUsers.Columns.Contains("IsActive"))
                dgvUsers.Columns["IsActive"].HeaderText = "Status";

            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private async void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _currentPage = 1;
            await LoadUsers();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _currentPage = 1;
            await LoadUsers();
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                _searchTimer.Stop();
                _currentPage = 1;

                await LoadUsers();
            }
        }

        private async void btnRefreshUsers_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            _currentPage = 1;
            await LoadUsers();
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            using var frm = new frmUserDialog();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                await LoadUsers();
            }
        }

        private async void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is UserResponseDto selectedUser)
            {
                using var frm = new frmUserDialog(selectedUser);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await LoadUsers();
                }
            }
            else
            {
                MessageBox.Show("Please select a user first.");
            }
        }

        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is UserResponseDto selectedUser)
            {
                var confirm = MessageBox.Show(
                    $"Are you sure you want to delete {selectedUser.Name}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        await ApiClient.Instance.DeleteAsync($"/api/users/{selectedUser.Id}");

                        MessageBox.Show("User deleted successfully.");

                        await LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Delete failed: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user first.");
            }
        }
    }
}