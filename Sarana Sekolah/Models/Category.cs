using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarana_Sekolah.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<Aspiration> Aspirations { get; set; }
        = new List<Aspiration>();

    }
}
