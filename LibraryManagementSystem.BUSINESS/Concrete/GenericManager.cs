using LibraryManagementSystem.BUSINESS.Abstract;
using LibraryManagementSystem.ENTITIES.Entities;
using LibraryManagementSystem.REPOSITORIES.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.BUSINESS.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : BaseClass
    {
        private readonly IGenericRepository<T> _repository;

        public GenericManager(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        public bool Activate(int id)
        {
            throw new NotImplementedException();
        }

        public bool Add(T item)
        {
            return _repository.Add(item);
        }

        public bool Add(List<T> item)
        {
            throw new NotImplementedException();
        }

        public bool Any(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public List<T> GetActive()
        {
            return _repository.GetActive();
        }
        public List<T> GetActive(string[] includes)
        {
            return _repository.GetActive(includes);
        }

        public List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public List<T> GetAll(string[] includes)
        {
            return _repository.GetAll(includes);
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetByDefault(exp);
        }

        public T GetByID(int id)
        {
            return _repository.GetByID(id);
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            return _repository.GetDefault(exp);

        }

        public bool Remove(T item)
        {
            return _repository.Remove(item);
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public bool Update(T item)
        {
            if (item == null) return false;
            return _repository.Update(item);
        }
    }
}
