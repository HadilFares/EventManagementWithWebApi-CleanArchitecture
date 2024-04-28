using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CategoryRepository
{
    public interface ICategoryService
    {
        Task<Category> FindCategoryByConditionAsync<Category>(Expression<Func<Category, bool>> predicate) where Category : class;

        Task<Category> GetCategory(Guid id);
        Task<Category> CreateCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Guid id);
       Task<List<Category>> GetAllCategories();
        Task<List<String>> GetAllNamesCategories();
        Task<List<Category>> GetCategoriesByUserId(string userId);
        Task<Guid> GetCategoryByName(string name);
        Task<String> GetCategoryById(Guid id);
      
    }
}
