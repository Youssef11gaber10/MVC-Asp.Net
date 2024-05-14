using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        //"signature for property " for each and every IRepo
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public Task<int> CompleteAsync();
        //public void Dispose();//no need cause i must implements them in class from interface IDisposable so signature is set in this Interface
    }
}
