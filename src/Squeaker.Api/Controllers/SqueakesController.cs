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
        public async Task<IActionResult> Get()
        {
            var squeakes = await this.useCase.FindAll(10, 1);
            this.Response.Headers.Add("X-Total-Count", $"{squeakes.Length}");
            return Ok(squeakes);
        }
    }
}
