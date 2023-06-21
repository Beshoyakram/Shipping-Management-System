using Microsoft.Build.Framework;

namespace Shipping.ViewModels
{
    public class EditRoleViewModel
    {
        public string RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
        
    }
}
