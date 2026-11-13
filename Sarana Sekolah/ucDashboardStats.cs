using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;

namespace Sarana_Sekolah
{
    public class ucDashboardStats : UserControl
    {
        private Label lblMenunggu, lblProses, lblSelesai;

        public ucDashboardStats()
        {
            this.Height = 130;
            this.BackColor = Color.White;

            // three equal columns, one card each
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.White
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            lblMenunggu = AddCard(table, "Menunggu", 0);
            lblProses = AddCard(table, "Proses", 1);
            lblSelesai = AddCard(table, "Selesai", 2);

            this.Controls.Add(table);
            Reload();
        }

        // builds one card (title + big number) and returns the number label
        private Label AddCard(TableLayoutPanel table, string title, int column)
        {
            var card = new Guna2Panel
            {
                Dock = DockStyle.Fill,
                BorderRadius = 10,
                FillColor = Color.FromArgb(221, 221, 221),   // same gray as your other cards
                Margin = new Padding(0, 0, 15, 0)
            };

            var lblTitle = new Label
            {
                Text = title,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(20, 18)
            };

            var lblValue = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(18, 48)
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            table.Controls.Add(card, column, 0);
            return lblValue;
        }

        // like Aspiration::where('status', ...)->count()
        public void Reload()
        {
            int menunggu = 0, proses = 0, selesai = 0;

            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    var cmd = new MySqlCommand(
                        "SELECT status, COUNT(*) AS total FROM aspirations GROUP BY status", conn);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string status = rd["status"].ToString();
                            int total = Convert.ToInt32(rd["total"]);

                            if (status == "Menunggu") menunggu = total;
                            else if (status == "Proses") proses = total;
                            else if (status == "Selesai") selesai = total;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat statistik: " + ex.Message);
            }

            lblMenunggu.Text = menunggu.ToString();
            lblProses.Text = proses.ToString();
            lblSelesai.Text = selesai.ToString();
        }
    }
}