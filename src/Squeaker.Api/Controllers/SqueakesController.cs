using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Squeaker.Application;

namespace Squeaker.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SqueakesController : ControllerBase
    {
        private readonly ListSqueakesUseCase useCase;

        public SqueakesController(ListSqueakesUseCase useCase)
        {
            this.useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "_limit")] int limit = 10,
            [FromQuery(Name = "_page")] int page = 1)
        {
            var (squeakes, count) = await this.useCase.FindAll(limit, page);
            this.Response.Headers.Add("X-Total-Count", $"{count}");
            return Ok(squeakes);
        }
    }
}