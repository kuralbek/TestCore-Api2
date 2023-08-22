using Microsoft.EntityFrameworkCore;
using TestCore_Api.ContactDir;

namespace TestCore_Api.Context
{
    public class ApplicContext :DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Auth0;Username=postgres;Password=123");
        }
    }
}
