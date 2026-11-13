using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Sarana_Sekolah
{
    public class FormTanggapi : Form
    {
        private Guna2ComboBox cmbStatus;
        private Guna2TextBox txtFeedback;

        public string StatusValue
        {
            get { return cmbStatus.SelectedItem.ToString(); }
        }

        public string FeedbackValue
        {
            get { return txtFeedback.Text.Trim(); }
        }

        public FormTanggapi(string status, string feedback)
        {
            this.Text = "Tanggapi";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(560, 340);
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            var lblTitle = new Label
            {
                Text = "Tanggapi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            var lblStatus = new Label
            {
                Text = "Status *",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 65)
            };
            cmbStatus = new Guna2ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BorderRadius = 8,
                Location = new Point(24, 92),
                Size = new Size(512, 40)
            };
            cmbStatus.Items.AddRange(new object[] { "Menunggu", "Proses", "Selesai" });
            cmbStatus.SelectedIndex = Math.Max(0, cmbStatus.Items.IndexOf(status ?? ""));

            var lblFeedback = new Label
            {
                Text = "Umpan balik",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 150)
            };
            txtFeedback = new Guna2TextBox
            {
                Multiline = true,
                BorderRadius = 8,
                Location = new Point(24, 177),
                Size = new Size(512, 90),
                Text = feedback ?? ""
            };

            var btnSubmit = new Guna2Button
            {
                Text = "Submit",
                Size = new Size(100, 40),
                Location = new Point(24, 285),
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 187, 0),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSubmit.Click += (s, e) => { this.DialogResult = DialogResult.OK; };

            var btnCancel = new Guna2Button
            {
                Text = "Cancel",
                Size = new Size(100, 40),
                Location = new Point(134, 285),
                BorderRadius = 8,
                FillColor = Color.White,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(lblFeedback);
            this.Controls.Add(txtFeedback);
            this.Controls.Add(btnSubmit);
            this.Controls.Add(btnCancel);
        }
    }
}
