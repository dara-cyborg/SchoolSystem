using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.Enums;
using SchoolSystem.Desktop.Forms.Admin;
using SchoolSystem.Desktop.Forms.Auth;
using SchoolSystem.Desktop.Forms.Homeroom;
using SchoolSystem.Desktop.Forms.Teacher;
using SchoolSystem.Desktop.Services;

namespace SchoolSystem.Desktop.Forms {
    public partial class frmMain : Form {
        private MenuStrip msMain = null!;
        private StatusStrip ssStatus = null!;
        private ToolStripStatusLabel tsslUserInfo = null!;
        private ToolStripStatusLabel tsslStatus = null!;
        private TabControl tcMain = null!;
        private ContextMenuStrip cmsTab = null!;
        private TabPage tpDashboard = null!;
        private Panel pnlDashboard = null!;
        private ToolStripMenuItem tsmiAdmin = null!;
        private ToolStripMenuItem tsmiTeacher = null!;
        private ToolStripMenuItem tsmiHomeroom = null!;
        private ToolStripMenuItem tsmiAccount = null!;
        private Label lblStatClassesValue = null!;
        private Label lblStatClassesLabel = null!;
        private Label lblStatUsersValue = null!;
        private Label lblStatUsersLabel = null!;
        private Label lblStatSubjectsValue = null!;
        private Label lblStatSubjectsLabel = null!;
        private Label lblStatStudentsValue = null!;
        private Label lblStatStudentsLabel = null!;
        private ToolStripMenuItem tsmiUsers = null!;
        private ToolStripMenuItem tsmiClasses = null!;
        private ToolStripMenuItem tsmiSubjects = null!;
        private ToolStripMenuItem tsmiGradebook = null!;
        private ToolStripMenuItem tsmiAttendance = null!;
        private ToolStripMenuItem tsmiScoreSubmit = null!;
        private ToolStripMenuItem tsmiClassOverview = null!;
        private ToolStripMenuItem tsmiReportSubmit = null!;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem tsmiCloseTab;
        private ToolStripMenuItem tsmiLogout = null!;

        public frmMain() {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents() {
            Load += frmMain_Load;

            tcMain.MouseDown += tcMain_MouseDown;

            tsmiUsers.Click += tsmiUsers_Click;
            tsmiClasses.Click += tsmiClasses_Click;
            tsmiSubjects.Click += tsmiSubjects_Click;
            tsmiGradebook.Click += tsmiGradebook_Click;
            tsmiAttendance.Click += tsmiAttendance_Click;
            tsmiScoreSubmit.Click += tsmiScoreSubmit_Click;
            tsmiClassOverview.Click += tsmiClassOverview_Click;
            tsmiReportSubmit.Click += tsmiReportSubmit_Click;
            tsmiLogout.Click += tsmiLogout_Click;
        }

        private async void frmMain_Load(object? sender, EventArgs e) {
            ApplyRoleVisibility();
            UpdateUserInfoLabel();
            await LoadDashboardAsync();
        }

        private void ApplyRoleVisibility() {
            var roles = GetCurrentRoles();

            var canSeeAdmin = roles.Contains(RoleName.SuperAdmin);
            var canSeeTeacher = roles.Contains(RoleName.SuperAdmin) || roles.Contains(RoleName.Teacher) || roles.Contains(RoleName.Homeroom);
            var canSeeHomeroom = roles.Contains(RoleName.SuperAdmin) || roles.Contains(RoleName.Homeroom);

            tsmiUsers.Visible = canSeeAdmin;
            tsmiClasses.Visible = canSeeAdmin;
            tsmiSubjects.Visible = canSeeAdmin;

            tsmiGradebook.Visible = canSeeTeacher;
            tsmiAttendance.Visible = canSeeTeacher;
            tsmiScoreSubmit.Visible = canSeeTeacher;

            tsmiClassOverview.Visible = canSeeHomeroom;
            tsmiReportSubmit.Visible = canSeeHomeroom;

            tsmiAdmin.Visible = canSeeAdmin;
            tsmiTeacher.Visible = canSeeTeacher;
            tsmiHomeroom.Visible = canSeeHomeroom;

            tsmiAccount.Visible = true;
        }

        private async Task LoadDashboardAsync() {
            tsslStatus.Text = "Loading dashboard...";

            var roles = GetCurrentRoles();
            var canSeeUsers = roles.Contains(RoleName.SuperAdmin);
            var canSeeClasses = roles.Contains(RoleName.SuperAdmin) || roles.Contains(RoleName.Homeroom);
            var canSeeStudents = roles.Contains(RoleName.SuperAdmin) || roles.Contains(RoleName.Homeroom);
            var canSeeSubjects = roles.Contains(RoleName.SuperAdmin);

            SetStatVisibility(lblStatUsersLabel, lblStatUsersValue, canSeeUsers);
            SetStatVisibility(lblStatClassesLabel, lblStatClassesValue, canSeeClasses);
            SetStatVisibility(lblStatStudentsLabel, lblStatStudentsValue, canSeeStudents);
            SetStatVisibility(lblStatSubjectsLabel, lblStatSubjectsValue, canSeeSubjects);

            if (canSeeUsers) {
                var count = await GetUserCountAsync();
                lblStatUsersValue.Text = count?.ToString() ?? "-";
            }

            if (canSeeClasses) {
                var count = await GetPagedItemCountAsync("/api/classes");
                lblStatClassesValue.Text = count?.ToString() ?? "-";
            }

            if (canSeeStudents) {
                var count = await GetPagedItemCountAsync("/api/students");
                lblStatStudentsValue.Text = count?.ToString() ?? "-";
            }

            if (canSeeSubjects) {
                var count = await GetPagedItemCountAsync("/api/subjects");
                lblStatSubjectsValue.Text = count?.ToString() ?? "-";
            }

            tsslStatus.Text = "Ready";
        }

        private async Task<int?> GetUserCountAsync() {
            try {
                var response = await ApiClient.Instance.GetAsync<PagedResult<object>>("/api/users?page=1&pageSize=1");
                if (response is null) {
                    return 0;
                }

                if (response.TotalCount > 0) {
                    return response.TotalCount;
                }

                return response.Items.Count;
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch {
                return null;
            }
        }

        private async Task<int?> GetPagedItemCountAsync(string endpoint) {
            try {
                var response = await ApiClient.Instance.GetAsync<PagedResult<object>>(
                    endpoint.Contains('?')
                        ? endpoint + "&page=1&pageSize=1"
                        : endpoint + "?page=1&pageSize=1"
                );
                if (response is null) return 0;
                if (response.TotalCount > 0) return response.TotalCount;
                return response.Items.Count;
            }
            catch (InvalidOperationException ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
            catch {
                return null;
            }
        }

        private void SetStatVisibility(Label label, Label value, bool visible) {
            label.Visible = visible;
            value.Visible = visible;

            if (!visible) {
                value.Text = string.Empty;
            }
        }

        private void tsmiUsers_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucUserManagement>();
        }

        private void tsmiClasses_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucClassManagement>();
        }

        private void tsmiSubjects_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucSubjectManagement>();
        }

        private void tsmiGradebook_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucGradebook>();
        }

        private void tsmiAttendance_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucAttendance>();
        }

        private void tsmiScoreSubmit_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucScoreSubmit>();
        }

        private void tsmiClassOverview_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucClassOverview>();
        }

        private void tsmiReportSubmit_Click(object? sender, EventArgs e) {
            OpenOrActivateTab<ucReportSubmit>();
        }

        private void OpenOrActivateTab<TControl>() where TControl : UserControl, new() {
            var controlType = typeof(TControl);

            foreach (TabPage tab in tcMain.TabPages) {
                if (tab.Tag is Type tabType && tabType == controlType) {
                    tcMain.SelectedTab = tab;
                    return;
                }
            }

            var tabName = controlType.Name;
            if (tabName.StartsWith("uc", StringComparison.Ordinal)) {
                tabName = tabName.Substring(2);
            }

            var newTab = new TabPage {
                Text = tabName,
                Tag = controlType
            };

            var control = new TControl {
                Dock = DockStyle.Fill
            };

            newTab.Controls.Add(control);
            tcMain.TabPages.Add(newTab);
            tcMain.SelectedTab = newTab;
        }

        private void tcMain_MouseDown(object? sender, MouseEventArgs e) {
            if (sender is not TabControl tc) return;
            if (e.Button != MouseButtons.Right) return;

            for (var i = 0; i < tc.TabPages.Count; i++) {
                if (tc.TabPages[i] == tpDashboard) continue;
                if (!tc.GetTabRect(i).Contains(e.Location)) continue;

                tc.SelectedIndex = i;
                cmsTab.Tag = tc.TabPages[i];
                cmsTab.Show(tc, e.Location);
                return;
            }
        }

        private void tsmiCloseTab_Click(object? sender, EventArgs e) {
            if (cmsTab.Tag is not TabPage tab) return;
            tcMain.TabPages.Remove(tab);
            tab.Dispose();
        }

        private async void tsmiLogout_Click(object? sender, EventArgs e) {
            ApiClient.Instance.Logout();

            var tabsToRemove = tcMain.TabPages
                .Cast<TabPage>()
                .Where(tp => tp != tpDashboard)
                .ToList();

            foreach (var tab in tabsToRemove) {
                tcMain.TabPages.Remove(tab);
                tab.Dispose();
            }

            Hide();

            using var loginForm = new frmLogin();
            var result = loginForm.ShowDialog();

            if (result == DialogResult.OK) {
                Show();
                ApplyRoleVisibility();
                UpdateUserInfoLabel();
                await LoadDashboardAsync();
                return;
            }

            Application.Exit();
        }

        private void UpdateUserInfoLabel() {
            var joinedRoles = string.Join(", ", ApiClient.Instance.Roles);
            tsslUserInfo.Text = $"Logged in as: {ApiClient.Instance.UserName} ({joinedRoles})";
        }

        private HashSet<RoleName> GetCurrentRoles() {
            var roles = new HashSet<RoleName>();

            foreach (var role in ApiClient.Instance.Roles) {
                if (Enum.TryParse<RoleName>(role, true, out var parsedRole)) {
                    roles.Add(parsedRole);
                }
            }

            return roles;
        }

        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            msMain = new MenuStrip();
            tsmiAdmin = new ToolStripMenuItem();
            tsmiUsers = new ToolStripMenuItem();
            tsmiClasses = new ToolStripMenuItem();
            tsmiSubjects = new ToolStripMenuItem();
            tsmiTeacher = new ToolStripMenuItem();
            tsmiGradebook = new ToolStripMenuItem();
            tsmiAttendance = new ToolStripMenuItem();
            tsmiScoreSubmit = new ToolStripMenuItem();
            tsmiHomeroom = new ToolStripMenuItem();
            tsmiClassOverview = new ToolStripMenuItem();
            tsmiReportSubmit = new ToolStripMenuItem();
            tsmiAccount = new ToolStripMenuItem();
            tsmiLogout = new ToolStripMenuItem();
            ssStatus = new StatusStrip();
            tsslUserInfo = new ToolStripStatusLabel();
            tsslStatus = new ToolStripStatusLabel();
            tcMain = new TabControl();
            tpDashboard = new TabPage();
            pnlDashboard = new Panel();
            lblStatSubjectsValue = new Label();
            lblStatSubjectsLabel = new Label();
            lblStatStudentsValue = new Label();
            lblStatStudentsLabel = new Label();
            lblStatClassesValue = new Label();
            lblStatClassesLabel = new Label();
            lblStatUsersValue = new Label();
            lblStatUsersLabel = new Label();
            cmsTab = new ContextMenuStrip(components);
            tsmiCloseTab = new ToolStripMenuItem();
            msMain.SuspendLayout();
            ssStatus.SuspendLayout();
            tcMain.SuspendLayout();
            tpDashboard.SuspendLayout();
            pnlDashboard.SuspendLayout();
            cmsTab.SuspendLayout();
            SuspendLayout();
            // 
            // msMain
            // 
            msMain.ImageScalingSize = new Size(20, 20);
            msMain.Items.AddRange(new ToolStripItem[] { tsmiAdmin, tsmiTeacher, tsmiHomeroom, tsmiAccount });
            msMain.Location = new Point(0, 0);
            msMain.Name = "msMain";
            msMain.Size = new Size(760, 28);
            msMain.TabIndex = 0;
            msMain.Text = "menuStrip1";
            // 
            // tsmiAdmin
            // 
            tsmiAdmin.DropDownItems.AddRange(new ToolStripItem[] { tsmiUsers, tsmiClasses, tsmiSubjects });
            tsmiAdmin.Name = "tsmiAdmin";
            tsmiAdmin.Size = new Size(114, 24);
            tsmiAdmin.Text = "Administrator";
            // 
            // tsmiUsers
            // 
            tsmiUsers.Name = "tsmiUsers";
            tsmiUsers.Size = new Size(147, 26);
            tsmiUsers.Text = "Users";
            // 
            // tsmiClasses
            // 
            tsmiClasses.Name = "tsmiClasses";
            tsmiClasses.Size = new Size(147, 26);
            tsmiClasses.Text = "Classes";
            // 
            // tsmiSubjects
            // 
            tsmiSubjects.Name = "tsmiSubjects";
            tsmiSubjects.Size = new Size(147, 26);
            tsmiSubjects.Text = "Subjects";
            // 
            // tsmiTeacher
            // 
            tsmiTeacher.DropDownItems.AddRange(new ToolStripItem[] { tsmiGradebook, tsmiAttendance, tsmiScoreSubmit });
            tsmiTeacher.Name = "tsmiTeacher";
            tsmiTeacher.Size = new Size(74, 24);
            tsmiTeacher.Text = "Teacher";
            // 
            // tsmiGradebook
            // 
            tsmiGradebook.Name = "tsmiGradebook";
            tsmiGradebook.Size = new Size(180, 26);
            tsmiGradebook.Text = "Gradebook";
            // 
            // tsmiAttendance
            // 
            tsmiAttendance.Name = "tsmiAttendance";
            tsmiAttendance.Size = new Size(180, 26);
            tsmiAttendance.Text = "Attendance";
            // 
            // tsmiScoreSubmit
            // 
            tsmiScoreSubmit.Name = "tsmiScoreSubmit";
            tsmiScoreSubmit.Size = new Size(180, 26);
            tsmiScoreSubmit.Text = "Submit Score";
            // 
            // tsmiHomeroom
            // 
            tsmiHomeroom.DropDownItems.AddRange(new ToolStripItem[] { tsmiClassOverview, tsmiReportSubmit });
            tsmiHomeroom.Name = "tsmiHomeroom";
            tsmiHomeroom.Size = new Size(100, 24);
            tsmiHomeroom.Text = "Homeroom";
            // 
            // tsmiClassOverview
            // 
            tsmiClassOverview.Name = "tsmiClassOverview";
            tsmiClassOverview.Size = new Size(190, 26);
            tsmiClassOverview.Text = "Class Overview";
            // 
            // tsmiReportSubmit
            // 
            tsmiReportSubmit.Name = "tsmiReportSubmit";
            tsmiReportSubmit.Size = new Size(190, 26);
            tsmiReportSubmit.Text = "Submit Report";
            // 
            // tsmiAccount
            // 
            tsmiAccount.DropDownItems.AddRange(new ToolStripItem[] { tsmiLogout });
            tsmiAccount.Name = "tsmiAccount";
            tsmiAccount.Size = new Size(77, 24);
            tsmiAccount.Text = "Account";
            // 
            // tsmiLogout
            // 
            tsmiLogout.Name = "tsmiLogout";
            tsmiLogout.Size = new Size(139, 26);
            tsmiLogout.Text = "Logout";
            // 
            // ssStatus
            // 
            ssStatus.ImageScalingSize = new Size(20, 20);
            ssStatus.Items.AddRange(new ToolStripItem[] { tsslUserInfo, tsslStatus });
            ssStatus.Location = new Point(0, 466);
            ssStatus.Name = "ssStatus";
            ssStatus.Size = new Size(760, 26);
            ssStatus.TabIndex = 1;
            ssStatus.Text = "statusStrip1";
            // 
            // tsslUserInfo
            // 
            tsslUserInfo.Name = "tsslUserInfo";
            tsslUserInfo.Size = new Size(119, 20);
            tsslUserInfo.Text = "Username - Role";
            // 
            // tsslStatus
            // 
            tsslStatus.Name = "tsslStatus";
            tsslStatus.Size = new Size(149, 20);
            tsslStatus.Text = "Loading Dashboard...";
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tpDashboard);
            tcMain.Dock = DockStyle.Fill;
            tcMain.Location = new Point(0, 28);
            tcMain.Name = "tcMain";
            tcMain.SelectedIndex = 0;
            tcMain.Size = new Size(760, 438);
            tcMain.TabIndex = 2;
            // 
            // tpDashboard
            // 
            tpDashboard.Controls.Add(pnlDashboard);
            tpDashboard.Location = new Point(4, 29);
            tpDashboard.Name = "tpDashboard";
            tpDashboard.Padding = new Padding(3);
            tpDashboard.Size = new Size(752, 405);
            tpDashboard.TabIndex = 0;
            tpDashboard.Text = "Dashboard";
            tpDashboard.UseVisualStyleBackColor = true;
            // 
            // pnlDashboard
            // 
            pnlDashboard.Controls.Add(lblStatSubjectsValue);
            pnlDashboard.Controls.Add(lblStatSubjectsLabel);
            pnlDashboard.Controls.Add(lblStatStudentsValue);
            pnlDashboard.Controls.Add(lblStatStudentsLabel);
            pnlDashboard.Controls.Add(lblStatClassesValue);
            pnlDashboard.Controls.Add(lblStatClassesLabel);
            pnlDashboard.Controls.Add(lblStatUsersValue);
            pnlDashboard.Controls.Add(lblStatUsersLabel);
            pnlDashboard.Dock = DockStyle.Fill;
            pnlDashboard.Location = new Point(3, 3);
            pnlDashboard.Name = "pnlDashboard";
            pnlDashboard.Size = new Size(746, 399);
            pnlDashboard.TabIndex = 0;
            // 
            // lblStatSubjectsValue
            // 
            lblStatSubjectsValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatSubjectsValue.AutoSize = true;
            lblStatSubjectsValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblStatSubjectsValue.ForeColor = SystemColors.Highlight;
            lblStatSubjectsValue.Location = new Point(591, 75);
            lblStatSubjectsValue.Name = "lblStatSubjectsValue";
            lblStatSubjectsValue.Size = new Size(60, 46);
            lblStatSubjectsValue.TabIndex = 7;
            lblStatSubjectsValue.Text = "00";
            // 
            // lblStatSubjectsLabel
            // 
            lblStatSubjectsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatSubjectsLabel.AutoSize = true;
            lblStatSubjectsLabel.Font = new Font("Segoe UI", 12F);
            lblStatSubjectsLabel.Location = new Point(591, 33);
            lblStatSubjectsLabel.Name = "lblStatSubjectsLabel";
            lblStatSubjectsLabel.Size = new Size(136, 28);
            lblStatSubjectsLabel.TabIndex = 6;
            lblStatSubjectsLabel.Text = "Total Subjects:";
            // 
            // lblStatStudentsValue
            // 
            lblStatStudentsValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatStudentsValue.AutoSize = true;
            lblStatStudentsValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblStatStudentsValue.ForeColor = SystemColors.Highlight;
            lblStatStudentsValue.Location = new Point(376, 75);
            lblStatStudentsValue.Name = "lblStatStudentsValue";
            lblStatStudentsValue.Size = new Size(60, 46);
            lblStatStudentsValue.TabIndex = 5;
            lblStatStudentsValue.Text = "00";
            // 
            // lblStatStudentsLabel
            // 
            lblStatStudentsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatStudentsLabel.AutoSize = true;
            lblStatStudentsLabel.Font = new Font("Segoe UI", 12F);
            lblStatStudentsLabel.Location = new Point(376, 33);
            lblStatStudentsLabel.Name = "lblStatStudentsLabel";
            lblStatStudentsLabel.Size = new Size(139, 28);
            lblStatStudentsLabel.TabIndex = 4;
            lblStatStudentsLabel.Text = "Total Students:";
            // 
            // lblStatClassesValue
            // 
            lblStatClassesValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatClassesValue.AutoSize = true;
            lblStatClassesValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblStatClassesValue.ForeColor = SystemColors.Highlight;
            lblStatClassesValue.Location = new Point(190, 75);
            lblStatClassesValue.Name = "lblStatClassesValue";
            lblStatClassesValue.Size = new Size(60, 46);
            lblStatClassesValue.TabIndex = 3;
            lblStatClassesValue.Text = "00";
            // 
            // lblStatClassesLabel
            // 
            lblStatClassesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatClassesLabel.AutoSize = true;
            lblStatClassesLabel.Font = new Font("Segoe UI", 12F);
            lblStatClassesLabel.Location = new Point(190, 33);
            lblStatClassesLabel.Name = "lblStatClassesLabel";
            lblStatClassesLabel.Size = new Size(124, 28);
            lblStatClassesLabel.TabIndex = 2;
            lblStatClassesLabel.Text = "Total Classes:";
            // 
            // lblStatUsersValue
            // 
            lblStatUsersValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatUsersValue.AutoSize = true;
            lblStatUsersValue.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblStatUsersValue.ForeColor = SystemColors.Highlight;
            lblStatUsersValue.Location = new Point(22, 75);
            lblStatUsersValue.Name = "lblStatUsersValue";
            lblStatUsersValue.Size = new Size(60, 46);
            lblStatUsersValue.TabIndex = 1;
            lblStatUsersValue.Text = "00";
            // 
            // lblStatUsersLabel
            // 
            lblStatUsersLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            lblStatUsersLabel.AutoSize = true;
            lblStatUsersLabel.Font = new Font("Segoe UI", 12F);
            lblStatUsersLabel.Location = new Point(22, 33);
            lblStatUsersLabel.Name = "lblStatUsersLabel";
            lblStatUsersLabel.Size = new Size(110, 28);
            lblStatUsersLabel.TabIndex = 0;
            lblStatUsersLabel.Text = "Total Users:";
            // 
            // cmsTab
            // 
            cmsTab.ImageScalingSize = new Size(20, 20);
            cmsTab.Items.AddRange(new ToolStripItem[] { tsmiCloseTab });
            cmsTab.Name = "cmsTab";
            cmsTab.Size = new Size(115, 28);
            // 
            // tsmiCloseTab
            // 
            tsmiCloseTab.Name = "tsmiCloseTab";
            tsmiCloseTab.Size = new Size(114, 24);
            tsmiCloseTab.Text = "Close";
            tsmiCloseTab.Click += tsmiCloseTab_Click;
            // 
            // frmMain
            // 
            ClientSize = new Size(760, 492);
            Controls.Add(tcMain);
            Controls.Add(ssStatus);
            Controls.Add(msMain);
            MainMenuStrip = msMain;
            Name = "frmMain";
            Text = "School Information System - Dashboard";
            WindowState = FormWindowState.Maximized;
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            ssStatus.ResumeLayout(false);
            ssStatus.PerformLayout();
            tcMain.ResumeLayout(false);
            tpDashboard.ResumeLayout(false);
            pnlDashboard.ResumeLayout(false);
            pnlDashboard.PerformLayout();
            cmsTab.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
