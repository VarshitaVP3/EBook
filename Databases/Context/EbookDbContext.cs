using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases.Interface
{
    public class EbookDbContext : DbContext
    {
        public EbookDbContext(DbContextOptions<EbookDbContext> options) : base(options)
        {
        }

        public DbSet<Author> AuthorsEf { get; set; }
        public DbSet<Ebook> EbooksEf { get; set; }
        public DbSet<Genere> GeneresEf { get; set; }
        public DbSet<AuthorEbook> AuthorEbooksEf { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.AuthorId);
                entity.Property(e => e.AuthorId).UseIdentityColumn();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Biography).HasMaxLength(100);
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.Country).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValue(DateTime.Now);
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).HasDefaultValue(DateTime.Now);
                entity.Property(e => e.isActive).IsRequired();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(30);
                entity.Property(e => e.ContactInfo).IsRequired().HasMaxLength(30);
                entity.Property(e => e.SocialMedia).HasMaxLength(30);
            });


            modelBuilder.Entity<Genere>(entity =>
            {
                entity.HasKey(e => e.GenereId);
                entity.Property(e => e.GenereId).UseIdentityColumn();
                entity.Property(e => e.GenereName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.GenereDescription).HasMaxLength(40);
            });


            modelBuilder.Entity<Ebook>(entity =>
            {
                entity.HasKey(e => e.EbookId);
                entity.Property(e => e.EbookId).UseIdentityColumn();
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(60);
                entity.Property(e => e.ISBN);
                entity.Property(e => e.PublicationDate);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Language).HasMaxLength(20);
                entity.Property(e => e.Publisher).HasMaxLength(20);
                entity.Property(e => e.PageCount);
                entity.Property(e => e.AverageCounting);
                entity.Property(e => e.CreatedAt).HasDefaultValue(DateTime.Now);
                entity.Property(e => e.UpdatedAt).HasDefaultValue(DateTime.Now);

                entity.Property(e => e.isAvailable);
                entity.Property(e => e.edition).HasMaxLength(20);

                entity.HasOne(d => d.Genere)
                    .WithMany(p => p.Ebooks)
                    .HasForeignKey(d => d.GenereId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Configure the AuthorEbook join entity
            modelBuilder.Entity<AuthorEbook>(entity =>
            {

                entity.HasKey(e => new { e.AuthorId, e.EbookId });

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.AuthorEbooks)
                    .HasForeignKey(d => d.AuthorId);

                entity.HasOne(d => d.Ebook)
                    .WithMany(p => p.AuthorEbooks)
                    .HasForeignKey(d => d.EbookId);
            });
        }
    }
}
