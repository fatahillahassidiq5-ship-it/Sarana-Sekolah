using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sarana_Sekolah
{
    public partial class Menu_Admin : Form
    {
        private void btnHomepage_Click(object sender, EventArgs e) { ShowHome(); }
        private void btnUsers_Click(object sender, EventArgs e) { ShowUsers(); }

        private UserControl currentPage;
        private ucDashboardStats stats;

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
        }

        private void ShowUsers()
        {
            var list = new ucUsers();
            list.NewUserClicked += () => ShowUserForm(null);
            list.EditUserClicked += id => ShowUserForm(id);
            ShowPage(list);
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

        private void ShowUserForm(int? id)
        {
            var form = new ucUserForm(id);
            form.Finished += ShowUsers;      // after save/cancel, go back to the list
            ShowPage(form);
        }
        public Menu_Admin()
        {
            InitializeComponent();

            btnHomepage.Click += btnHomepage_Click;
            btnUsers.Click += btnUsers_Click;
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void txtProcessing_Click(object sender, EventArgs e)
        {

        }
        private void Menu_Admin_Load(object sender, EventArgs e)
        {
            if (!Sesssion.IsAdmin())
            {
                MessageBox.Show("Hanya admin yang boleh membuka halaman ini.");
                this.Close();
                return;
            }

            string name = Sesssion.CurrentUser.Name;
            string initial = name.Length > 0 ? name.Substring(0, 1).ToUpper() : "?";
            label5.Text = name;
            label3.Text = initial;
            label4.Text = initial;

            // dashboard stats (Menunggu / Proses / Selesai)
            stats = new ucDashboardStats();
            stats.Left = guna2Panel1.Left;                                       // line up with the cards
            stats.Top = Math.Max(guna2Panel1.Bottom, guna2Panel2.Bottom) + 25;   // below the two cards
            stats.Width = guna2Panel2.Right - guna2Panel1.Left;                  // as wide as both cards
            pnlHome.Controls.Add(stats);

            ShowHome();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void btnAspirations_Click(object sender, EventArgs e)
        {
            ShowAspirations();
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            ShowAspirations();
            Sesssion.CurrentUser = null;
            this.Close();
        }
    }
}
