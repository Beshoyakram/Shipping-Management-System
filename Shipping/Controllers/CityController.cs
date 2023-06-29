using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Repository;

namespace Shipping.Controllers
{
    public class CityController : Controller
    {
        ICityRepository _cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }


        public IActionResult Index(int stateId)
        {

            return View(_cityRepository.GetAllByState(stateId));
        }

        #region Add
        public IActionResult Add(int stateId)
        {
            ViewBag.stateId = stateId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(int stateId, City city)
        {
            if (ModelState.IsValid)
            {
                _cityRepository.AddToState(stateId,city);
                return Redirect("/State/Index");
            }

            return View(city);
        }

        #endregion
        
    }
}
