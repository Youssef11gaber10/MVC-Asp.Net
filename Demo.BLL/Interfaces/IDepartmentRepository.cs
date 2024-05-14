using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IDepartmentRepository:IGenericRepository<Department>
    {
        //eh lazmet dah ?:
        //this inherts common methods if Department has diffrent additional function will write it here seprated
    }


}















////have all prototype(signature) function for department 
////basicly crud operations
//public IEnumerable<Department> GetAll();
//public Department GetById(int id);

//public int Add(Department department);//return int => numbers of rows affected in db

//public int Update(Department department);
//public int DeleteById(Department department);

