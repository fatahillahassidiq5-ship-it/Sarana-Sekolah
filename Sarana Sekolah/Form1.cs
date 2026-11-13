using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using Sarana_Sekolah.Models;

namespace Sarana_Sekolah
{
    public partial class Login : Form
    {
        Koneksi konn = new Koneksi();
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            CenterLoginPanel();
        }

        private void CenterLoginPanel()
        {
            guna2Panel1.Left =
                (this.ClientSize.Width - guna2Panel1.Width) / 2;

            guna2Panel1.Top =
                (this.ClientSize.Height - guna2Panel1.Height) / 2;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CenterLoginPanel();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_Address_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Username dan juga Password Harus di isi!");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(Db.koneksijaringan))
                {
                    conn.Open();
                    string sql = @"SELECT id, name, email, password, role, nis, kelas
                           FROM users WHERE email = @u OR name = @u LIMIT 1";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (!rd.Read())
                            {
                                MessageBox.Show("Akun tidak ditemukan");
                                return;
                            }

                            string stored = rd["password"].ToString();

                            // hashed password (from the Create User form) or old plain text
                            bool ok = stored.StartsWith("$2")
                                ? BCrypt.Net.BCrypt.Verify(password, stored)
                                : password == stored;

                            if (!ok)
                            {
                                MessageBox.Show("Password Salah");
                                return;
                            }

                            Sesssion.CurrentUser = new User
                            {
                                Id = Convert.ToInt32(rd["id"]),
                                Name = rd["name"].ToString(),
                                Email = rd["email"].ToString(),
                                Role = rd["role"].ToString(),
                                Nis = rd["nis"].ToString(),
                                Kelas = rd["kelas"].ToString()
                            };
                        }
                    }
                }
                Form menu;
                if (Sesssion.IsAdmin())
                    menu = new Menu_Admin();
                else
                    menu = new Menu_Siswa();

                menu.FormClosed += (s, args) => { txtPassword.Clear(); this.Show(); };
                menu.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }
        private void btnGenerateHash_Click_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text;

            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            MessageBox.Show(hash, "Generated Password Hash");

        }

        private void btnGenerateHash_Click(object sender, EventArgs e)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("Admin123");

            MessageBox.Show(hash);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
