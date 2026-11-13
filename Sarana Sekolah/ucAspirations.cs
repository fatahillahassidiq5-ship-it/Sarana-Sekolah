using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace Sarana_Sekolah
{
    public partial class ucAspirations : UserControl
    {
        public event Action NewAspirationClicked;

        private TableLayoutPanel layout;
        private Guna2TextBox txtSearch;
        private Guna2Button btnFilter;
        private FlowLayoutPanel pnlFilter;
        private Guna2ComboBox cmbKategori, cmbBulan;
        private DateTimePicker dtpTanggal;
        private Guna2TextBox txtNis;
        private DataGridView grid;

        private int fCat = 0;
        private bool fUseDate = false;
        private DateTime fDate = DateTime.Today;
        private int fMonth = 0;
        private string fNis = "";

        public ucAspirations()
        {
            InitializeComponent();
            BuildUi();
            LoadCategories();
            LoadData();
        }

        private void BuildUi()
        {
            this.BackColor = Color.FromArgb(250, 250, 250);
            this.Padding = new Padding(30, 20, 30, 20);

            var pnlTop = new Panel { Dock = DockStyle.Fill };
            pnlTop.Controls.Add(new Label
            {
                Text = "Aspirations  >  List",
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(0, 0)
            });
            pnlTop.Controls.Add(new Label
            {
                Text = "Aspirations",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 25)
            });
            var btnNew = new Guna2Button
            {
                Text = "New aspiration",
                Size = new Size(160, 42),
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 187, 0),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnNew.Location = new Point(pnlTop.Width - btnNew.Width, 30);
            btnNew.Click += (s, e) => NewAspirationClicked?.Invoke();
            pnlTop.Controls.Add(btnNew);

            var pnlTool = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 10, 0, 0)
            };
            btnFilter = new Guna2Button
            {
                Text = "Filters",
                Size = new Size(120, 40),
                BorderRadius = 8,
                FillColor = Color.White,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10)
            };
            btnFilter.Click += (s, e) => ToggleFilter();

            txtSearch = new Guna2TextBox
            {
                PlaceholderText = "Search",
                Size = new Size(280, 40),
                BorderRadius = 8
            };
            txtSearch.TextChanged += (s, e) => LoadData();

            pnlTool.Controls.Add(btnFilter);   
            pnlTool.Controls.Add(txtSearch);

            pnlFilter = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Visible = false,
                Padding = new Padding(10, 5, 10, 0)
            };

            cmbKategori = new Guna2ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10),
                Height = 36
            };

            dtpTanggal = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,      
                Checked = false,
                Font = new Font("Segoe UI", 10)
            };

            cmbBulan = new Guna2ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10),
                Height = 36
            };
            cmbBulan.Items.AddRange(new object[]
            {
                "All", "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                "Juli", "Agustus", "September", "Oktober", "November", "Desember"
            });
            cmbBulan.SelectedIndex = 0;     

            txtNis = new Guna2TextBox
            {
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10),
                Height = 36
            };

            pnlFilter.Controls.Add(FilterGroup("Kategori", cmbKategori));
            pnlFilter.Controls.Add(FilterGroup("Tanggal", dtpTanggal));
            pnlFilter.Controls.Add(FilterGroup("Bulan", cmbBulan));
            pnlFilter.Controls.Add(FilterGroup("NIS", txtNis));

            var btnApply = new Guna2Button
            {
                Text = "Apply filters",
                Size = new Size(120, 40),
                Location = new Point(0, 26),
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 187, 0),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnApply.Click += (s, e) => ApplyFilters();

            var btnReset = new Guna2Button
            {
                Text = "Reset",
                Size = new Size(90, 40),
                Location = new Point(130, 26),
                BorderRadius = 8,
                FillColor = Color.White,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 10)
            };
            btnReset.Click += (s, e) => ResetFilters();

            var pnlButtons = new Panel { Size = new Size(240, 80) };
            pnlButtons.Controls.Add(btnApply);
            pnlButtons.Controls.Add(btnReset);
            pnlFilter.Controls.Add(pnlButtons);

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
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "User", DataPropertyName = "user_name" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "NIS", DataPropertyName = "nis" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Category", DataPropertyName = "category" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Location", DataPropertyName = "location" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "status", HeaderText = "Status", DataPropertyName = "status" });
            grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Feedback", DataPropertyName = "feedback" });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "created_at",
                HeaderText = "Created at",
                DataPropertyName = "created_at",
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = { Format = "MMM dd, yyyy HH:mm:ss" }
            });

            if (Sesssion.IsAdmin())
            {
                grid.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "tanggapi",
                    HeaderText = "",
                    Text = "Tanggapi",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Flat
                });
            }

            grid.CellFormatting += Grid_CellFormatting;
            grid.CellContentClick += Grid_CellContentClick;

            layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));    
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));    
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 0));     
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));    
            layout.Controls.Add(pnlTop, 0, 0);
            layout.Controls.Add(pnlTool, 0, 1);
            layout.Controls.Add(pnlFilter, 0, 2);
            layout.Controls.Add(grid, 0, 3);
            this.Controls.Add(layout);
        }

        private Panel FilterGroup(string label, Control input)
        {
            var p = new Panel { Size = new Size(210, 80) };
            p.Controls.Add(new Label
            {
                Text = label,
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(0, 4)
            });
            input.Location = new Point(0, 28);
            input.Width = 195;
            p.Controls.Add(input);
            return p;
        }

        private void ToggleFilter()
        {
            bool show = !pnlFilter.Visible;
            pnlFilter.Visible = show;
            layout.RowStyles[2].Height = show ? 100 : 0;
        }

        private void ApplyFilters()
        {
            fCat = cmbKategori.SelectedIndex > 0 ? Convert.ToInt32(cmbKategori.SelectedValue) : 0;
            fUseDate = dtpTanggal.Checked;
            fDate = dtpTanggal.Value.Date;
            fMonth = cmbBulan.SelectedIndex > 0 ? cmbBulan.SelectedIndex : 0;
            fNis = txtNis.Text.Trim();

            int n = (fCat > 0 ? 1 : 0) + (fUseDate ? 1 : 0)
                  + (fMonth > 0 ? 1 : 0) + (fNis != "" ? 1 : 0);
            btnFilter.Text = n > 0 ? "Filters (" + n + ")" : "Filters";

            LoadData();
        }

        private void ResetFilters()
        {
            cmbKategori.SelectedIndex = 0;
            dtpTanggal.Checked = false;
            cmbBulan.SelectedIndex = 0;
            txtNis.Clear();
            ApplyFilters();
        }

        private void LoadCategories()
        {
            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    var dt = new DataTable();
                    new MySqlDataAdapter("SELECT id, name FROM categories ORDER BY name", conn).Fill(dt);

                    var all = dt.NewRow();
                    all["id"] = 0;
                    all["name"] = "All";
                    dt.Rows.InsertAt(all, 0);

                    cmbKategori.DisplayMember = "name";
                    cmbKategori.ValueMember = "id";
                    cmbKategori.DataSource = dt;
                    cmbKategori.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat kategori: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                bool admin = Sesssion.IsAdmin();

                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    string sql = @"SELECT a.id, u.name AS user_name, u.nis, c.name AS category,
                                          a.location, a.status,
                                          CASE WHEN CHAR_LENGTH(a.feedback) > 30
                                               THEN CONCAT(LEFT(a.feedback, 30), '...')
                                               ELSE a.feedback END AS feedback,
                                          a.created_at
                                   FROM aspirations a
                                   JOIN users u ON u.id = a.user_id
                                   JOIN categories c ON c.id = a.category_id
                                   WHERE (@admin = 1 OR a.user_id = @uid)
                                     AND (u.name LIKE @k OR c.name LIKE @k OR a.location LIKE @k)
                                     AND (@cat = 0 OR a.category_id = @cat)
                                     AND (@useDate = 0 OR DATE(a.created_at) = @tgl)
                                     AND (@bulan = 0 OR MONTH(a.created_at) = @bulan)
                                     AND (@nis = '' OR CAST(u.nis AS CHAR) = @nis)
                                   ORDER BY a.created_at DESC";

                    using (var da = new MySqlDataAdapter(sql, conn))
                    {
                        var p = da.SelectCommand.Parameters;
                        p.AddWithValue("@admin", admin ? 1 : 0);
                        p.AddWithValue("@uid", Sesssion.CurrentUser.Id);
                        p.AddWithValue("@k", "%" + txtSearch.Text.Trim() + "%");
                        p.AddWithValue("@cat", fCat);
                        p.AddWithValue("@useDate", fUseDate ? 1 : 0);
                        p.AddWithValue("@tgl", fDate);
                        p.AddWithValue("@bulan", fMonth);
                        p.AddWithValue("@nis", fNis);

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

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            if (grid.Columns[e.ColumnIndex].Name != "status") return;

            string s = e.Value.ToString();
            if (s == "Menunggu")
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 243, 205);
                e.CellStyle.ForeColor = Color.FromArgb(170, 100, 0);
            }
            else if (s == "Proses")
            {
                e.CellStyle.BackColor = Color.FromArgb(219, 234, 254);
                e.CellStyle.ForeColor = Color.FromArgb(30, 90, 200);
            }
            else
            {
                e.CellStyle.BackColor = Color.FromArgb(220, 252, 231);
                e.CellStyle.ForeColor = Color.FromArgb(22, 128, 60);
            }
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (grid.Columns[e.ColumnIndex].Name != "tanggapi") return;
            if (!Sesssion.IsAdmin()) return;

            int id = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["id"].Value);

            string status = "Menunggu";
            string feedback = "";
            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(
                        "SELECT status, feedback FROM aspirations WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            status = rd["status"].ToString();
                            feedback = rd["feedback"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message);
                return;
            }

            using (var dlg = new FormTanggapi(status, feedback))
            {
                if (dlg.ShowDialog(this.FindForm()) != DialogResult.OK) return;

                try
                {
                    using (var conn = new MySqlConnection(Db.koneksijaringan))
                    {
                        conn.Open();
                        var cmd = new MySqlCommand(
                            @"UPDATE aspirations
                              SET status = @s, feedback = @f, updated_at = NOW()
                              WHERE id = @id", conn);
                        cmd.Parameters.AddWithValue("@s", dlg.StatusValue);
                        cmd.Parameters.AddWithValue("@f",
                            dlg.FeedbackValue == "" ? (object)DBNull.Value : dlg.FeedbackValue);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menyimpan: " + ex.Message);
                    return;
                }
            }

            MessageBox.Show("Tanggapan disimpan", "Sukses",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }
    }
}