using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using Shipping.Repository.CityRepo;
using Shipping.Repository.StateRepo;

namespace Shipping.Controllers
{
    public class CityController : Controller
    {
        ICityRepository _cityRepository;
        IStateRepository _stateRepository;
        public CityController(ICityRepository cityRepository, IStateRepository stateRepository)
        {
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
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


        #region change state
        public IActionResult ChangeState(int Id, bool status)
        {
            var City = _cityRepository.GetById(Id);
            City.Status = status;
            _cityRepository.Update(Id, City);
            return RedirectToAction("Index");
        }
        #endregion


        #region Edit
        public IActionResult Edit(int id)
        {
            var city = _cityRepository.GetById(id);

            if (city == null)
            {
                return NotFound();
            }
            var states = _stateRepository.GetAll().ToList();
            ViewBag.StateList = new SelectList(states, "Id", "Name");
            return View(city);
        }
        [HttpPost]
        public IActionResult Edit(int id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }
          
                _cityRepository.Update(id, city);




            return RedirectToAction(nameof(Index), new { stateId = city.StateId });
        }
        #endregion

        #region search
        public IActionResult Search(int id, string query)
        {

            List<City> city;
            if (string.IsNullOrWhiteSpace(query)) { city = _cityRepository.GetAllByState(id).ToList(); }
            else
            {
                city = _cityRepository.GetAllByState(id).Where(i => i.Name.ToUpper().Contains(query.ToUpper())).ToList();

            }
            return View("Index", city);
        }

        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {


            if (id == null)
            {
                return BadRequest();
            }

            City city = _cityRepository.GetById(id);
            city.IsDeleted = true;
            _cityRepository.Update(id, city);




            return RedirectToAction(nameof(Index), new { stateId = city.StateId });
        }

#endregion
    }
}
