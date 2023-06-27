namespace Shipping.ViewModels
{
    public class MerchantViewModel
    {
        public string? MerchantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int BranchId { get; set; }
        public string? Type { get; set; }
        public bool Status { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Government { get; set; }
        public int PickUpPrice { get; set; }
        public int RefuseCostPercent{ get; set; }

    }
}
