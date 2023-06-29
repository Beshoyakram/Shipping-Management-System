using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository;
using Shipping.ViewModels;

namespace Shipping.Controllers
{
    public class MerchantController : Controller
    {
        IMerchantRepository _merchantRepo;
        MyContext _myContext;
        public MerchantController(IMerchantRepository merchantRepo, MyContext myContext)
        {
            _merchantRepo = merchantRepo;
            _myContext = myContext;

        }
        #region ViewAll
        public async Task<IActionResult> Index(string Name)
        {
            var merchantViewModels = await _merchantRepo.GetAll(Name);
            return View(merchantViewModels);
        }

        #endregion

        #region Adding
        public IActionResult Add()
        {
            var Branchs = _myContext.Branches.ToList();

            ViewBag.Branchs = Branchs;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(MerchantViewModel merchantViewModel)
        {

            if (ModelState.IsValid)
            {
                bool res = await _merchantRepo.AddMechant(merchantViewModel);
                if(!res)
                {
                    ModelState.AddModelError("", "Error in your data.");
                }
            }
            else
            {
                var Branchs = _myContext.Branches.ToList();

                ViewBag.Branchs = Branchs;

                return View(merchantViewModel);
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(string Id)
        {
            var merchant = await _merchantRepo.GetById(Id);
            var merchantViewModel = await _merchantRepo.MapToViewModel(merchant);
            if (merchant == null)
            {
                return NotFound();
            }

            var Branchs = _myContext.Branches.ToList();
            ViewBag.Branchs = Branchs;



            return View(merchantViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string Id, MerchantViewModel merchantViewModel)
        {
            var merchant = await _merchantRepo.GetById(Id);
            if (ModelState.IsValid)
            {
                _merchantRepo.EditMerchant(merchant, merchantViewModel);
            }
            else
            {
                var Branchs = _myContext.Branches.ToList();

                ViewBag.Branchs = Branchs;

                return View("Edit", merchantViewModel);
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region changeStatus
        public async Task<IActionResult> ChangeState(string Id, bool status)
        {
            var merchant = await _merchantRepo.GetById(Id);
            _merchantRepo.UpdateStatus(merchant, status);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
