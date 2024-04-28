using Application.Interfaces.IBaseRepository;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.BaseRepository
{


    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly EventlyDbContext _context;
        public BaseRepository(EventlyDbContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

       
        public Task<T> Get(Guid id)
        {
            return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<T>> GetAll()
        {
            return _context.Set<T>().ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);

        }
        

        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task<bool> Delete(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<T> Get(string id)
        { 
            // Convert the string id to Guid type
    Guid guidId = new Guid(id);
    
    // Perform the comparison
    return _context.Set<T>().FirstOrDefaultAsync(x => x.Id == guidId);

        }

        public Task<Category> FindByConditionAsync<Category>(Expression<Func<Category, bool>> predicate) where Category : class
        {
            throw new NotImplementedException();
        }
    }
}
