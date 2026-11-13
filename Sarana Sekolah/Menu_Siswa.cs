using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;

namespace Sarana_Sekolah
{
    public partial class Menu_Siswa : Form
    {
        private UserControl currentPage;
        private ucDashboardStats stats;

        public Menu_Siswa()
        {
            InitializeComponent();

            this.Load += Menu_Siswa_Load;
            btnHomepage.Click += (s, e) => ShowHome();
            btnAspirations.Click += (s, e) => ShowAspirations();
            btnKeluar.Click += (s, e) =>            
            {
                Sesssion.CurrentUser = null;
                this.Close();                        
            };
        }

        private void Menu_Siswa_Load(object sender, EventArgs e)
        {
            if (Sesssion.CurrentUser == null)
            {
                MessageBox.Show("Silakan login dulu.");
                this.Close();
                return;
            }

            string name = Sesssion.CurrentUser.Name;
            string initial = name.Length > 0 ? name.Substring(0, 1).ToUpper() : "?";
            label5.Text = name;
            label3.Text = initial;
            label4.Text = initial;

            stats = new ucDashboardStats();
            stats.Left = guna2Panel1.Left;
            stats.Top = Math.Max(guna2Panel1.Bottom, guna2Panel2.Bottom) + 25;
            stats.Width = guna2Panel2.Right - guna2Panel1.Left;
            pnlHome.Controls.Add(stats);

            ShowHome();
        }

        private void ClearPage()
        {
            if (currentPage != null)
            {
                pnlContent.Controls.Remove(currentPage);
                currentPage.Dispose();
                currentPage = null;
            }
        }

        private void ShowPage(UserControl page)
        {
            ClearPage();
            pnlHome.Visible = false;
            currentPage = page;
            page.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(page);
            page.BringToFront();
        }

        private void ShowHome()
        {
            ClearPage();
            pnlHome.Visible = true;
            if (stats != null) stats.Reload();
        }

        private void ShowAspirations()
        {
            var list = new ucAspirations();
            list.NewAspirationClicked += ShowAspirationForm;
            ShowPage(list);
        }

        private void ShowAspirationForm()
        {
            var form = new ucAspirationForm();
            form.Finished += ShowAspirations;
            ShowPage(form);
        }
    }
}