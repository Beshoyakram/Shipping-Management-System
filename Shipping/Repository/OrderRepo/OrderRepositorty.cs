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
                StateId = city.StateId,
                BranchId = orderViewModel.BranchId,


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
                    OrderStatus = order.OrderStatus,
                    BranchId = order.BranchId,
                    DeliveryId = order.DeliveryId



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
                    OrderStatus = order.OrderStatus,
                    DeliveryId = order.DeliveryId,
                    BranchId = order.BranchId


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

        public void UpdateDelivery(Order order, int DeliveryId)
        {
            if (order != null)
            {
                order.OrderStatus = "قيد_الانتظار";
                order.DeliveryId = DeliveryId;

                _myContext.SaveChanges();
            }
        }


        public List<string> GenerateTable(OrdersPlusDeliverysViewModel OrdersPlusDeliverys)
        {
            List<string> Rows = new List<string>();
            if (OrdersPlusDeliverys.orders.Count != 0)
            {
                for (int i = 0; i < OrdersPlusDeliverys.orders.Count; i++)
                {
                    string x = $"<tr>" +
                                    $"<td>{OrdersPlusDeliverys.orders[i].Id}</td>" +
                                    $"<td>{OrdersPlusDeliverys.orders[i].OrderDate}</td>" +
                                    $"<td>{OrdersPlusDeliverys.orders[i].ClientName} < br/> {OrdersPlusDeliverys.orders[i].ClientPhoneNumber1}</td>" +
                                    $"<td>{OrdersPlusDeliverys.orders[i].StateName}</td>" +
                                    $"<td>{OrdersPlusDeliverys.orders[i].CityName}</td>" +
                                    $"<td>{OrdersPlusDeliverys.orders[i].OrderCost} </td>" +
                                    $"<td> <a class='btn btn-info' > تعديل</a></td>" +
                                    "<td>" +
                                    $"<select id = 'status_{OrdersPlusDeliverys.orders[i].Id}' class='form-select' onchange='changeStatus({OrdersPlusDeliverys.orders[i].Id})'>" +
                        $"<option selected = 'true' disabled value = '{OrdersPlusDeliverys.orders[i].OrderStatus}'>{OrdersPlusDeliverys.orders[i].OrderStatus}</option>";
                    foreach (string statusName in Enum.GetNames(typeof(OrderStatus)))
                    {
                        x += $"<option value = '{statusName}'>{statusName} </option>";
                    }
                    x += $"</select>" +
                            $"</td>" +
                            $"<td><a class= 'btn btn-info'> حذف</a></td>" +
                            "<td><a class= 'btn btn-info'> طباعة</ a></ td >" +
                            "<td>" +
                                $"<select id = 'delivery_{OrdersPlusDeliverys.orders[i].Id}' class= 'form-select' onchange = 'AssignDelivery({OrdersPlusDeliverys.orders[i].Id})'>" +
                                $"<option value = '' > اختر المندوب</option>";
                    for (int j = 0; j < OrdersPlusDeliverys.deliveries.Count; j++)
                    {
                        var item1 = OrdersPlusDeliverys.deliveries[j];
                        if (OrdersPlusDeliverys.orders[i].DeliveryId == null)
                            OrdersPlusDeliverys.orders[i].DeliveryId = -1;

                        if (item1.BranchId == OrdersPlusDeliverys.orders[i].BranchId)
                        {
                            string type = "";
                            if (item1.OrignalIdOnlyInDeliveryTable == OrdersPlusDeliverys.orders[i].DeliveryId)
                                type = "selected";
                            else
                                type = "";

                            x += $"<option {type} value = '{item1.OrignalIdOnlyInDeliveryTable}' >{item1.Name}</option>";
                        }
                    }
                    x += "</select></td></tr>";
                    Rows.Add(x);
                }
            }
            return Rows;

        }

    }

}
