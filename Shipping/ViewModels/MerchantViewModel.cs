using Shipping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shipping.ViewModels
{
    public class MerchantViewModel
    {
        public string? MerchantId { get; set; }
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        [MinLength(10, ErrorMessage ="ادخل الاسم ثلاثي")]
        public string Name { get; set; }
        [Required(ErrorMessage = "يجب ادخال البريد الاليكتروني")]
        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage ="ادخل بريد الكتروني صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم الهاتف")]
        [MaxLength(11, ErrorMessage ="يجب ان يتكون رقم الهاتف من 11 رقم"),MinLength(11,ErrorMessage ="يجب ان يتكون رقم الهاتم من 11 رقم")]
        [RegularExpression("^\\d+$", ErrorMessage ="ادخل رقم صحيح")]
        public string Phone { get; set; }
        public string BranchName { get; set; }
        public string? Role { get; set; }
        public bool Status { get; set; }
        [StringLength(255, ErrorMessage = "ادخل ع الاقل 6احرف و ارقام", MinimumLength = 6)]
        [Required(ErrorMessage = "يجب ادخال الرقم السري")]
        public string Password { get; set; }
        [Required(ErrorMessage = "يجب ادخال العنوان")]
        public string Address { get; set; }
        [Required(ErrorMessage = "يجب ادخال المدينة")]
        public string City { get; set; }
        [Required(ErrorMessage = "يجب ادخال المحافظة")]
        public string Government { get; set; }
        [Required(ErrorMessage = "يجب ادخال سعر pickup")]
        public int PickUpPrice { get; set; }
        [Required(ErrorMessage = "يجب ادخال نسبة تحمل التاجر للرفض")]
        public int RefuseCostPercent{ get; set; }

        public List<SpecialCitiesPrice>? SpecialCities { get; set; }

    }
}
