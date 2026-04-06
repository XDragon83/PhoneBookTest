using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Domain.Models;

namespace PhoneBook.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactPicture> ContactPictures { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
