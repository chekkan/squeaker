using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Squeaker.Application
{
    public class SqueakesRepository
    {
        private readonly SqueakerContext dbContext;

        public SqueakesRepository(SqueakerContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Squeake[]> FindAll(int limit = 10)
        {
            var items = await this.dbContext.Squeakes.Take(limit).ToListAsync();
            return items.ToArray();
        }
    }
}
