using Microsoft.EntityFrameworkCore;
using SkopeiBackendAssignment.Entities;

namespace SkopeiBackendAssignment.DataContext
{
    // Entity framwwork data context class for performing data layer actions
    public class TestDataContext : DbContext
    {
        public TestDataContext(DbContextOptions options)
            : base(options)
        {
        }

        // represents the Users Database
        public DbSet<User> Users { get; set; }

        // represents the Products Database
        public DbSet<Product> Products { get; set; }
    }
}