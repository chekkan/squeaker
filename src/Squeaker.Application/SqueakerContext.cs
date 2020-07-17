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

    public class Squeake
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string CreatedAt { get; set; }
    }
}
