using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in yyyy-MM-dd format")]
        public DateTime BirthDate { get; set; }
        public string? Country { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public bool? isActive { get; set; }

        public string? Email { get; set; }
        public string? ContactInfo { get; set; }
        public string? SocialMedia { get; set; }

        public ICollection<AuthorEbook> AuthorEbooks { get; set; } = new List<AuthorEbook>();


        public Author()
        {
            
            //CreatedAt = DateTime.Now;
            //UpdatedAt = DateTime.Now;
            //BirthDate = DateTime.Now;
        }
    }
}
