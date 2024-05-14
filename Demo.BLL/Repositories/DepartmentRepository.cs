using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
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
    public class DepartmentRepository : GenericRepository<Department>,IDepartmentRepository
    {
        //private readonly MvcAppDbContext _dbContext;//no additional fucions so don't use it
        public DepartmentRepository(MvcAppDbContext dbContext):base(dbContext)
        {
            //_dbContext = dbContext;//no additional fucions so don't use it
        }

    }
}

























































//private readonly MvcAppDbContext _dbContext;//object not created yet //this what i handle with dbcontext cause other one create by depend injection its just refrenc not obj
//                                            //when it created? when need to use functions ,and function are public non static so must create obj first
//                                            // so i will make object from dbcontext to open connetion in constructor of this class to ensure
//                                            //only the open connection when use it by obj need to use functions

//public DepartmentRepository(MvcAppDbContext dbContext)//Ask from clr to create object from Dbcontext when create object from this repo
//                                                      //this is dependecy inection the creation of this repo depends on inject obj from dbcontext 
//{
//    // dbContext = new MvcAppDbContext();//open connection
//    //but this have problem each time make obj will make diffrent open to connection this cause error

//    //so will use dependency injection will make obj for first time when create another obj will pass the obj
//    //created before 
//    _dbContext = dbContext;//for say the obj you created i will use it with another ref in class
//}


//public int Add(Department department)
//{
//    //_dbContext.Departments.Add(department);//locally
//    _dbContext.Add(department);//locally
//    return _dbContext.SaveChanges();//remotly //save changes return number of rows affected in db
//}

//public int DeleteById(Department department)
//{
//    _dbContext.Remove(department);
//    return _dbContext.SaveChanges();
//}

//public IEnumerable<Department> GetAll()
//{
//    return _dbContext.Departments.ToList();
//}

//public Department GetById(int id)
//{
//    //var department= _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();//search local first
//    // if(department == null)
//    //     department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();search remotly
//    // return department;

//    return _dbContext.Departments.Find(id);//first serach locall then remotly


//}

//public int Update(Department department)
//{
//    _dbContext.Departments.Update(department);
//    return _dbContext.SaveChanges();
//}
