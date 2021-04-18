using System.Data.Entity;

namespace IBA_Task_2.Models
{
    /// <summary>
    /// User context
    /// </summary>
    public class UserContext : DbContext
    {
        public UserContext() : base("DbConnection")
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Сountries { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
