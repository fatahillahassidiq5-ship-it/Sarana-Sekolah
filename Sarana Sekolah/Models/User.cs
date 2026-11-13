using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sarana_Sekolah.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Nis { get; set; }
        public string Kelas {  get; set; }
        public bool IsAdmin()
        {
            return Role == "admin";
        }
    }
}
