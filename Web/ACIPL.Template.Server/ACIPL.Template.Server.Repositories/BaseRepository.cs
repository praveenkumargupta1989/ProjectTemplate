using ACIPL.Template.Server.DataAccess;
using Ninject;
using System;
using System.Collections.Generic;

namespace ACIPL.Template.Server.Repositories
{
    public interface IBaseRepository<T>
    {
        T SaveOrUpdate(T entity);
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Delete(T entity);
    }

    public class BaseRepository<T> : IBaseRepository<T>
    {
        [Inject]
        public IDataAccess DataAccess { get; set; }

        public T Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public T SaveOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }
    }
}