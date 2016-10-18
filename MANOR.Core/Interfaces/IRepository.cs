using System;
using System.Linq;

namespace MANOR.Core.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        T GetById(IComparable id);
        void Create(T entity);
        void Update(IComparable id, T entity);
        void Delete(IComparable id);
    }
}
