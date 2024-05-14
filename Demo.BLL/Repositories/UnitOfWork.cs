using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork ,IDisposable
    {


        //i need the props refers to obj from (EmpRepo,DeptRepo)
        //when i create obj from UnitofWork
        public UnitOfWork(MvcAppDbContext dbContext)
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        #region Why not do it like that
        //public UnitOfWork(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository
        //    ,MvcAppDbContext dbContext)
        //{
        //    DepartmentRepository = departmentRepository;
        //    EmployeeRepository = employeeRepository;
        //    _dbContext = dbContext;
        //} 
        #endregion


        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        private readonly MvcAppDbContext _dbContext;
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        //can't changed to async i have its signature from IDisposable
        public void Dispose()//must class unit of work implements interface IDisposable
        {
           _dbContext.Dispose();//to close connection with db
        }
    }
}
