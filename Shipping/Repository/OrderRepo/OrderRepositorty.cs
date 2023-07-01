using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Shipping.Models;
using Shipping.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace Shipping.Repository.OrderRepo
{
    public class OrderRepositorty : IOrderRepository
    {
        MyContext _myContext;

        public OrderRepositorty(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async void Add(OrderViewModel orderViewModel)
        {
            var city = _myContext.Cities.FirstOrDefault(c => c.Name == orderViewModel.CityName);
            Order order = new Order()
            {
                CityId = city.Id,
                IsVillage = orderViewModel.IsVillage,
                ClientEmail = orderViewModel.ClientEmail,
                ClientName = orderViewModel.ClientName,
                ClientPhoneNumber1 = orderViewModel.ClientPhoneNumber1,
                ClientPhoneNumber2 = orderViewModel.ClientPhoneNumber2,
                Notes = orderViewModel.Notes,
                OrderCost = orderViewModel.OrderCost,
                PaymentType = orderViewModel.PaymentType,
                ShippingType = orderViewModel.ShippingType,
                StreetName = orderViewModel.StreetName,
                TotalWeight = orderViewModel.TotalWeight,
                Type = orderViewModel.Type,
                StateId = city.StateId

            };

            _myContext.Orders.Add(order);
            _myContext.SaveChanges();
        }

        public List<OrderViewModel> GetAllOrders()
        {
            var Orders = _myContext.Orders.Include(o => o.City).ThenInclude(c => c.State).ToList();
            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();
            foreach (var order in Orders)
            {
                orderViewModels.Add(new OrderViewModel()
                {
                    CityName = order.City.Name,
                    Id = order.SerialNumber,
                    ClientName = order.ClientName,
                    ClientEmail = order.ClientEmail,
                    ClientPhoneNumber1 = order.ClientPhoneNumber1,
                    ClientPhoneNumber2 = order.ClientPhoneNumber2,
                    Notes = order.Notes,
                    OrderCost = order.OrderCost,
                    PaymentType = order.PaymentType,
                    ShippingType = order.ShippingType,
                    StreetName = order.StreetName,
                    TotalWeight = order.TotalWeight,
                    Type = order.Type,
                    IsVillage = order.IsVillage,
                    StateName = order.City.State.Name,
                    OrderDate = order.Date,
                    OrderStatus = order.OrderStatus


                });
            }
            return orderViewModels;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = _myContext.Orders.FirstOrDefault(o => o.SerialNumber == id);

            return order;
        }

        public List<OrderViewModel> GetOrderByStatus(string orderStatus)
        {
            var Orders = _myContext.Orders.Where(o => o.OrderStatus == orderStatus).Include(o => o.City).ThenInclude(c => c.State).ToList();

            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();
            foreach (var order in Orders)
            {
                orderViewModels.Add(new OrderViewModel()
                {
                    CityName = order.City.Name,
                    Id = order.SerialNumber,
                    ClientName = order.ClientName,
                    ClientEmail = order.ClientEmail,
                    ClientPhoneNumber1 = order.ClientPhoneNumber1,
                    ClientPhoneNumber2 = order.ClientPhoneNumber2,
                    Notes = order.Notes,
                    OrderCost = order.OrderCost,
                    PaymentType = order.PaymentType,
                    ShippingType = order.ShippingType,
                    StreetName = order.StreetName,
                    TotalWeight = order.TotalWeight,
                    Type = order.Type,
                    IsVillage = order.IsVillage,
                    StateName = order.City.State.Name,
                    OrderDate = order.Date,
                    OrderStatus = order.OrderStatus


                });
            }
            return orderViewModels;

        }

        public void UpdateStatus(Order order, string status)
        {
            if (order != null)
            {
                order.OrderStatus = status;
                _myContext.SaveChanges();
            }
        }


    }

}
