using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository;
using Shipping.ViewModels;

namespace Shipping.Controllers
{
    public class EmployeeController : Controller
    {

        IEmployeeRepository _employeeRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public EmployeeController(IEmployeeRepository employeeRepository, RoleManager<ApplicationRole> roleManager)
        {
            _employeeRepository = employeeRepository;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()

        {

            return View(await _employeeRepository.GetAllEmployees());
        }
        public async Task<IActionResult> Add()
        {
          
            var employeeViewModel = new EmployeeViewModel();


            var branches = _employeeRepository.GetAllBranches();
            var branchList = new SelectList(branches, "Name", "Name");

            ViewBag.BranchList = branchList;


            var empRoles = await _roleManager.Roles.Where(r=> r.Name!="admin" && r.Name != "التجار" && r.Name != "المناديب").ToListAsync();
            var empRolesSelect = new SelectList(empRoles, "Name", "Name");
            ViewBag.empRoles = empRolesSelect;

            return View(employeeViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel employeeViewModel)
        {
            if(ModelState.IsValid)
            {
                await _employeeRepository.Add(employeeViewModel, employeeViewModel.Role);
                _employeeRepository.save();
                return RedirectToAction("Index", "Employee");
            }
            return View(employeeViewModel);
        }
        public IActionResult Edit(string Id)
        {
            var employee = _employeeRepository.GetEmployeeById(Id);

            if (employee != null)
            {
                ViewBag.BranchList = _employeeRepository.GetAllBranches().ToList();
                return View(employee);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Update(EmployeeViewModel employeeViewModel)
        {
           if (ModelState.IsValid)
            {
                _employeeRepository.Update(employeeViewModel);
                _employeeRepository.save();
            }
           else
           {

                 ViewBag.BranchList = _employeeRepository.GetAllBranches().ToList();
                  return View("Edit", employeeViewModel);
           }
             return RedirectToAction("Index");

            }
            public IActionResult Search(string searchTerm)
        {
            var employees = _employeeRepository.Search(searchTerm);

           
            return View("Index", employees);
        }


        public async Task<IActionResult> ChangeState(string Id, bool status)
        {
            var employee = _employeeRepository.GetById(Id);  
            _employeeRepository.UpdateStatus(employee, status);
            return RedirectToAction("Index");
        }



    }
}
