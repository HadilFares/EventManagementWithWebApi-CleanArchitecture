using Application.Interfaces.CategoryRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
   public class CategoryService : BaseRepository<Category>, ICategoryService
    {
        private readonly EventlyDbContext _context;
        public CategoryService(EventlyDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<List<Category>> GetCategoriesByUserId(string userId)
        {
            return await _context.Categories
                  .Where(c => c.OrganizerId == userId)
                  .ToListAsync();
        }
    }
}
