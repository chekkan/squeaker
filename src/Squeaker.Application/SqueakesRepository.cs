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

        public async Task<Squeake[]> FindAll(int limit = 10, int page = 1)
        {
            var items = await this.dbContext.Squeakes
                .Skip(GetOffset(page, limit))
                .Take(limit)
                .ToListAsync();
            return items.ToArray();
        }

        private static int GetOffset(int page, int limit) => (page - 1) * limit;
    }
}
