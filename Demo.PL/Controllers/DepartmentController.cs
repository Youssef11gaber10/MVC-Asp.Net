using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Contracts;
using System.Runtime.Intrinsics.X86;
using System;
using Demo.DAL.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    //[AllowAnonymous]//default
    [Authorize]
    //[Authorize("Admin")]
    public class DepartmentController : Controller
    {


        //take ref from interface not concrete class(REPO)
        //program against inteface not concrete class
        //Why Use Interfaces, Not Concrete Classes:
        //Loose Coupling: When a class depends on an interface, it's not tied to a specific concrete class implementation. This allows for flexibility in choosing different implementations depending on the context.
        //Testability: By using interfaces, you can easily mock or stub the dependency during testing.This isolates the unit under test and avoids relying on real external dependencies.
        //Reusability: Interfaces promote code reuse as they define a standard contract that multiple concrete classes can implement.You can use the same interface in various parts of your application.
        // private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork
           //, IDepartmentRepository departmentRepository
           )
        {

            _unitOfWork = unitOfWork;
           //_departmentRepository = departmentRepository;
            //to allow dependcy injection must add this servic to container in startup.cs
            //to make this we have 3 types of obj life time (scoped/singleton/transit) 
        }



        #region Index
        //BaseURL/Department/Index
        public async Task<IActionResult> Index()//this is master page for entity department so will include list of all departments
        {
            //i need to call func get all inside bbl insid repo 
            //so i now need to make obj from repo, (dependeny injection) will create obj from dbcontext and then choose func
            // var departments = _unitOfWork.DepartmentRepository.GetAllAsync().Result;//make it sync again
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();//if don't put await will not wait the GetAllAsync() to finish and return IEnumerabel will return Task<IEnumerable>
            //why i put await cause Var departments need it to complete execution of this func so benefit from getallasync must make it await if use it in context will rely on it until its finish
            return View(departments);//use second overload accept data from controller
                                     //=> controller sends data to view and view bind this data inside it

        }
        #endregion


        #region Create
        [HttpGet]//default
        public IActionResult Create()
        {
            return View();//this not add department this just show the form take data and fill a domy obj form type department
            //to add this domy as real data in db 
        }


        //public IActionResult Create(Department department)//can't differ between 2 actions with overload like functions 
        //only way to differ between them by method (get/post/delete/put)
        [HttpPost]
        public async Task<IActionResult> Create(Department department)//like in url this paramter action get its value 1.form (if exist) 2.segmetn 3.querystring 4file
        {
            if (ModelState.IsValid)//this prop using inside actions only//server side validation
            {

                await _unitOfWork.DepartmentRepository.AddAsync(department);
                int rows = await _unitOfWork.CompleteAsync();

                //Temp Data => Dictionary => transfer data from action to action
                if (rows > 0)
                {
                    TempData["Message"] = "Department is Created";//this sent to action Index then you can use it in its view
                    //this called temp after sends from action to acion its removed can't access again
                    //so if you access this action(index) from another situation  will not sent to it
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(department);//return to same view name of action "Create"but with the data of department
            }

        }

        #endregion


        #region Details
        //baseUrl/Details/10
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();//return status code 400//also IactionResult
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);//get the value of nullabe object //used if ensure here will be a value inside nullable obj
            //var department use what it return from function GetById so must await it or will cause error cause it will be null 
            if (department is null)
                return NotFound();//this IactionResult 
            return View(ViewName, department);

        }

        #endregion


        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();//return status code 400//also IactionResult
            //var department = _departmentRepository.GetById(id.Value);//get the value of nullabe object //used if ensure here will be a value inside nullable obj
            //if (department is null)
            //    return NotFound();//this IactionResult 
            //return View(department);

            return await Details(id, "Edit");//details work async so must await it before return
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1.log exception
                    //2.form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);
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
        public IActionResult Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();
            try
            {
                _unitOfWork.DepartmentRepository.DeleteById(department);
                _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }


        }




        #endregion









    }


}
