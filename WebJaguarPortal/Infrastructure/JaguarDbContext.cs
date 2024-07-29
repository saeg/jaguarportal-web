using Microsoft.EntityFrameworkCore;
using WebJaguarPortal.Models;

namespace WebJaguarPortal.Infrastructure
{
    public class JaguarDbContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder
        //.UseLazyLoadingProxies();

        public JaguarDbContext(DbContextOptions<JaguarDbContext> options)
            : base(options)
        {
        }

        public DbSet<Settings> Settings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RenewPassword> RenewPasswords { get; set; }

        public DbSet<ControlFlowAnalysis> ControlFlowAnalyzes { get; set; }
        public DbSet<ClassAnalysis> ClassAnalyzes { get; set; }
        public DbSet<FileAnalysis> FileAnalyzes { get; set; }
        public DbSet<LineAnalysis> LineAnalyzes { get; set; }
    }
}
