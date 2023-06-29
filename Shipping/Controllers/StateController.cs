using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.Repository;

namespace Shipping.Controllers
{
    public class StateController : Controller
    {
        IStateRepository _stateRepository;
        public StateController(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public IActionResult Index()
        {
            return View(_stateRepository.GetAll());
        }



        #region Add
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(State state)
        {
            if (ModelState.IsValid)
            {
                _stateRepository.Add(state);
                return RedirectToAction("Index");
            }

            return View(state);
        }

        #endregion


        #region changeStatus
        public async Task<IActionResult> ChangeState(int Id, bool status)
        {
            var state = await _stateRepository.GetById(Id);
            _stateRepository.UpdateStatus(state, status);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
