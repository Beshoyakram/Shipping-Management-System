using System.ComponentModel.DataAnnotations;

namespace Shipping.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Password annd RePassword not matched.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
