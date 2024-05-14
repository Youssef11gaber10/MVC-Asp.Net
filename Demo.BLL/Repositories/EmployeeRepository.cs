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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        private readonly MvcAppDbContext _dbContext;
        public EmployeeRepository(MvcAppDbContext dbContext):base(dbContext)//sends dependancy injection to base constructor in Generic repo
        {
            _dbContext=dbContext;
        }



        //this inherts implementation of generic signature from generic Repo , but must implement additional function in interface
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            return _dbContext.Employees.Where(E => E.Address.Equals(address));
        }

        public IQueryable<Employee> GetEmployeesWithName(string Name)
        {
            return _dbContext.Employees.Include(E=>E.Department).Where(E => E.Name.ToLower().Contains(Name.ToLower()));
        }
    }
}









































//private readonly MvcAppDbContext _dbContext;
//public EmployeeRepository(MvcAppDbContext dbContext)
//{
//    _dbContext = dbContext;
//}



//public int Add(Employee employee)
//{
//    _dbContext.Employees.Add(employee);
//    return _dbContext.SaveChanges();
//}

//public int DeleteById(Employee employee)
//{
//    _dbContext.Remove(employee);
//    return _dbContext.SaveChanges();
//}

//public IEnumerable<Employee> GetAll()
//{
//    return _dbContext.Employees.ToList();
//}

//public Employee GetById(int id)
//{
//    return _dbContext.Employees.Find(id);
//}

//public int Update(Employee employee)
//{
//    _dbContext.Update(employee);
//    return _dbContext.SaveChanges();
//}


























//internal class EmployeeRepository : IEmployeeRepository
//{
//    private readonly MvcAppDbContext _dbContext;
//    public EmployeeRepository(MvcAppDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }



//    public int Add(Employee employee)
//    {
//        _dbContext.Employees.Add(employee);
//        return _dbContext.SaveChanges();
//    }

//    public int DeleteById(Employee employee)
//    {
//        _dbContext.Remove(employee);
//        return _dbContext.SaveChanges();
//    }

//    public IEnumerable<Employee> GetAll()
//    {
//        return _dbContext.Employees.ToList();
//    }

//    public Employee GetById(int id)
//    {
//         return  _dbContext.Employees.Find(id);
//    }

//    public int Update(Employee employee)
//    {
//        _dbContext.Update(employee);
//        return _dbContext.SaveChanges();
//    }
//}
