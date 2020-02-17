using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inlmämningsuppgift2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Inlmämningsuppgift2.Repository
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly TomasosContext _context;

        public EfRepository(TomasosContext context)
        {
            _context = context;
        }
        public async Task<T> GetById(int id) => await _context.Set<T>().FindAsync(id);

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
            => _context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPaths)
        {
            IQueryable<T> set = _context.Set<T>();
            foreach (var navigationPropertyPath in navigationPropertyPaths)
            {
                set = set.Include(navigationPropertyPath);
            }
            return await set.FirstOrDefaultAsync(predicate);
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChangesAsync();
        }

        public Task Remove(T entity, params object[] propertiesToDelete)
        {
            foreach (IEnumerable obj in propertiesToDelete)
            {
                foreach (var property in obj)
                {
                    _context.Remove(property);
                }
            }
            _context.Remove(entity);
            return _context.SaveChangesAsync();
        }

        public Task Remove(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            return _context.SaveChangesAsync();
        }
        public Task Remove(IEnumerable<T> entities, params object[] propertiesToDelete)
        {
            foreach (IEnumerable obj in propertiesToDelete)
            {
                foreach (var property in obj)
                {
                    _context.Remove(property);
                }
            }
            foreach (var entity in entities)
            {
                _context.Remove(entity);
            }
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAll(params Expression<Func<T, object>>[] navigationPropertyPaths)
        {
            IQueryable<T> set = _context.Set<T>();
            foreach (var navigationPropertyPath in navigationPropertyPaths)
            {
                set = set.Include(navigationPropertyPath);
            }
            return await set.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public Task<int> CountAll() => _context.Set<T>().CountAsync();

        public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
            => _context.Set<T>().CountAsync(predicate);

        public Task<bool> Any(Expression<Func<T, bool>> predicate) => _context.Set<T>().AnyAsync(predicate);
    }
}
