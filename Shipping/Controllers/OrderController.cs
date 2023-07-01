using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.ViewModels;
using System.Text.Json.Serialization;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;
using Shipping.Repository.StateRepo;
using Shipping.Repository.CityRepo;
using Shipping.Repository.OrderRepo;

namespace Shipping.Controllers
{
    public class OrderController : Controller
    {
        IStateRepository _stateRepository;
        ICityRepository _cityRepository;
        IOrderRepository _orderRepository;
        MyContext _myContext;
        public OrderController(IStateRepository stateRepository, ICityRepository cityRepository, MyContext myContext, IOrderRepository orderRepository)
        {
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _myContext = myContext;
            _orderRepository = orderRepository;
        }

        #region ViewAll
        [HttpGet]
        public IActionResult Index(OrderStatus? status = null)
        {
            List<OrderViewModel> Orders;
            if (status == null)
            {
                Orders = _orderRepository.GetAllOrders();
            }
            else
            {
                  Orders =  _orderRepository.GetOrderByStatus(status.Value);
            }

            return View(Orders);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int Id, OrderStatus status)
        {
            var order = await _orderRepository.GetOrderById(Id);
            _orderRepository.UpdateStatus(order, status);
            return RedirectToAction("Index");
        }


        #endregion


        #region Add
        public IActionResult Add()
        {
            ViewBag.States = _stateRepository.GetAll();
            
            ViewBag.Branches = _myContext.Branches.ToList(); ;


            return View();
        }
        [HttpPost]
        public IActionResult Add(OrderViewModel orderViewModel)
        {
            if(ModelState.IsValid)
            {
                _orderRepository.Add(orderViewModel);
                return Redirect("/order/index");
            }

            ViewBag.States = _stateRepository.GetAll();

            ViewBag.Branches = _myContext.Branches.ToList();
            return View(orderViewModel);
        }
        [HttpGet]
        public  IActionResult GetCitiesByState(string state)
        {
            var cities = _cityRepository.GetAllByStateName(state);

            return Json(cities);
        }

        #endregion
    }
}
