 using Shipping.ViewModels;
 using Shipping.Models;


namespace Shipping.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<BranchViewModel> GetAllBranches();
        Task<List<EmployeeViewModel>> GetAllEmployees();
        IEnumerable<EmployeeViewModel> Search(string query);
        EmployeeViewModel GetEmployeeById(string id);
        Employee GetById(string id);
        Task<bool> Add(EmployeeViewModel NewEmp, string role);
        void Delete(int id);

        void Update(EmployeeViewModel newemp);
        void UpdateStatus(Employee employee, bool status);
        void save();
    }
}
