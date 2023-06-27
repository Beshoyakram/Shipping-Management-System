using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shipping.Models;
using Shipping.Repository;
using Shipping.ViewModels;

namespace Shipping.Controllers
{
    public class EmployeeController : Controller
    {

        IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()

        {

            return View(_employeeRepository.GetAllEmployees());
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            
            _employeeRepository.Add(employeeViewModel);
            _employeeRepository.save();
           
            return RedirectToAction("Index", "Employee");
        }
        [HttpGet]
        public IActionResult Create()
        {
          
            var employeeViewModel = new EmployeeViewModel();


            var branches = _employeeRepository.GetAllBranches();
            var branchList = new SelectList(branches, "Name", "Name");

            ViewBag.BranchList = branchList;


            return View(employeeViewModel);
        }
        public IActionResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);

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
