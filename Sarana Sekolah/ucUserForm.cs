using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace Sarana_Sekolah
{
    public partial class ucUserForm : UserControl
    {
        public event Action Finished;

        private readonly int? _id;          
        private Guna2TextBox txtName, txtEmail, txtNis, txtKelas, txtPassword;
        private Guna2ComboBox cmbRole;
        private TableLayoutPanel table;

        public ucUserForm(int? id)
        {
            InitializeComponent();
            _id = id;
            BuildUi();
            if (_id.HasValue) LoadUser(_id.Value);
        }

        private void BuildUi()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Padding = new Padding(30, 20, 30, 20);
            this.AutoScroll = true;

            bool isEdit = _id.HasValue;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 90 };
            pnlTop.Controls.Add(new Label
            {
                Text = isEdit ? "Users  >  Edit" : "Users  >  Create",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(0, 0)
            });
            pnlTop.Controls.Add(new Label
            {
                Text = isEdit ? "Edit User" : "Create User",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 25)
            });

            
            table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 6,
                Height = 3 * (28 + 50) + 20
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 3; i++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            }

            txtName = NewText("");
            txtEmail = NewText("");
            txtNis = NewText("");
            txtKelas = NewText("");
            txtPassword = NewText("");
            txtPassword.UseSystemPasswordChar = true;
            if (isEdit) txtPassword.PlaceholderText = "Kosongkan jika tidak ingin mengganti";

            cmbRole = new Guna2ComboBox
            {
                BorderRadius = 8,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbRole.Items.Add("admin");
            cmbRole.Items.Add("siswa");
            cmbRole.SelectedItem = "siswa";          // default, like the Filament form

            AddField("Name *", txtName, 0, 0);
            AddField("Email address *", txtEmail, 1, 0);
            AddField("Role *", cmbRole, 0, 1);
            AddField("Nis", txtNis, 1, 1);
            AddField("Kelas", txtKelas, 0, 2);
            AddField(isEdit ? "Password" : "Password *", txtPassword, 1, 2);

            // ---- buttons ----
            var pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 70,
                Padding = new Padding(0, 15, 0, 0)
            };

            var btnCreate = new Guna2Button
            {
                Text = isEdit ? "Save" : "Create",
                Size = new Size(110, 42),
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 187, 0),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCreate.Click += (s, e) => { if (Save()) Finished?.Invoke(); };
            pnlButtons.Controls.Add(btnCreate);

            if (!isEdit)
            {
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
                pnlButtons.Controls.Add(btnAnother);
            }

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
            pnlButtons.Controls.Add(btnCancel);

            this.Controls.Add(pnlButtons);
            this.Controls.Add(table);
            this.Controls.Add(pnlTop);
        }

        private Guna2TextBox NewText(string placeholder)
        {
            return new Guna2TextBox
            {
                PlaceholderText = placeholder,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10)
            };
        }

        private void AddField(string label, Control input, int col, int pairRow)
        {
            var lbl = new Label
            {
                Text = label,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Margin = new Padding(0, 6, 0, 0)
            };
            input.Dock = DockStyle.Fill;
            input.Margin = new Padding(0, 0, 20, 0);

            table.Controls.Add(lbl, col, pairRow * 2);
            table.Controls.Add(input, col, pairRow * 2 + 1);
        }

        private void ClearFields()
        {
            txtName.Clear(); txtEmail.Clear(); txtNis.Clear();
            txtKelas.Clear(); txtPassword.Clear();
            cmbRole.SelectedItem = "siswa";
            txtName.Focus();
        }

        private void LoadUser(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(
                        "SELECT name, email, role, nis, kelas FROM users WHERE id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            txtName.Text = rd["name"].ToString();
                            txtEmail.Text = rd["email"].ToString();
                            cmbRole.SelectedItem = rd["role"].ToString();
                            txtNis.Text = rd["nis"].ToString();
                            txtKelas.Text = rd["kelas"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat user: " + ex.Message);
            }
        }

        private static object NullIfEmpty(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();
        }

        private bool Save()
        {
            string password = txtPassword.Text;

            if (txtName.Text.Trim() == "" || txtEmail.Text.Trim() == "")
            {
                MessageBox.Show("Name dan Email wajib diisi.");
                return false;
            }
            // password required only when creating
            if (!_id.HasValue && password == "")
            {
                MessageBox.Show("Password wajib diisi untuk akun baru.");
                return false;
            }

            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    MySqlCommand cmd;

                    if (!_id.HasValue)
                    {
                        cmd = new MySqlCommand(@"INSERT INTO users (name, email, password, role, nis, kelas)
                                                 VALUES (@name, @email, @pass, @role, @nis, @kelas)", conn);
                        cmd.Parameters.AddWithValue("@pass", BCrypt.Net.BCrypt.HashPassword(password));
                    }
                    else if (password != "")
                    {
                        cmd = new MySqlCommand(@"UPDATE users SET name=@name, email=@email, password=@pass,
                                                 role=@role, nis=@nis, kelas=@kelas WHERE id=@id", conn);
                        cmd.Parameters.AddWithValue("@pass", BCrypt.Net.BCrypt.HashPassword(password));
                        cmd.Parameters.AddWithValue("@id", _id.Value);
                    }
                    else
                    {
                        cmd = new MySqlCommand(@"UPDATE users SET name=@name, email=@email,
                                                 role=@role, nis=@nis, kelas=@kelas WHERE id=@id", conn);
                        cmd.Parameters.AddWithValue("@id", _id.Value);
                    }

                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@nis", NullIfEmpty(txtNis.Text));
                    cmd.Parameters.AddWithValue("@kelas", NullIfEmpty(txtKelas.Text));
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                MessageBox.Show("Email sudah dipakai akun lain.");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan: " + ex.Message);
                return false;
            }
        }
    }
}