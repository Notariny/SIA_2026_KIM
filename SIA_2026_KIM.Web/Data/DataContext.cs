using Microsoft.EntityFrameworkCore;
using SIA_2026_KIM.Web.Data.Entities;

namespace SIA_2026_KIM.Web.Data

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :  base(options)
        {


        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Event { get; set; }
    }
}
