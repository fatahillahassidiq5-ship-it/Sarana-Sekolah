using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarana_Sekolah.Models
{
    public class Aspiration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; } = "";
        public string Status { get; set; } = "Menunggu";
        public string Feedback { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; }
        public Category category { get; set; }


    }
}
