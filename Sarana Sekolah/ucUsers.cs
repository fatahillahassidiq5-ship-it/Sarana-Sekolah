using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace Sarana_Sekolah
{
    public partial class ucUsers : UserControl
    {
        public event Action NewUserClicked;
        public event Action<int> EditUserClicked;

        private Guna2TextBox txtSearch;
        private DataGridView grid;

        public ucUsers()
        {
            InitializeComponent();
            BuildUi();
            LoadData("");
        }

        private void BuildUi()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Padding = new Padding(30, 20, 30, 20);

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 90 };

            var lblCrumb = new Label
            {
                Text = "Users  >  List",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            var lblTitle = new Label
            {
                Text = "Users",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 25)
            };
            var btnNew = new Guna2Button
            {
                Text = "New User",
                Size = new Size(130, 42),
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 187, 0),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnNew.Location = new Point(pnlTop.Width - btnNew.Width, 30);
            btnNew.Click += (s, e) => NewUserClicked?.Invoke();

            pnlTop.Controls.Add(lblCrumb);
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(btnNew);

            var pnlSearch = new Panel { Dock = DockStyle.Top, Height = 60 };
            txtSearch = new Guna2TextBox
            {
                PlaceholderText = "Search",
                Size = new Size(300, 40),
                BorderRadius = 8,
                Dock = DockStyle.Right
            };
            txtSearch.TextChanged += (s, e) => LoadData(txtSearch.Text.Trim());
            pnlSearch.Controls.Add(txtSearch);

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 42 }
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            grid.ColumnHeadersHeight = 42;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 243, 205);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;

            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "id", DataPropertyName = "id", Visible = false });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "name" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Email address", DataPropertyName = "email" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Role", DataPropertyName = "role" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Nis", DataPropertyName = "nis" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Kelas", DataPropertyName = "kelas" });
            grid.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "edit",
                HeaderText = "",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            });
            grid.CellContentClick += Grid_CellContentClick;

            this.Controls.Add(grid);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlTop);
            grid.BringToFront();
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (grid.Columns[e.ColumnIndex].Name != "edit") return;

            int id = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["id"].Value);
            EditUserClicked?.Invoke(id);
        }

        private void LoadData(string keyword)
        {
            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    string sql = @"SELECT id, name, email, role, nis, kelas FROM users
                                   WHERE name LIKE @k OR email LIKE @k
                                      OR nis LIKE @k OR kelas LIKE @k
                                   ORDER BY id";
                    using (var da = new MySqlDataAdapter(sql, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@k", "%" + keyword + "%");
                        var dt = new DataTable();
                        da.Fill(dt);
                        grid.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message);
            }
        }
    }
}
