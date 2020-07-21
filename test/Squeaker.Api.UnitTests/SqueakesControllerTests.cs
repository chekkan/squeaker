using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Squeaker.Api.Controllers;
using Squeaker.Application;
using Xunit;

namespace Squeaker.Api.UnitTests
{
    public class SqueakesControllerTests
    {
        private Mock<ListSqueakesUseCase> useCaseMock;
        private SqueakesController sut;

        public SqueakesControllerTests()
        {
            this.useCaseMock = new Mock<ListSqueakesUseCase>();
            this.sut = new SqueakesController(useCaseMock.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                    }
                };
        }

        [Fact]
        public void ImplementsControllerBase()
        {
            Assert.IsAssignableFrom<ControllerBase>(this.sut);
        }

        [Fact]
        public async Task GetReturnsSqueakersFromRepository()
        {
            var expected = GenerateSqueakes(3);
            this.useCaseMock
                .Setup(repo => repo.FindAll(10, 1))
                .ReturnsAsync(expected);

            var result = await this.sut.Get();

            var scResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, scResult.StatusCode);
            var okValue = Assert.IsAssignableFrom<Squeake[]>(scResult.Value);
            Assert.Same(expected, okValue);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task GetReturnsTotalCountResponseHeader(int count)
        {
            var squeakes = GenerateSqueakes(count);
            this.useCaseMock
                .Setup(uc => uc.FindAll(10, 1))
                .ReturnsAsync(squeakes);

            await this.sut.Get();

            Assert.Equal($"{count}", this.sut.Response.Headers["X-Total-Count"]);
        }

        private Squeake[] GenerateSqueakes(int count)
        {
            return Enumerable.Range(1, count)
                .Select(_ => new Squeake{ Id = Guid.NewGuid().ToString()})
                .ToArray();
        }
    }
}
