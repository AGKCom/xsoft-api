
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
            modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<ProfilePermission>()
                .HasKey(pp => new { pp.PermissionId, pp.ProfileId });

            modelBuilder.Entity<UserConfiguration>()
                .HasKey(uc => new { uc.UserId, uc.ConfigurationId });

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfilePermission> ProfilePermissions { get; set; }
        public DbSet<UserConfiguration> UserConfigurations { get; set; }

    }
}