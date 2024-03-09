
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xsoft.models;

namespace xsoft.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserConfiguration>()
                .HasKey(uc => new { uc.UserId, uc.ConfigurationId });

            modelBuilder.Entity<UserConfiguration>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserConfigurations)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserConfiguration>()
                .HasOne(uc => uc.Configuration)
                .WithMany(c => c.UserConfigurations)
                .HasForeignKey(uc => uc.ConfigurationId);
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Configuration> Configurations => Set<Configuration>();
    }
}