using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Genere
    {
        public int GenereId { get; set; }
        public string GenereName { get; set;}
        public string GenereDescription { get; set;}
        public ICollection<Ebook> Ebooks { get; set; }
    }
}
