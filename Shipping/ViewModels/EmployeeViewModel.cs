using Shipping.Models;

namespace Shipping.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string BranchName { get; set; }
        public string Role { get; set; }
        public string Password
        {
            get; set;
        }

        }
}
