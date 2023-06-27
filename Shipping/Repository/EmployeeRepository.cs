using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.ViewModels;
using System.Security.Policy;

namespace Shipping.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        MyContext _myContext;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<ApplicationUser> _roleManager;

        public EmployeeRepository(MyContext myContext, UserManager<ApplicationUser> userManager)
        {
            _myContext = myContext;
            _userManager = userManager;
            //_userManager = userManager;
            //_roleManager = roleManager;
        }


        #region add
        public void Add(EmployeeViewModel NewEmp)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Email = NewEmp.Email,
                UserName = NewEmp.Email,
                Name = NewEmp.Name,
                PhoneNumber = NewEmp.Phone,
                //Type =TypeOfUser.Employee
            };
            user.PasswordHash = hasher.HashPassword(user,NewEmp.Password);

            var branch = _myContext.Branches.FirstOrDefault(b => b.Name == NewEmp.BranchName);

            
            var employee = new Employee
            {
                User = user,
                Branch = branch,



            };

            
            _myContext.Employees.Add(employee);
            NewEmp.Id = employee.User.Id;
        }
        #endregion


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }



        #region GetAlls
        public IEnumerable<EmployeeViewModel> GetAllEmployees()
        {
            
            var employees = _myContext.Employees.Include(e => e.User).Include(e => e.Branch);

            
            var employeeViewModels = employees.Select(e => new EmployeeViewModel
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Email = e.User.Email,
                Phone = e.User.PhoneNumber,
                BranchName = e.Branch.Name,
                Status = e.User.Status
                
            });

            return employeeViewModels;
        }
        public IEnumerable<BranchViewModel> GetAllBranches()
        {
            // Get all the Branch objects from the database
            var branches = _myContext.Branches;

            // Map the Branch objects to BranchViewModel objects
            var branchViewModels = branches.Select(b => new BranchViewModel
            {
                Id = (int)b.Id,
                Name = b.Name
            });

            return branchViewModels;
        }
        #endregion


        #region getbyid
        public EmployeeViewModel GetEmployeeById(int id)
        {
            var employee = _myContext.Employees
                .Include(e => e.User)
                .Include(e => e.Branch)
                .FirstOrDefault(e => e.Id == id);

            if (employee != null)
            {
                return new EmployeeViewModel
                {
                    Id = employee.User.Id,
                    Name = employee.User.Name,
                    Email = employee.User.Email,
                    Phone = employee.User.PhoneNumber,
                    BranchName = employee.Branch.Name,
                
                };
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Search
        public IEnumerable<EmployeeViewModel> Search(string query)
        {
            var employees = _myContext.Employees
                .Include(e => e.User)
                .Include(e => e.Branch)
                .Where(e => e.User.Name.Contains(query) ||
                            e.User.Email.Contains(query) ||
                            e.User.PhoneNumber.Contains(query) ||
                            e.Branch.Name.Contains(query));

            var employeeViewModels = employees.Select(e => new EmployeeViewModel
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Email = e.User.Email,
                Phone = e.User.PhoneNumber,
                BranchName = e.Branch.Name,
               // Role = e.User.Role
            });

            return employeeViewModels;
        }
        #endregion


        #region update
        public void Update(EmployeeViewModel employeeViewModel)
        {
            
            var employee = _myContext.Employees.Include(e => e.User).Include(e => e.Branch).FirstOrDefault(e => e.User.Id == employeeViewModel.Id);

            if (employee != null)
            {
                
                employee.User.Name = employeeViewModel.Name;
                employee.User.Email = employeeViewModel.Email;
                employee.User.PhoneNumber = employeeViewModel.Phone;
                //employee.User.Role = employeeViewModel.Role;
                employee.User.PasswordHash = employeeViewModel.Password;

                if (employee.Branch.Name != employeeViewModel.BranchName)
                {
                    var branch = _myContext.Branches.FirstOrDefault(b => b.Name == employeeViewModel.BranchName);
                    if (branch != null)
                    {
                        employee.Branch = branch;
                    }
                }
                
            }
        }
        #endregion


        #region updateStatus
        public void UpdateStatus(Employee employee, bool status)
        {
            if (employee != null)
            {
                employee.User.Status = status;
                _myContext.SaveChanges();
            }
        }
        #endregion



        public void save() { _myContext.SaveChanges(); }



        public Employee GetById(string id)
        {
            var emp = _myContext.Employees.Include(e=>e.User).FirstOrDefault(e => e.UserId == id);
            return emp;
        }
    }
}
