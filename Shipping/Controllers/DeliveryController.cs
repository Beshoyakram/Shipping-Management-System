using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shipping.Constants;
using Shipping.Models;
using Shipping.Repository.DeliveryRepo;
using Shipping.ViewModels;

namespace Shipping.Controllers
{
    public class DeliveryController : Controller
    {
        IDeliveryRepository _deliveryRepository;
        public DeliveryController(IDeliveryRepository deliveryRepository)
        {
            this._deliveryRepository = deliveryRepository;
        }


        # region GetALL
        [HttpGet]
        [Authorize(Permissions.Deliveries.View)]
        public async Task<IActionResult> Index(String Name)
        {
            var deliveryViewModel = await _deliveryRepository.GetAll(Name);
            return View(deliveryViewModel);
        }
        #endregion

        #region Add
        [HttpGet]
        [Authorize(Permissions.Deliveries.Create)]
        public IActionResult add()
        {
            var Branchs = _deliveryRepository.GetAllBranches().Where(b => b.Status == true);
            ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

            var States = _deliveryRepository.GetAllStates().Where(s => s.Status == true);
            ViewBag.StatesList = new SelectList(States, "Name", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Permissions.Deliveries.Create)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> add(DeliveryViewModel deliveryViewModel)
        {
            if (ModelState.IsValid)
            {
                var res = await _deliveryRepository.AddDelivery(deliveryViewModel);
                if (res != null)
                {
                    foreach (var error in res.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var Branchs = _deliveryRepository.GetAllBranches().Where(b => b.Status == true);
                    ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

                    var States = _deliveryRepository.GetAllStates().Where(s => s.Status == true);
                    ViewBag.StatesList = new SelectList(States, "Name", "Name");
                    return View(deliveryViewModel);
                }
                //if everything is good
                return RedirectToAction("Index");
            }
            else
            {
                var Branchs = _deliveryRepository.GetAllBranches().Where(b => b.Status == true);
                ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

                var States = _deliveryRepository.GetAllStates().Where(s => s.Status == true);
                ViewBag.StatesList = new SelectList(States, "Name", "Name");

                return View(deliveryViewModel);
            }
            
        }
        #endregion

        #region Edit
        [HttpGet]
        [Authorize(Permissions.Deliveries.Edit)]
        public async Task<IActionResult> Edit(string Id)
        {
            var delivery = await _deliveryRepository.GetById(Id);
            if (delivery == null)
            {
                return NotFound();
            }

            var Branchs = _deliveryRepository.GetAllBranches().Where(b => b.Status == true);
            ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

            var States = _deliveryRepository.GetAllStates().Where(s => s.Status == true);
            ViewBag.StatesList = new SelectList(States, "Name", "Name");


            return View(delivery);
        }

        [HttpPost]
        [Authorize(Permissions.Deliveries.Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, DeliveryEditViewModel deliveryEditViewModel)
        {
            Delivery delivery = await _deliveryRepository.GetDeliveryById(Id);
            if (ModelState.IsValid)
            {
                _deliveryRepository.EditDelivery(delivery, deliveryEditViewModel);
            }
            else
            {
                var Branchs = _deliveryRepository.GetAllBranches().Where(b => b.Status == true);
                ViewBag.BranchList = new SelectList(Branchs, "Name", "Name");

                var States = _deliveryRepository.GetAllStates().Where(s => s.Status == true);
                ViewBag.StatesList = new SelectList(States, "Name", "Name");
                return View("Edit", deliveryEditViewModel);
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete
        [Authorize(Permissions.Deliveries.Delete)]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            Delivery delivery = await _deliveryRepository.GetDeliveryById(Id);
            _deliveryRepository.Delete(delivery);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Permissions.Deliveries.Delete)]
        public async Task<IActionResult> ChangeState(string Id, bool status)
        {
            Delivery delivery = await _deliveryRepository.GetDeliveryById(Id);
            _deliveryRepository.UpdateStatus(delivery, status);
            return RedirectToAction("Index");
        }
        #endregion


    }
}

