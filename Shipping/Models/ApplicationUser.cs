using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string? Date { get; set; }
    }


    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }
    }
    
    public class ApplicationRoleCliams : IdentityRoleClaim<string>
    {
        public string? ArabicName { get; set; }

     
    }


}
