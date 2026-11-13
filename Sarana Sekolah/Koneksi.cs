using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace Sarana_Sekolah
{   
    public class Koneksi
    {
        private string koneksijaringan;
        private MySqlConnection koneksi;
        public Koneksi() 
        {
            koneksijaringan = "Server=localhost;Database=sarana_ukk;Uid=root;Pwd=";
        }

        public MySqlConnection BukaKoneksi()
        {
            try
            {
                koneksi = new MySqlConnection(koneksijaringan);
                koneksi.Open();
                return koneksi;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ASTAGA GAGAL KI MAU MELEDAK PC MU 😭😭😭🗣️🔥🔥🔥" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void TutupKoneksi()
        {
            if( koneksijaringan != null && koneksi.State == System.Data.ConnectionState.Open)
            {
                koneksi.Close();
            }
        }
    }
}
