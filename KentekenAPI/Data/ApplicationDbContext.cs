using Microsoft.EntityFrameworkCore;
using KentekenAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KentekenAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
