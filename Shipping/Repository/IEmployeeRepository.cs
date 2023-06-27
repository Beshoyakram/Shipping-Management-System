 using Shipping.ViewModels;
 using Shipping.Models;


namespace Shipping.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<BranchViewModel> GetAllBranches();
        IEnumerable<EmployeeViewModel> GetAllEmployees();
        IEnumerable<EmployeeViewModel> Search(string query);
        EmployeeViewModel GetEmployeeById(int id);
        Employee GetById(string id);
        void Add(EmployeeViewModel NewEmp);
        void Delete(int id);

        void Update(EmployeeViewModel newemp);
        void UpdateStatus(Employee employee, bool status);
        void save();
    }
}
