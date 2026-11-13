using Sarana_Sekolah.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarana_Sekolah
{
    public static class Sesssion
    {
        public static User CurrentUser { get; set; }
        public static bool IsAdmin()
        {
            return CurrentUser != null &&  CurrentUser.IsAdmin();
        }
    }
}
