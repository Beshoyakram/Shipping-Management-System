using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.ViewModels
{
    public class DeliveryViewModel
    {
        public string? DeliveryId { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        public string Name { get; set; }
        [Required(ErrorMessage = "يجب ادخال البريد الاليكتروني")]
        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "ادخل بريد الكتروني صحيح")]
        public string Email { get; set; }
        [StringLength(255, ErrorMessage = "ادخل ع الاقل 6احرف و ارقام", MinimumLength = 6)]
        [Required(ErrorMessage = "يجب ادخال الرقم السري")]
        public string Password { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [MaxLength(11, ErrorMessage = "يجب ان يتكون رقم الهاتف من 11 رقم"), MinLength(11, ErrorMessage = "يجب ان يتكون رقم الهاتم من 11 رقم")]
        [RegularExpression("^\\d+$", ErrorMessage = "ادخل رقم صحيح")]
        public string Phone { get; set; }
        public string BranchName { get; set; }
        public string? Type { get; set; }
        [Required(ErrorMessage = "يجب ادخال المحافظة")]
        public string Government { get; set; }
        [Required(ErrorMessage = "يجب ادخال العنوان")]
        public string Address { get; set; }
        public bool status { get; set; }
        public DiscountType DiscountType { get; set; }
        [Required(ErrorMessage = "يجب ادخال نسبة الشركة من الطلب")]
        public int CompanyPercentage { get; set; }
        public int BranchId { get; set; }
        public int? OrignalIdOnlyInDeliveryTable  { get; set; }
        public string? DeliverName { get; set; }
    }
        
}
