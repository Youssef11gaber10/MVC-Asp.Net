using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;// to use funtions in its repo (Crud)
			_signInManager = signInManager;
		}



		//public IActionResult Index()//i don't have master page in account controller
		// {
		//     return View();
		// }



		#region Register
		//Reqister
		//baseURl/Account/Register
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel modelVM)
		{
			if (ModelState.IsValid) //server side validation
			{//we will do it manual mapping

				var User = new ApplicationUser()
				{
					UserName = modelVM.Email.Split('@')[0],//split when find @ then take first element => username@gmail.com
					Email = modelVM.Email,
					FName = modelVM.FName,
					LName = modelVM.LName,
					IsAgree = modelVM.IsAgree,

				};
				//createAsync inside it use some repos in Di and need to allow Di to those repos that come from interfaces
				var Result = await _userManager.CreateAsync(User, modelVM.Password);
				//this result have status 
				if (Result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
					//after that also he go out if then return to view with modelVM data agains
				}

			}
			return View(modelVM);

		}



		#endregion


		#region Login

		//Login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel modelVM)
		{
			if (ModelState.IsValid)//server side validation
			{
				var user = await _userManager.FindByEmailAsync(modelVM.Email);
				if (user is not null)
				{
					//login
					var Result = await _userManager.CheckPasswordAsync(user, modelVM.Password);
					//here compare hashed password with passord you give him after same hashed
					if (Result)
					{
						//login//this function need to add service to generate token when login 
						var LoginResult = await _signInManager.PasswordSignInAsync(user, modelVM.Password, modelVM.RememberMe, false);
						if (LoginResult.Succeeded)
						{
							return RedirectToAction("index", "Home");
						}
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Password is InCorrect");
					}
				}

			}
			else
			{
				ModelState.AddModelError(string.Empty, "Email isn't exist");
			}

			return View(modelVM);

		}

		#endregion

		#region SignOut

		//Signout
		public new async Task<IActionResult> SignOut()//there another version of signout in inherts class controller so put "new" to hide it and use this
													  //cause i don't know its implemenation
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}


		#endregion


		#region ForgetPassword & ResetPassword


		#region Forget password
		//Forget password
		public IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel modelVM)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(modelVM.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					//https://localhost:44351/Account/ResetPassword?email=UserName@gmail.com?Token=dnkasklkjkljlkjklj
					//var ResePasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email,Token=token },"https", "localhost:44351");
					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, Token = token }, Request.Scheme);


					//send email
					var email = new Email()
					{
						To = user.Email,
						Subject = "Reset PassWord",
						Body = ResetPasswordLink
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));

				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email isn't Exist");
				}
			}

			return View("ForgetLPassword", modelVM);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}

		#endregion

		#region Resetpasword

		//Reset password
		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel modelVM)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);

				var Result = await _userManager.ResetPasswordAsync(user, token, modelVM.Password);
				if (Result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					foreach (var error in Result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);
				}

				
			}
			return View(modelVM);
		}


		#endregion



		#endregion





	}
}

