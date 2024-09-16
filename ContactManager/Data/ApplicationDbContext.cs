using ContactManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ContactManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
    }

}
