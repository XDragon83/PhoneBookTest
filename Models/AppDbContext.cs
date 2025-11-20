using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Phonebook.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
