using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.ViewModels
{
    public class DeliveryEditViewModel
    {
        public string? DeliveryId { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "يجب ادخال البريد الاليكتروني")]
        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "ادخل بريد الكتروني صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [MaxLength(11, ErrorMessage = "يجب ان يتكون رقم الهاتف من 11 رقم"), MinLength(11, ErrorMessage = "يجب ان يتكون رقم الهاتم من 11 رقم")]
        [RegularExpression("^\\d+$", ErrorMessage = "ادخل رقم صحيح")]
        public string Phone { get; set; }
        public string BranchName { get; set; }
        [Required(ErrorMessage = "يجب ادخال المحافظة")]
        public string Government { get; set; }
        [Required(ErrorMessage = "يجب ادخال العنوان")]
        public string Address { get; set; }
        public DiscountType DiscountType { get; set; }
        [Required(ErrorMessage = "يجب ادخال نسبة الشركة من الطلب")]
        public int CompanyPercentage { get; set; }

    }
}
