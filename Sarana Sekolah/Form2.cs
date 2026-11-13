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

namespace Sarana_Sekolah
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btnTestKoneksi_Click_Click(object sender, EventArgs e)
        {
            try
            {
                Koneksi db = new Koneksi();
                using (MySqlConnection conn = db.BukaKoneksi())
                {
                    MessageBox.Show("Database IS CONNECTED YAY!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connectin FUCKING FAILED!: " + ex.Message);
            }
        }
    }
}
