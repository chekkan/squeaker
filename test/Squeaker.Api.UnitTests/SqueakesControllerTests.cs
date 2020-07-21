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
        [Fact]
        public void ImplementsControllerBase()
        {
            var listUseCase = new Mock<ListSqueakesUseCase>().Object;
            var byIdUseCase = new Mock<SqueakeByIdUseCase>().Object;
            var sut = new SqueakesController(listUseCase, byIdUseCase);
            Assert.IsAssignableFrom<ControllerBase>(sut);
        }

        public class Get
        {
            private readonly Mock<ListSqueakesUseCase> useCaseMock;
            private readonly SqueakesController sut;

            public Get()
            {
                this.useCaseMock = new Mock<ListSqueakesUseCase>();
                var byIdUseCase = new Mock<SqueakeByIdUseCase>().Object;

                this.sut = new SqueakesController(this.useCaseMock.Object, byIdUseCase)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                    }
                };
            }

            [Fact]
            public async Task GetReturnsSqueakersFromRepository()
            {
                var expected = GenerateSqueakes(3);
                this.useCaseMock
                    .Setup(repo => repo.FindAll(10, 1))
                    .ReturnsAsync((expected, 3));

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
                    .ReturnsAsync((squeakes, count));

                await this.sut.Get();

                Assert.Equal($"{count}", this.sut.Response.Headers["X-Total-Count"]);
            }

            [Fact]
            public async Task GetReturnsPaginatedItems()
            {
                var squeakes = GenerateSqueakes(3);
                this.useCaseMock
                    .Setup(uc => uc.FindAll(1, 2))
                    .ReturnsAsync((squeakes, 11));

                var result = await this.sut.Get(1, 2);

                var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
                var okValue = Assert.IsAssignableFrom<Squeake[]>(okResult.Value);
                Assert.Equal(squeakes[0].Id, okValue[0].Id);
                Assert.Equal("11", this.sut.Response.Headers["X-Total-Count"]);
            }
        }

        public class GetById
        {
            private readonly Mock<SqueakeByIdUseCase> byIdUseCaseMock;
            private readonly SqueakesController sut;

            public GetById()
            {
                var listUseCase = new Mock<ListSqueakesUseCase>().Object;
                this.byIdUseCaseMock = new Mock<SqueakeByIdUseCase>();

                this.sut = new SqueakesController(listUseCase,
                                                  this.byIdUseCaseMock.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                    }
                };
            }

            [Fact]
            public async Task ReturnsOk()
            {
                var squeake = GenerateSqueakes(1).Single();

                this.byIdUseCaseMock.Setup(uc => uc.FindById(squeake.Id))
                    .ReturnsAsync(squeake);

                var result = await this.sut.GetById(squeake.Id);

                var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
                var okValue = Assert.IsAssignableFrom<Squeake>(okResult.Value);
                Assert.Equal(squeake.Id, okValue.Id);
            }
        }

        private static Squeake[] GenerateSqueakes(int count)
        {
            return Enumerable.Range(1, count)
                .Select(_ => new Squeake { Id = Guid.NewGuid().ToString() })
                .ToArray();
        }
    }
}
