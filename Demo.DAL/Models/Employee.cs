using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

      
        public int? Age { get; set; }
      
        public string Address { get; set; }

     
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

    
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string ImageName { get; set; }



        [ForeignKey("Department")]//not required if naming  not change and not put it as nullable int
        public int? DepartmentId { get; set; }
        //Fk optional => onDelete: restrict =>can't delete fk (department) have employees
        //Fk required => onDelete: cascade => if delete fk (department) have employees will also delete those employees

        [InverseProperty("Employees")]//not required//if you have one relation per class
        public Department Department { get; set; }
        //Navigational Property [one] by default not loaded


    }
}
