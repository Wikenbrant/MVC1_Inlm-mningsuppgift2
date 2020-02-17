using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inlmämningsuppgift2.Repository
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPaths);

        Task Add(T entity);
        Task Add(IEnumerable<T> entities);
        Task Update(T entity);
        Task Remove(T entity);
        Task Remove(T entity, params object[] propertiesToDelete);
        Task Remove(IEnumerable<T> entities);
        Task Remove(IEnumerable<T> entities, params object[] propertiesToDelete);

        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] navigationPropertyPaths);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> predicate);

        Task<bool> Any(Expression<Func<T, bool>> predicate);
    }
}
