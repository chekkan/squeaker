using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Squeaker.Application;
using Xunit;

namespace Squeaker.UnitTests
{
    public class SqueakesRepositoryTests : IDisposable
    {
        private SqueakerContext dbContext;

        public SqueakesRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<SqueakerContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            this.dbContext = new SqueakerContext(dbContextOptions);
            this.dbContext.Database.OpenConnection();

            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task CanReturnSqueakes()
        {
            var squeakes = GenerateSqueakes(3);
            this.dbContext.Squeakes.AddRange(squeakes);
            this.dbContext.SaveChanges();

            var sut = new SqueakesRepository(this.dbContext);
            var result = await sut.FindAll();

            Assert.Equal(3, result.Length);
            Assert.Equal(squeakes[0].Id, result[0].Id);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public async Task CanLimitTheNumberOfItems(int limit)
        {
            var squeakes = GenerateSqueakes(limit + 3);
            this.dbContext.Squeakes.AddRange(squeakes);
            this.dbContext.SaveChanges();
            var sut = new SqueakesRepository(this.dbContext);

            var result = await sut.FindAll(limit);

            Assert.Equal(limit, result.Length);
        }

        private static Squeake[] GenerateSqueakes(int count)
            => Enumerable.Range(1, count)
                .Select(_ => new Squeake { Id = Guid.NewGuid().ToString() }).ToArray();

        public void Dispose()
        {
            this.dbContext.Database.CloseConnection();
            this.dbContext.Dispose();
        }
    }
}
