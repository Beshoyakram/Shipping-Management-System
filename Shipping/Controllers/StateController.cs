using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shipping.Models;
using Shipping.Repository.StateRepo;

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

        public IActionResult Edit(int id)
        {
            var state = _stateRepository.GetById(id);

            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }
        [HttpPost]
        public IActionResult Edit(int id, State state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }

            _stateRepository.Update(id, state);




            return RedirectToAction(nameof(Index));
        }

        #region changeStatus
        public async Task<IActionResult> ChangeState(int Id, bool status)
        {
            var state = _stateRepository.GetById(Id);
            _stateRepository.UpdateStatus(state, status);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
