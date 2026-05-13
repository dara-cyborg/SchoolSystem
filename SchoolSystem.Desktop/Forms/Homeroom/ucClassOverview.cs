using SchoolSystem.Core.DTOs;
using SchoolSystem.Core.DTOs.Class;
using SchoolSystem.Core.DTOs.Student;
using SchoolSystem.Desktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Homeroom;

public partial class ucClassOverview : UserControl
{
    private bool _isInitializing = true;
    public ucClassOverview()
    {
        InitializeComponent();
    }

    private async void ucClassOverview_Load(object sender, EventArgs e)
    {
        _isInitializing = true;
        await LoadFilterClasses();
        _isInitializing = false;

        if (cboClass.Items.Count > 0)
            await RefreshStudentGrid();
    }
    private async Task LoadFilterClasses()
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
            MessageBox.Show($"Class Load Error: {ex.Message}");
        }
    }
    private async Task RefreshStudentGrid()
    {
        if (_isInitializing || cboClass.SelectedValue == null) return;

        try
        {
            int classId = (int)cboClass.SelectedValue;
            var students = await ApiClient.Instance.GetAsync<List<StudentDto>>($"/api/classes/{classId}/students");

            dgvStudents.DataSource = null;
            if (students != null)
            {
                dgvStudents.DataSource = students;
                FormatGrid();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to sync with Backend: {ex.Message}");
        }
    }
    private void FormatGrid()
    {
        if (dgvStudents.Columns.Count == 0) return;

        string[] hiddenColumns = { "Id", "ClassId", "CreatedAt" };
        foreach (var col in hiddenColumns)
        {
            if (dgvStudents.Columns.Contains(col))
                dgvStudents.Columns[col].Visible = false;
        }

        if (dgvStudents.Columns.Contains("Name"))
            dgvStudents.Columns["Name"].HeaderText = "Student Name";

        if (dgvStudents.Columns.Contains("Sex"))
            dgvStudents.Columns["Sex"].HeaderText = "Gender";

        if (dgvStudents.Columns.Contains("Dob"))
        {
            dgvStudents.Columns["Dob"].HeaderText = "Date of Birth";
            dgvStudents.Columns["Dob"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.ReadOnly = true;
        dgvStudents.AllowUserToAddRows = false;
    }
    //private async Task LoadStudents()
    //{
    //    try
    //    {
    //        dgvStudents.AutoGenerateColumns = true;

    //        var students = await ApiClient.Instance
    //            .GetAsync<List<StudentDto>>("/api/students");

    //        dgvStudents.DataSource = students;
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex.Message);
    //    }
    //}

    private async void cboClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        await RefreshStudentGrid();
    }

    private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // Only trigger edit if clicking a valid row (not header)
        if (e.RowIndex >= 0)
        {
            btnEditStudent_Click(sender, e);
        }
    }

    private async void btnAddStudent_Click(object sender, EventArgs e)
    {
        using var frm = new frmStudentDialog();
        if (frm.ShowDialog() == DialogResult.OK) await RefreshStudentGrid();
    }

    private async void btnEditStudent_Click(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow?.DataBoundItem is StudentDto student)
        {
            using var frm = new frmStudentDialog(student);
            if (frm.ShowDialog() == DialogResult.OK) await RefreshStudentGrid();
        }
    }

    private async void btnDeleteStudent_Click(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow?.DataBoundItem is StudentDto student)
        {
            var confirm = MessageBox.Show($"Are you sure you want to delete {student.Name}?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    await ApiClient.Instance.DeleteAsync($"/api/students/{student.Id}");
                    await RefreshStudentGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Delete failed: {ex.Message}");
                }
            }
        }
    }
    private async void btnLinkParent_Click(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow?.DataBoundItem is StudentDto student)
        {
            MessageBox.Show($"Linking parents for {student.Name} is not in the current project scope.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}