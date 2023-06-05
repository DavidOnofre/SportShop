using Microsoft.EntityFrameworkCore;
using SportsShop.Models;

namespace SportsShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Sport> Sport { get; set; }
    }

   
}
