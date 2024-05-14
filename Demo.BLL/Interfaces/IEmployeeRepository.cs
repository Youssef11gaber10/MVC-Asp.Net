using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        //eh lazmet dah ?:
        //this inherts common methods if Employee has diffrent additional function will write it here seprated


        //public IEnumerable<Employee> GetByAddress(string address);
        public IQueryable<Employee> GetEmployeesByAddress(string address);
        //iQueryable works first in db , db works Sync so can't do IQueryable as Task if was IEnumrable then can make it as task

        //IEnumerable vs Iqureable 
        //enumerable gets all employees in project then filter them by condition(address)
        //equerable filter in db first then return matches employees => so this better for safe memory

        public IQueryable<Employee> GetEmployeesWithName(string Name);
    }
}




























//public IEnumerable<Employee> GetAll();
//public Employee GetById(int id);

//public int Add(Employee employee);

//public int Update(Employee employee);
//public int DeleteById(Employee employee);
