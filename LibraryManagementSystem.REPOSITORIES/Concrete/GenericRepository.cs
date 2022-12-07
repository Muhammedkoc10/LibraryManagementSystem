using LibraryManagementSystem.ENTITIES.Entities;
using LibraryManagementSystem.REPOSITORIES.Abstract;
using LibraryManagementSystem.REPOSITORIES.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.REPOSITORIES.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
    {
        private readonly LMSDbContext _context;
        public GenericRepository(LMSDbContext context)
        {
            _context = context;
        }
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public List<T> GetAll(string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.ToList();
        }
        public T GetByID(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public bool Update(T item)
        {
            try
            {
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public bool Add(T item)
        {
            _context.Set<T>().Add(item);
            return Save() > 0;
        }

        public bool Add(List<T> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            _context.Set<T>().Remove(item);
            return Save() > 0;
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().FirstOrDefault(exp);
        }

        public List<T> GetActive()
        {
            throw new NotImplementedException();
        }
        public List<T> GetActive(string[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);
            return query.Where(x => x.IsActive == true).ToList();
        }
        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Where(exp).ToList();
        }

    
        public bool Activate(int id)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }
    }
}
