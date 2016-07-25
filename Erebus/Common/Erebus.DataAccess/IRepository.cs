using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Erebus.Data
{
    public interface IRepository
    {
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class, IPersistableObject;
        IQueryable<TEntity> FindBy<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, IPersistableObject;
        void Add<TEntity>(TEntity entity) where TEntity : class, IPersistableObject;
        void Delete<TEntity>(TEntity entity) where TEntity : class, IPersistableObject;
        void Edit<TEntity>(TEntity entity) where TEntity : class, IPersistableObject;
        void Save();
    }
}
