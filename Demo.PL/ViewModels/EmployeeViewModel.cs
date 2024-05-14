using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        [MaxLength(50, ErrorMessage = "Max length is 50 chars")]
        [MinLength(5, ErrorMessage = "Min length is 5 chars")]
        public string Name { get; set; }

        [Range(22, 35, ErrorMessage = "Age Must Be In Range From 22 To 35")]
        public int? Age { get; set; }
        [RegularExpression(
            "^[0-9]{1,3}-[a-zA-z]{5,10}-[a-zA-z]{4,10}-[a-zA-z]{5,10}$"
            , ErrorMessage = ("Address Must Be Like 123-City-Country"))]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        //public DateTime CreationDate { get; set; } = DateTime.Now; //not needed in ViewModel



        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }//needed in view to take value of id


        [InverseProperty("Employees")]
        public Department Department { get; set; }//needed in view to show value of deptName


        public IFormFile Image { get; set; }

        public string ImageName { get; set; }



    }

}
