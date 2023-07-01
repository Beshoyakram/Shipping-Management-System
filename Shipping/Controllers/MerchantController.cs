using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shipping.Constants;
using Shipping.Models;
using Shipping.Repository.MerchantRepo;
using Shipping.ViewModels;

namespace Shipping.Controllers
{
    public class MerchantController : Controller
    {
        IMerchantRepository _merchantRepo;
        public MerchantController(IMerchantRepository merchantRepo)
        {
            _merchantRepo = merchantRepo;
        }
        #region ViewAll
        [Authorize(Permissions.Merchants.View)]
        [HttpGet]
        public async Task<IActionResult> Index(string Name)
        {
            var merchantViewModels = await _merchantRepo.GetAll(Name);
            return View(merchantViewModels);
        }

        #endregion

        #region Adding
        [HttpGet]
        [Authorize(Permissions.Merchants.Create)]
        public IActionResult Add()
        {
            var Branchs = _merchantRepo.GetAllBranches();
            ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

            var States = _merchantRepo.GetAllStates();
            ViewBag.StatesList = new SelectList(States, "Name", "Name");

            var Cities = _merchantRepo.GetAllCities();
            ViewBag.CitiesList = new SelectList(Cities, "Name", "Name");
            return View();
        }
        [HttpPost]
        [Authorize(Permissions.Merchants.Create)]
        public async Task<IActionResult> Add(MerchantViewModel merchantViewModel)
        {

            if (ModelState.IsValid)
            {
                var res = await _merchantRepo.AddMechant(merchantViewModel);
                if(res != null)
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var Branchs = _merchantRepo.GetAllBranches();
                    ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

                    var States = _merchantRepo.GetAllStates();
                    ViewBag.StatesList = new SelectList(States, "Name", "Name");
                    return View(merchantViewModel);
                }
                //if everything is good
                return RedirectToAction("Index");
            }
            else
            {
                var Branchs = _merchantRepo.GetAllBranches();
                ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

                var States = _merchantRepo.GetAllStates();
                ViewBag.StatesList = new SelectList(States, "Name", "Name");

                return View(merchantViewModel);
            }
        }

        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Permissions.Merchants.Edit)]
        public async Task<IActionResult> Edit(string Id)
        {
            var merchant = await _merchantRepo.GetById(Id);
            var merchantEditViewModel = await _merchantRepo.MapToViewModel(merchant);
            if (merchant == null)
            {
                return NotFound();
            }
            var Branchs = _merchantRepo.GetAllBranches();
            ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

            var States = _merchantRepo.GetAllStates();
            ViewBag.StatesList = new SelectList(States, "Name", "Name");

            var Cities = _merchantRepo.GetAllCities();
            ViewBag.CitiesList = new SelectList(Cities, "Name", "Name");


            return View(merchantEditViewModel);
        }
        [HttpPost]
        [Authorize(Permissions.Merchants.Edit)]
        public async Task<IActionResult> Edit(string Id, MerchantEditViewModel merchantEditViewModel)
        {
            var merchant = await _merchantRepo.GetById(Id);
            if (ModelState.IsValid)
            {
                _merchantRepo.EditMerchant(merchant, merchantEditViewModel);
            }
            else
            {
                var Branchs = _merchantRepo.GetAllBranches();
                ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

                var States = _merchantRepo.GetAllStates();
                ViewBag.StatesList = new SelectList(States, "Name", "Name");

                var Cities = _merchantRepo.GetAllCities();
                ViewBag.CitiesList = new SelectList(Cities, "Name", "Name");
                return View("Edit", merchantEditViewModel);
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region changeStatus
        [Authorize(Permissions.Merchants.Delete)]
        [HttpPost]
        public async Task<IActionResult> ChangeState(string Id, bool status)
        {
            var merchant = await _merchantRepo.GetById(Id);
            _merchantRepo.UpdateStatus(merchant, status);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
