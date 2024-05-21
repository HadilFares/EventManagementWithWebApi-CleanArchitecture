using Application.Interfaces.CategoryRepository;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly EventlyDbContext _context;
        private readonly IBaseRepository<Category> _baseRepository;

        public CategoryService(EventlyDbContext context, IBaseRepository<Category> baseRepository) 
        {
            _context = context;
            _baseRepository=baseRepository;

        }

        public async  Task<Category> CreateCategory(Category category)
        {
          await  _baseRepository.Create(category);
            return category;
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            return await _baseRepository.Delete(id);

        }

        public async Task<Category> FindCategoryByConditionAsync<Category>(Expression<Func<Category, bool>> predicate) where Category : class
        {
            return await _baseRepository.FindByConditionAsync(predicate);

        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _baseRepository.GetAll();
        }

        public async Task<List<String>> GetAllNamesCategories()
        {

            return await _context.Categories
            .Select(c => c.Name)
             .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesByUserId(string userId)
        {
            return await _context.Categories
                  .Where(c => c.OrganizerId == userId)
                  .ToListAsync();
        }

        public async Task<Category> GetCategory(Guid id)
        {

            return await _baseRepository.Get(id);
        }

        public async Task<string> GetCategoryById(Guid id)
        {
            return await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => c.Name)
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

        public async Task<Category> UpdateCategory(Category category)
        {
          await  _baseRepository.Update(category);
            return category;
        }
    
    }
}
