
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
   

            modelBuilder.Entity<Client>()
                .HasMany(c => c.collaborators)
                .WithOne(e => e.client)
                .HasForeignKey(e => e.clientId);


            modelBuilder.Entity<Client>()
                .HasMany(c => c.Configurations)
                .WithOne(e => e.client)
                .HasForeignKey(e => e.clientId);
        }
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Configuration> Configurations => Set<Configuration>();
        public DbSet<Collaborator> Collaborators => Set<Collaborator>();
    }
}