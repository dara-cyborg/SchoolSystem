using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SchoolSystem.Desktop.Forms.Teacher {
    public partial class ucScoreSubmit : UserControl
    {
        public ucScoreSubmit()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void SetupGrid()
        {
            if (dgvScores.Columns.Count == 0)
            {
                dgvScores.Columns.Add("colStudent", "Student");
                dgvScores.Columns.Add("colScore", "Score");

                // IMPORTANT: lock column
                dgvScores.Columns.Add("IsLocked", "Locked");
                dgvScores.Columns["IsLocked"].Visible = false;
            }
        }

        private void ucScoreSubmit_Load(object sender, EventArgs e)
        {
            cboClassSubject.Items.Add("Math - Class A");
            cboClassSubject.Items.Add("Science - Class B");

            nudMonth.Minimum = 1;
            nudMonth.Maximum = 12;

            nudYear.Minimum = 2000;
            nudYear.Maximum = 2100;
        }

        private void btnSubmitScores_Click(object sender, EventArgs e)
        {
            if (dgvScores.Rows.Count == 0)
            {
                MessageBox.Show("No scores to submit.");
                return;
            }

            pgbScoreProgress.Value = 0;
            pgbScoreProgress.Maximum = dgvScores.Rows.Count;
            pgbScoreProgress.Visible = true;

            foreach (DataGridViewRow row in dgvScores.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["IsLocked"].Value = true;
                }

                pgbScoreProgress.Value++;
            }

            MessageBox.Show("Submit Success!");
            pgbScoreProgress.Visible = false;

            dgvScores.Refresh();
        }

        private void dgvScores_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvScores.Rows[e.RowIndex];

            if (dgvScores.Columns.Contains("IsLocked") &&
                row.Cells["IsLocked"].Value != null &&
                (bool)row.Cells["IsLocked"].Value == true)
            {
                e.CellStyle.BackColor = ColorTranslator.FromHtml("#D3D3D3");
                e.CellStyle.ForeColor = Color.DarkGray;
                row.ReadOnly = true;
            }
        }

        private void cboClassSubject_SelectedIndexChanged(object sender, EventArgs e) { }

        private void nudMonth_ValueChanged(object sender, EventArgs e) { }

        private void nudYear_ValueChanged(object sender, EventArgs e) { }

        private void dgvScores_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void pgbScoreProgress_Click(object sender, EventArgs e) { }
    }
}

