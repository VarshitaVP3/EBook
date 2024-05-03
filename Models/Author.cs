using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Country { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public bool? isActive { get; set; }

        public string? Email { get; set; }
        public string? ContactInfo { get; set; }
        public string? SocialMedia { get; set; }


        public Author()
        {
            
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            BirthDate = DateTime.Now;
        }
    }
}
