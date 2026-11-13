using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace Sarana_Sekolah
{
    public partial class ucAspirationForm : UserControl
    {
        public event Action Finished;

        private Guna2ComboBox cmbCategory;
        private Guna2TextBox txtLocation, txtDesc;

        public ucAspirationForm()
        {
            InitializeComponent();
            BuildUi();
            LoadCategories();
        }

        private Label NewLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 6, 0, 0)
            };
        }

        private void BuildUi()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Padding = new Padding(30, 20, 30, 20);

            // ---- title ----
            var pnlTop = new Panel { Dock = DockStyle.Fill };
            pnlTop.Controls.Add(new Label
            {
                Text = "Aspirations  >  Create",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(0, 0)
            });
            pnlTop.Controls.Add(new Label
            {
                Text = "Create Aspiration",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 25)
            });

            // ---- fields ----
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));

            cmbCategory = new Guna2ComboBox
            {
                BorderRadius = 8,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 20, 0)
            };
            txtLocation = new Guna2TextBox
            {
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10),
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0)
            };
            txtDesc = new Guna2TextBox
            {
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 0, 0)
            };

            table.Controls.Add(NewLabel("Category *"), 0, 0);
            table.Controls.Add(NewLabel("Location *"), 1, 0);
            table.Controls.Add(cmbCategory, 0, 1);
            table.Controls.Add(txtLocation, 1, 1);

            var lblDesc = NewLabel("Description *");
            table.Controls.Add(lblDesc, 0, 2);
            table.SetColumnSpan(lblDesc, 2);
            table.Controls.Add(txtDesc, 0, 3);
            table.SetColumnSpan(txtDesc, 2);

            // ---- buttons ----
            var pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 15, 0, 0)
            };

            var btnCreate = new Guna2Button
            {
                Text = "Create",
                Size = new Size(110, 42),
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 187, 0),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCreate.Click += (s, e) => { if (Save()) Finished?.Invoke(); };

            var btnAnother = new Guna2Button
            {
                Text = "Create & create another",
                Size = new Size(200, 42),
                BorderRadius = 8,
                FillColor = Color.White,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10)
            };
            btnAnother.Click += (s, e) => { if (Save()) ClearFields(); };

            var btnCancel = new Guna2Button
            {
                Text = "Cancel",
                Size = new Size(100, 42),
                BorderRadius = 8,
                FillColor = Color.White,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.Click += (s, e) => Finished?.Invoke();

            pnlButtons.Controls.Add(btnCreate);
            pnlButtons.Controls.Add(btnAnother);
            pnlButtons.Controls.Add(btnCancel);

            // ---- put it together ----
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 250));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.Controls.Add(pnlTop, 0, 0);
            root.Controls.Add(table, 0, 1);
            root.Controls.Add(pnlButtons, 0, 2);
            this.Controls.Add(root);
        }

        private void LoadCategories()
        {
            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    var da = new MySqlDataAdapter("SELECT id, name FROM categories ORDER BY name", conn);
                    var dt = new DataTable();
                    da.Fill(dt);

                    cmbCategory.DisplayMember = "name";
                    cmbCategory.ValueMember = "id";
                    cmbCategory.DataSource = dt;
                    cmbCategory.SelectedIndex = -1;   // nothing chosen yet
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat kategori: " + ex.Message);
            }
        }

        private void ClearFields()
        {
            cmbCategory.SelectedIndex = -1;
            txtLocation.Clear();
            txtDesc.Clear();
            txtLocation.Focus();
        }

        private bool Save()
        {
            if (cmbCategory.SelectedValue == null ||
                txtLocation.Text.Trim() == "" ||
                txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Category, Location, dan Description wajib diisi.");
                return false;
            }

            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    // like CreateAspiration.php: user_id = logged-in user, status = Menunggu
                    var cmd = new MySqlCommand(
                        @"INSERT INTO aspirations
                            (user_id, category_id, location, description, status, created_at, updated_at)
                          VALUES (@uid, @cat, @loc, @desc, 'Menunggu', NOW(), NOW())", conn);
                    cmd.Parameters.AddWithValue("@uid", Sesssion.CurrentUser.Id);
                    cmd.Parameters.AddWithValue("@cat", Convert.ToInt32(cmbCategory.SelectedValue));
                    cmd.Parameters.AddWithValue("@loc", txtLocation.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", txtDesc.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Created");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan: " + ex.Message);
                return false;
            }
        }
    }
}