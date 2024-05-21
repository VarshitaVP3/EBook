using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Ebook
    {
        public int EbookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }

        public string Language {  get; set; }
        public string Publisher { get; set; }
        public int PageCount { get; set; }
        public int AverageCounting { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        
        public int GenereId { get; set; }

        public bool isAvailable { get; set; }

        public string edition {  get; set; }

        [JsonIgnore]
        public Genere Genere { get; set; }

        public ICollection<AuthorEbook> AuthorEbooks { get; set; } = new List<AuthorEbook>();


        public Ebook()
        {
            
            //DateOnly.FromDateTime(DateTime.Now);
            //CreatedAt = DateTime.Now;
            //UpdatedAt = DateTime.Now;
            isAvailable = true;
            PublicationDate = DateTime.Now;

        }
    }
}
