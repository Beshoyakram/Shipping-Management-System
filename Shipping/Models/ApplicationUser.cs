using Microsoft.AspNetCore.Identity;

namespace Shipping.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
    }
}
