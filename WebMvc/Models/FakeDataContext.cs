using Microsoft.EntityFrameworkCore;
using CountryApi.Models;

namespace WebMvc.Models
{
    public class FakeContext : DbContext
    {
        public FakeContext(DbContextOptions<FakeContext> options)
            : base(options)
        {
        }

        public DbSet<CountryItem> CountryItems { get; set; }

        public DbSet<CountryApi.Models.CountryItem> CountryItem { get; set; }
    }
}
