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
        private readonly SqueakerContext dbContext;
        private readonly SqueakesRepository sut;

        public SqueakesRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<SqueakerContext>()
                .UseSqlite("Filename=:memory:")
                .Options;

            this.dbContext = new SqueakerContext(dbContextOptions);
            this.dbContext.Database.OpenConnection();

            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Database.EnsureCreated();

            this.sut = new SqueakesRepository(this.dbContext);
        }

        [Fact]
        public void ImplementsListSqueakesUseCase()
        {
            Assert.IsAssignableFrom<ListSqueakesUseCase>(this.sut);
        }

        [Fact]
        public async Task CanReturnSqueakes()
        {
            var squeakes = GenerateSqueakes(3);
            this.dbContext.Squeakes.AddRange(squeakes);
            this.dbContext.SaveChanges();

            var (result, _) = await this.sut.FindAll();

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

            var (result, _) = await this.sut.FindAll(limit);

            Assert.Equal(limit, result.Length);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public async Task CanPageItems(int page)
        {
            var squeakes = GenerateSqueakes(3 * page + 3);
            this.dbContext.Squeakes.AddRange(squeakes);
            this.dbContext.SaveChanges();

            var (result, count) = await this.sut.FindAll(3, page);

            Assert.Equal(squeakes.Length, count);
            Assert.Equal(squeakes[(page - 1) * 3].Id, result[0].Id);
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
