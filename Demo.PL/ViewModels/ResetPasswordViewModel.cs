using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm New Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Doesn't Match")]
        public string ConfirmPassword { get; set; }
       
    }
}
