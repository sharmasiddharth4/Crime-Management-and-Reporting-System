using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlyForIdentity.Models;

namespace OnlyForIdentity.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Report> Reports { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Case> Case { get; set; }
    }
}
