using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.ViewModels
{
    public class EmployeeViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "يجب ادخال البريد الاليكتروني")]
        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "ادخل بريد الكتروني صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [MaxLength(11, ErrorMessage = "يجب ان يتكون رقم الهاتف من 11 رقم"), MinLength(11, ErrorMessage = "يجب ان يتكون رقم الهاتم من 11 رقم")]
        [RegularExpression("^\\d+$", ErrorMessage = "ادخل رقم صحيح")]
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string BranchName { get; set; }
        public string Role { get; set; }
        [StringLength(255, ErrorMessage = "ادخل ع الاقل خمس ارقام", MinimumLength = 5)]
        [Required(ErrorMessage = "يجب ادخال الرقم السري")]
        public string Password
        {
            get; set;
        }

    }
}
