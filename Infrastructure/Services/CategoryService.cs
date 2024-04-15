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

        public async Task<String> GetCategoryById(Guid id)
        {
            return await _context.Categories
                .Where(c=> c.Id==id)
                .Select(c=>c.Name)
                .FirstOrDefaultAsync();

        }

        public async Task<Guid> GetCategoryByName(string name)
        {
            Guid categoryId = await _context.Categories
                .Where(c => c.Name == name)
                .Select(c => c.Id)
                 .FirstOrDefaultAsync();

            return categoryId;
        }
    }
}
