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
        private readonly ListSqueakesUseCase listUseCase;
        private readonly SqueakeByIdUseCase byIdUseCase;

        public SqueakesController(ListSqueakesUseCase listUseCase,
                                  SqueakeByIdUseCase byIdUseCase)
        {
            this.listUseCase = listUseCase;
            this.byIdUseCase = byIdUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "_limit")] int limit = 10,
            [FromQuery(Name = "_page")] int page = 1)
        {
            var (squeakes, count) = await this.listUseCase.FindAll(limit, page);
            this.Response.Headers.Add("X-Total-Count", $"{count}");
            return Ok(squeakes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var squeake = await this.byIdUseCase.FindById(id);
            return Ok(squeake);
        }
    }
}
