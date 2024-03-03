
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace xsoft.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        DbSet<User> users => Set<User>();
    }
}