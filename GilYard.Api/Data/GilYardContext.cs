using GilYard.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GilYard.Api.Data
{
    public class GilYardContext : DbContext
    {
        public GilYardContext(DbContextOptions<GilYardContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        // public DbSet<Student> Students { get; set; }
        // public DbSet<Grade> Grades { get; set; }

        // protected override void OnModelCreating(DbModelBuilder modelBuilder)
        // {
        //     // configures one-to-many relationship
        //     modelBuilder.Entity<Visitor>()
        //         .HasRequired<User>(s => s.User)
        //         .WithMany(g => g.Students)
        //         .HasForeignKey<int>(s => s.CurrentGradeId);
        // }
    }
}