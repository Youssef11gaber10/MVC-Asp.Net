using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }



        #region Index

        public async Task<IActionResult> Index(string Search)
        {
            if (string.IsNullOrEmpty(Search))
            {
                var Roles = await _roleManager.Roles.ToListAsync();
                var modelVM = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(Roles);
                return View(modelVM);
            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(Search);
                var modelVM = _mapper.Map<IdentityRole, RoleViewModel>(Role);
                return View(new List<RoleViewModel>() { modelVM });
            }


        }



        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel modelVM)
        {
            if (ModelState.IsValid)
            {
                //identity role has those attibutes
                //Id
                //Name
                //NormalizedName
                //ConcurrencyStamp
                var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(modelVM);//RoleName in view model changed in In IdenityRole so must added as option in its Profile
                await _roleManager.CreateAsync(MappedRole);
                return RedirectToAction(nameof(Index));

            }
            return View(modelVM);


        }

        #endregion

        #region Details
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role is null)
                return NotFound();
            var RoleVM = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(ViewName, RoleVM);
        }

        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel modelVM, [FromRoute] string id)
        {
            if (id != modelVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);
                    Role.Name = modelVM.RoleName;

                    var Result = await _roleManager.UpdateAsync(Role);
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
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                //var user = _mapper.Map<UserViewModel,ApplicationUser>(modelVM);//can't like update
                var Role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(Role);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        #endregion


        #region AddOrRemoveUsers


        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var Role = await _roleManager.FindByIdAsync(roleId);
            if (Role is null)
                return NotFound();

            ViewData["roleId"]=roleId;

            var UsersInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var UserInRole = new UserInRoleViewModel()//here map the useres in  User in Role
                {
                    Id = user.Id,
                    UserName = user.UserName,
                };
                if (await _userManager.IsInRoleAsync(user, Role.Name))
                {
                    UserInRole.IsSelected = true;
                }
                else
                {
                    UserInRole.IsSelected = false;
                }
                UsersInRole.Add(UserInRole);    //add in list of VM the new Vm you added here
            }
            return View(UsersInRole);


        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewModel> Users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in Users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.Id);
                    if (AppUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(AppUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(AppUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = roleId });
            }

            return View(Users);


        }




        #endregion
    }
}
