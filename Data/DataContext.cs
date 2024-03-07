
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

        public DbSet<User> users => Set<User>();
        public DbSet<Configuration> configurations => Set<Configuration>();
    }
}