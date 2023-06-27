using Shipping.Models;

namespace Shipping.ViewModels
{
    public class DeliveryViewModel
    {
        public string? DeliveryId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public string Phone { get; set; }
        public int BranchId { get; set; }
        public string? Type { get; set; }
        public string Government { get; set; }
        public string Address { get; set; }
        public bool status { get; set; }
        public DiscountType DiscountType { get; set; }
        public int CompanyPercentage { get; set; }
        }
        
}
