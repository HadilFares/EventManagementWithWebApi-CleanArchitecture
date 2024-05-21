using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IBaseRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<int> Create(T entity);
        Task<int> Update(T entity);
        Task<bool> Delete(Guid id);
        Task<T> Get(Guid id);
        Task<T> Get(string id);
        Task<List<T>> GetAll();
        Task<bool> SaveChangesAsync();
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<Category> FindByConditionAsync<Category>(Expression<Func<Category, bool>> predicate) where Category : class;
    }
}
