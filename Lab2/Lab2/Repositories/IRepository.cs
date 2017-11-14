using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Repositories
{
    public interface IRepository<T> where T : class, new() {
            void Add(T entity);
            void Delete(T entity);
            void Update(T entity);
            T Get(int id);
            IEnumerable<T> GetAll();
    }
}
