using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CategoryRepository
{
    public interface ICategoryService:IBaseRepository<Category>
    {
        Task<List<Category>> GetCategoriesByUserId(string userId);
        Task<Guid> GetCategoryByName(string name);
        Task<String> GetCategoryById(Guid id);
      
    }
}
