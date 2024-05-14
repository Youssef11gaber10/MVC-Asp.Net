using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork//i ask clr to create obj from class implemetns interface IUnitOfWork that will be have properties refer to objects to all repos
                                                        //, IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository
            , IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }



        #region index
        [HttpGet]
        public async Task<IActionResult> Index(string Search)
        {


            IEnumerable<Employee> employees;
            //dynamic employees;

            if (!string.IsNullOrEmpty(Search))
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeesWithName(Search);//those func returns Employee and View Wait EmployeeViewModel must map
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();//those func returns Employee and View Wait EmployeeViewModel must map
            }

            //var employeesVM = _mapper.Map<IQueryable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            var employeesVM = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            //doesn't matter if IQuerable or IEnumerable


            #region Binding Additional Data to View Template

            //1. ViewData => [dictionary] key-value pair
            //used for transfer data from Controller [action] to its View => .net framework 3.5
            ViewData["Message"] = "hellow from View Data";

            //2. ViewBag => dynamic Property [based on  Dynamic keyword ]
            ViewBag.Message = "hello from View Bag";//both become as "From ViewBag " 
                                                    // both store in same place so , in ViewBag as you overwrtie on  ViewData

            #endregion

            return View(employeesVM);//return EmployeeViewModel

        }
        #endregion


        #region Create


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            //ViewBag.Departments = _departmentRepository.GetAll();//i want sends this to view Create to lists them first
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();//i want sends this to view Create to lists them first
            return View();
        }

        [HttpPost]

        public IActionResult Create(EmployeeViewModel employeeVM)
        {

            if (ModelState.IsValid)
            {
                #region Manual mapping
                ////Mapping Manual
                ////var MappedEmployee = new Employee()
                ////{
                ////    Name = employeeVM.Name,
                ////    Age = employeeVM.Age,
                ////    Address = employeeVM.Address,
                ////    PhoneNumber = employeeVM.PhoneNumber,
                ////    DepartmentId = employeeVM.DepartmentId,
                ////};
                ////Employee employee = (Employee)employeeVM; //also manual need to do explicit overloading , and can't do in Employee because its model can't have function 
                #endregion

                employeeVM.ImageName = DocumentSetttings.UploadFile(employeeVM.Image, "Images");//here i upload file from EmpVm
                //the return fileName will assign in empVm then when mapped will mapped to Employee in Dal to store imageName In db

                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);//here i have Employee have nameImage only , EmpVm have file,fileName
                _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);//here upload Emp with NameImage
                _unitOfWork.CompleteAsync();//to ensure all funtion changes first then saved

                //here not reuire to await them  cause you don't use them in below liens of code
                //so if don't add await will redirect you to index before insure if he created or not 
                //so i know this not supposed but this what will happen if don't add await 
                //and not cause any error
                //but supposed to added



                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(employeeVM);
            }


        }

        #endregion


        #region Details
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();

            var employeeVM = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(ViewName, employeeVM);

        }
        #endregion


        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id, nameof(Edit));
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id) return BadRequest();

            if (ModelState.IsValid)//this required to submit form to make validation here in server side 
                                   //what if i want make like in front client side validation
            {
                try
                {
                    employeeVM.ImageName = DocumentSetttings.UploadFile(employeeVM.Image, "Images");

                    var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmployeeRepository.Update(employee);
                    _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(employeeVM);
        }


        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.DeleteById(employee);
                var rows = await _unitOfWork.CompleteAsync();
                if (rows > 0 && employeeVM.ImageName is not null)
                {
                    DocumentSetttings.DeleteFile(employeeVM.ImageName, "Images");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }


        }


        #endregion








    }
}
