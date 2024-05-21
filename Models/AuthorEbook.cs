using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class AuthorEbook
    {
        public int AuthorId { get; set; }

        [JsonIgnore]
        public Author Author { get; set; }

        public int EbookId { get; set; }

        [JsonIgnore]
        public Ebook Ebook { get; set; }
    }
}
