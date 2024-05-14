using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        #region Index
        public async Task<IActionResult> Index(string Search)
        {
            if (string.IsNullOrEmpty(Search))
            {
                //getall
                //will do mapping with manual mapping diffrent way
                var Users = await _userManager.Users.Select(//select accept to send anonymous obj
                    U => new UserViewModel
                    {
                        Id = U.Id,
                        FName = U.FName,
                        LName = U.LName,
                        Email = U.Email,
                        PhoneNumber = U.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(U).Result

                    }).ToListAsync();// MultipleActiveResultsSets = true" to run 2 query at same line in connection string

                return View(Users);
            }
            else
            {
                //get search
                var User = await _userManager.FindByEmailAsync(Search);
                var ManualUserVM = new UserViewModel()
                {
                    Id = User.Id,
                    FName = User.FName,
                    LName = User.LName,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Roles = await _userManager.GetRolesAsync(User)
                };
                return View(new List<UserViewModel> { ManualUserVM });//must return same data type to render one data type


            }



        }
        #endregion


        #region Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
                return NotFound();
            var UserVM = _mapper.Map<ApplicationUser, UserViewModel>(User);
            return View(ViewName, UserVM);
        }
        #endregion


        #region Edit

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel modelVM, [FromRoute] string id)
        {
            if (id != modelVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //var User = _mapper.Map<UserViewModel, ApplicationUser>(modelVM);
                    //this will not work cause the in edit in empController our update (empMapped) its from linq its first search on emp in db then update it 
                    //here UpdateAsync don't do that i must bring user from db then modify it then pass it to updateAsunc

                    var User = await _userManager.FindByIdAsync(id);
                    User.PhoneNumber = modelVM.PhoneNumber;
                    User.FName = modelVM.FName;
                    User.LName = modelVM.LName;

                    var Result = await _userManager.UpdateAsync(User);
                    if (Result.Succeeded)
                        return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(modelVM);

        }


        #endregion


        #region Delete
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete([FromRoute]string id)
        {
            try
            {
                //var user = _mapper.Map<UserViewModel,ApplicationUser>(modelVM);//can't like update
                var User = await _userManager.FindByIdAsync(id);
                var Result = await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }
        #endregion
    }

}
