using System.ComponentModel.DataAnnotations;

namespace Shipping.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
