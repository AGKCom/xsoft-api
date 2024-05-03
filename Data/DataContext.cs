
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

            // Configure Table-per-Type (TPT) inheritance
            modelBuilder.Entity<Admin>()
                .ToTable("Admins");

            modelBuilder.Entity<User>()
                .ToTable("Users");

            // User and Role Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.roleId);

            // Role and Permission Many-to-Many Relationship
            modelBuilder.Entity<Role>()
                .HasMany(r => r.permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("RolePermissions"));

            // Configuration and User Owner Relationship
            modelBuilder.Entity<Configuration>()
                .HasOne(c => c.owner)
                .WithMany() // If you want to track which configurations a user owns, add a collection in User
                .HasForeignKey(c => c.ownerId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.OwnedConfiguration)
                .WithOne(c => c.owner)
                .HasForeignKey<Configuration>(c => c.ownerId)
                .IsRequired(false);  // If the Configuration is optional

            // Configuration and User Collaborators Many-to-Many Relationship
            modelBuilder.Entity<Configuration>()
                .HasMany(c => c.collaborators)
                .WithMany(u => u.CollaboratingConfigurations)
                .UsingEntity<Dictionary<string, object>>(
                    "ConfigurationUser",
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j => j.HasOne<Configuration>().WithMany().HasForeignKey("ConfigurationId"),
                    j =>
                    {
                        j.ToTable("ConfigurationUsers"); // Table for the join
                        j.HasKey("ConfigurationId", "UserId"); // Primary key for the join table
                    });

            // User and OverridedPermission Relationship
            modelBuilder.Entity<OverridedPermission>()
                .HasOne(op => op.user)
                .WithMany(u => u.userPermissions)
                .HasForeignKey(op => op.userId);

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OverridedPermission> OverridedPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

    }
}