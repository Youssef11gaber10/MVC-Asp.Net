using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class//to ensure can't accept any thing except classes in ths generic
    {
        private readonly MvcAppDbContext _dbContext;

        public GenericRepository(MvcAppDbContext dbContext)//i sends from Employee repo ,department repo obj , here won't create another obj no will use same obj => so leave for readability
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
            //_dbContext.Add(item);
          await  _dbContext.AddAsync(item);
            //_dbContext.SaveChanges();//remove them to controll ransaction fail all or save all in unit of work in Complete() 


        }

        public void DeleteById(T item)
        {
            _dbContext.Remove(item);
            // _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))//this proplem solve with design pattern called (specification)
            {
                return (IEnumerable<T>) await _dbContext.Employees.Include(E => E.Department).ToListAsync();//not wright code

            }

            return await _dbContext.Set<T>().ToListAsync();
            //Set<T>()=>create dbSet of <T>
        }

        public async  Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _dbContext.Update(item);
            // _dbContext.SaveChanges();

        }
    }
}
