using GilYard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GilYard.Api.Data
{
    public class GilYardContext : DbContext
    {
        public GilYardContext(DbContextOptions<GilYardContext> options){}

        public DbSet<User> Users { get; set; }
        
        
    }
}