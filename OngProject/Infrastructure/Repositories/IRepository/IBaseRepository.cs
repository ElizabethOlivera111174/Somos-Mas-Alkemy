using OngProject.Common;
using OngProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Repositories.IRepository
{
    public interface IBaseRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Insert(T entity);
        Task <Result> Delete(int id);
        Task<Result> Update(T entity);
        bool EntityExists(int id);
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<T>> GetPageAsync(Expression<Func<T, object>> order, int limit, int page);
        Task<int> CountAsync();
    }
}
