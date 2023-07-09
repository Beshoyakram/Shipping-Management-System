using Microsoft.AspNetCore.Mvc;
using Shipping.Models;
using Shipping.ViewModels;
using System.Text.Json.Serialization;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;
using Shipping.Repository.StateRepo;
using Shipping.Repository.CityRepo;
using Shipping.Repository.OrderRepo;
using Shipping.Repository.DeliveryRepo;
using Shipping.Repository.BranchRepo;
using Microsoft.AspNetCore.Identity;
using Azure;
using System.Security.Claims;

namespace Shipping.Controllers
{
    public class OrderController : Controller
    {
        IStateRepository _stateRepository;
        ICityRepository _cityRepository;
        IOrderRepository _orderRepository;
        IDeliveryRepository _deliveryRepository;
        IbranchRepository _branchRepository;

        UserManager<ApplicationUser> _userManager;
        MyContext _myContext;
        public OrderController(IStateRepository stateRepository, ICityRepository cityRepository, MyContext myContext, IOrderRepository orderRepository, IDeliveryRepository deliveryRepository, IbranchRepository ibranchRepository, UserManager<ApplicationUser> userManager)
        {
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _myContext = myContext;
            _orderRepository = orderRepository;
            _deliveryRepository = deliveryRepository;
            _branchRepository = ibranchRepository;
            _userManager = userManager;

        }

        #region ViewAll
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var Orders = _orderRepository.GetAllOrders();
            ViewData["Delivery"] = await _deliveryRepository.GetAll(null);


            ViewBag.Branches = _myContext.Branches.ToList();
            return View(Orders);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeDelivery(int Id, int deliveryId)
        {

            var order = await _orderRepository.GetOrderById(Id);

            _orderRepository.UpdateDelivery(order, deliveryId);

            var Orders = _orderRepository.GetAllOrders();
            ViewData["Delivery"] = await _deliveryRepository.GetAll(null);


            ViewBag.Branches = _myContext.Branches.ToList();
            return View("Index", Orders);
        }




        [HttpPost]
        public async Task<IActionResult> GetOrdersDependonStatus(string? status = null)
        {
            List<OrderViewModel> Orders;
            if (status == null)
            {
                Orders = _orderRepository.GetAllOrders();
            }
            else
            {
                Orders = _orderRepository.GetOrderByStatus(status);
            }
            var dileveryList = await _deliveryRepository.GetAll(null);
            var OrdersPlusDeliverys = new OrdersPlusDeliverysViewModel();
            OrdersPlusDeliverys.orders = Orders;
            OrdersPlusDeliverys.deliveries = dileveryList;

            var Rows = _orderRepository.GenerateTable(OrdersPlusDeliverys);
            return Json(Rows);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int Id, string status)
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
            ViewBag.Branches = _myContext.Branches.ToList();           

            return View();
        }
        [HttpPost]
        public IActionResult Add(OrderViewModel orderViewModel)
        {

            if (ModelState.IsValid)
            {

                _orderRepository.CalcShipping(orderViewModel);

                _orderRepository.Add(orderViewModel);
                return Redirect("/order/index");
            }

            ViewBag.States = _stateRepository.GetAll();

            ViewBag.Branches = _myContext.Branches.ToList();
            return View(orderViewModel);
        }
        [HttpGet]
        public IActionResult GetCitiesByState(string state)
        {
            var cities = _cityRepository.GetAllByStateName(state);

            return Json(cities);
        }

        #endregion

        #region  GetBranchesByState
        public IActionResult GetBranchesByState(string state)
        {
            var branches = _branchRepository.GetBranchesByStateName(state);

            return Json(branches);
        }
        #endregion

        #region search

        #region SearchByClientName
        public async Task<IActionResult> SearchByClientName(string query)
        {
            List<OrderViewModel> Orders;
            if (string.IsNullOrWhiteSpace(query)) { Orders = _orderRepository.GetAllOrders().ToList(); }
            else
            {
                Orders = _orderRepository.GetAllOrders().Where(i => i.ClientName.ToUpper().Contains(query.ToUpper())).ToList();

            }
            var x = ViewData["Delivery"] = await _deliveryRepository.GetAll(null);
            ViewBag.Deliverys = await _deliveryRepository.GetAll(null);


            ViewBag.Branches = _myContext.Branches.ToList();
            return View("Index", Orders);
        }

        #endregion

        #region SearchByDeliveryName
        public async Task<IActionResult> SearchByDeliveryName(string query)
        {
            List<OrderViewModel> Orders = new List<OrderViewModel>();

            var delivery = await _deliveryRepository.GetAll(null);

            if (string.IsNullOrWhiteSpace(query)) { 
                Orders = _orderRepository.GetAllOrders().ToList();
                RedirectToAction("Index");
            }
            else
            {


                var deliveryAfterFilteration = delivery.Where(x => x.DeliverName.ToUpper().Contains(query.ToUpper())).ToList();
                foreach (var item in deliveryAfterFilteration)
                {
                    var ordersAfterFilter = _orderRepository.GetAllOrders().Where(i => i.DeliveryId == item.OrignalIdOnlyInDeliveryTable).ToList();
                    foreach (var item1 in ordersAfterFilter)
                    {
                        Orders.Add(item1);
                    }
                }


            }

            ViewData["Delivery"] = await _deliveryRepository.GetAll(null);
            ViewBag.Deliverys = await _deliveryRepository.GetAll(null);


            ViewBag.Branches = _myContext.Branches.ToList();
            return View("Index", Orders);
        }
        #endregion
        #endregion

        #region edit

        public async Task<IActionResult> Edit(int Id)
        {
            ViewBag.States = _stateRepository.GetAll();



            ViewBag.Branches = _myContext.Branches.ToList();

            var orderViewModel = await _orderRepository.OrderViewModelById(Id);

            ViewBag.Cities = _cityRepository.GetAllByStateName(orderViewModel.StateName).ToList();

            return View(orderViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int Id, OrderViewModel orderViewModel)
        {

            if (ModelState.IsValid)
            {
                
                var order = await _orderRepository.GetOrderById(Id);
                _orderRepository.Edit(order, orderViewModel);
                return Redirect("/order/index");
            }

            ViewBag.States = _stateRepository.GetAll();
            ViewBag.Branches = _myContext.Branches.ToList();
            return View(orderViewModel);
        }

        #endregion

        #region Delete
        [HttpGet, ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var order = await _orderRepository.GetOrderById(Id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int Id)
            {
            var order = await _orderRepository.GetOrderById(Id);
            if (order == null)
            {
                return NotFound();
            }
            _orderRepository.Delete(order);
            return RedirectToAction("Index");
        }
        #endregion

        #region OrderCount
        public IActionResult OrderCount()
        {
            

                string roleName = User.FindFirstValue(ClaimTypes.Role);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _orderRepository.GetAllOrders();

            ViewBag.RoleName=roleName;
            if (roleName == "Admin" || roleName == "الموظفين")           
            { return View(result); }
            else if (roleName == "التجار")
            {
                var intMerchantId = _myContext.Merchants.Where(p => p.UserId == userId).Select(p => p.Id).FirstOrDefault();
                return View(result.Where(p => p.MerchantId == intMerchantId).ToList());
            }
            else if (roleName == "المناديب")
            {
                var intDeliveryId = _myContext.Deliveries.Where(p => p.UserId == userId).Select(p => p.Id).FirstOrDefault();
                return View(result.Where(p => p.DeliveryId == intDeliveryId).ToList());
            }
            else
            {
                return BadRequest();
            }
            
        }

        #endregion

        #region related to the count screen
        public async Task<IActionResult> IndexAfterFilter(string query)
        {
            string roleName = User.FindFirstValue(ClaimTypes.Role);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            
            var Orders = _orderRepository.GetAllOrders().ToList();

            ViewData["Delivery"] = await _deliveryRepository.GetAll(null);
            var newOrders = Orders.Where(p => p.OrderStatus == query).ToList();

            ViewBag.Branches = _myContext.Branches.ToList();
            ViewBag.RoleName = roleName;



            if (roleName == "Admin" || roleName == "الموظفين")
            { return View("index", model: newOrders); }
            else if (roleName == "التجار")
            {

                var intMerchantId = _myContext.Merchants.Where(p => p.UserId == userId).Select(p => p.Id).FirstOrDefault();
                var result= Orders.Where(p => p.OrderStatus == query &&p.MerchantId==intMerchantId).ToList();
                return View(result);
            }
            else if (roleName == "المناديب")
            {

                var intDeliveryId = _myContext.Deliveries.Where(p => p.UserId == userId).Select(p => p.Id).FirstOrDefault();
                var result = Orders.Where(p => p.OrderStatus == query && p.DeliveryId == intDeliveryId).ToList();
                return View(result);
            }
            else
            {
                return BadRequest();
            }


        }
       
        #endregion
    }
}
