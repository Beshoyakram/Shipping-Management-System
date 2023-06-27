using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Repository;
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
        public async Task<IActionResult> Index(String Name)
        {
            var deliveryViewModel = await _deliveryRepository.GetAll(Name);
            return View(deliveryViewModel);
        }
        //public async Task<IActionResult> Search(string Name)
        //{
        //    var deliveryViewModel = await _deliveryRepository.GetAll(Name);
        //    return View(deliveryViewModel);
        //}

        #endregion

        #region Add
        public IActionResult add()
        {
            var Branchs = _deliveryRepository.GetAllBranches();

            ViewBag.Branchs = Branchs;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult add(DeliveryViewModel deliveryViewModel) {
            if (ModelState.IsValid)
            {
                _deliveryRepository.AddDelivery(deliveryViewModel);
            }
            else
            {
                var Branchs = _deliveryRepository.GetAllBranches();

                ViewBag.Branchs = Branchs;

                return View(deliveryViewModel);
            }
            return RedirectToAction("Index");
            }
        #endregion

        #region Edit

        public async Task<IActionResult> Edit(string Id)
        {
            var merchant = await _deliveryRepository.GetById(Id);
            if (merchant == null)
            {
                return NotFound();
            }

            var Branchs = _deliveryRepository.GetAllBranches();
            ViewBag.Branchs = Branchs;



            return View(merchant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, DeliveryViewModel deliveryViewModel)
        {
            Delivery delivery = await _deliveryRepository.GetDeliveryById(Id);
            if (ModelState.IsValid)
            {
                _deliveryRepository.EditDelivery(delivery,deliveryViewModel);
            }
            else
            {
                var Branchs = _deliveryRepository.GetAllBranches();

                ViewBag.Branchs = Branchs;

                return View("Edit",deliveryViewModel);
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Delete
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            Delivery delivery = await _deliveryRepository.GetDeliveryById(Id);
            _deliveryRepository.Delete(delivery);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(string Id,bool status)
        {
            Delivery delivery = await _deliveryRepository.GetDeliveryById(Id);
            _deliveryRepository.UpdateStatus(delivery,status);
            return RedirectToAction("Index");
        }
        #endregion


    }
}

