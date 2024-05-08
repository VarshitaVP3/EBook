using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EbookDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ISBN { get; set; }

        public decimal Price { get; set; }

        public string Language { get; set; }
        public string Publisher { get; set; }
        public int PageCount { get; set; }
        public int AverageCounting { get; set; }
      
        public DateTime UpdatedAt { get; set; }       

        public bool isAvailable { get; set; }

        public string edition { get; set; }
        
        public int GenereId { get; set; }



        public EbookDto()
        {
            //DateOnly.FromDateTime(DateTime.Now);
            //UpdatedAt = DateTime.Now;
            isAvailable = true;

        }
    }
}
