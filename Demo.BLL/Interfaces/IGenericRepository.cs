using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();//to transfer to Async this will do a task so will return task
        public Task<T> GetByIdAsync(int id);

        public Task AddAsync(T item);//Task == void
        
        public void Update(T item);
        public void DeleteById(T item);
    }
}
