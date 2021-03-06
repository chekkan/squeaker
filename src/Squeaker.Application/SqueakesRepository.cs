﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Squeaker.Application
{
    public class SqueakesRepository : ListSqueakesUseCase, SqueakeByIdUseCase
    {
        private readonly SqueakerContext dbContext;

        public SqueakesRepository(SqueakerContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<(Squeake[], int)> FindAll(int limit = 10, int page = 1)
        {
            var items = await this.dbContext.Squeakes
                .Skip(GetOffset(page, limit))
                .Take(limit)
                .ToListAsync();

            var totalCount = await this.dbContext.Squeakes.CountAsync();

            return (items.ToArray(), totalCount);
        }

        public Task<Squeake> FindById(string id)
            => this.dbContext.Squeakes.FirstAsync(s => s.Id == id);

        private static int GetOffset(int page, int limit) => (page - 1) * limit;
    }
}