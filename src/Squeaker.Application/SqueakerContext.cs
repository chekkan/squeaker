using Microsoft.EntityFrameworkCore;

namespace Squeaker.Application
{
    public class SqueakerContext : DbContext
    {
        public SqueakerContext(DbContextOptions<SqueakerContext> options)
            : base(options)
        { }

        public DbSet<Squeake> Squeakes { get; set; }
    }
}
