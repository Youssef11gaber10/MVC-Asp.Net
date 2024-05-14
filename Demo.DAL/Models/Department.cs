using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department
    {
        public int Id { get; set; }//pk
        [Required(ErrorMessage = "Name Is Required")]//this will not save in db so its not best place to wrote it here DA:

        [MaxLength(50)]
        public string Name { get; set; }//.net5 allow null not required
        [Required(ErrorMessage = "Code Is Required")]//this error message render in html page can show asp-validation-for=Name=> name of property if have error  message 
        public string Code { get; set; }
        public DateTime DateOfCreation { get; set; }



        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
        // Navigational Property [Many]

    }
}
