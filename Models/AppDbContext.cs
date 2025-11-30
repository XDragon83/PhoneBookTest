using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Models;

namespace Phonebook.Models
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
