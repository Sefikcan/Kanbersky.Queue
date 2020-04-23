using Kanbersky.Queue.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Kanbersky.Queue.DAL.Concrete.EntityFramework.Context
{
    public class KanberContext : DbContext
    {
        public KanberContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
