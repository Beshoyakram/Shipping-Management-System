using Shipping.Models;
using Shipping.ViewModels;

namespace Shipping.Repository.OrderRepo
{
    public interface IOrderRepository
    {
        List<OrderViewModel> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task<OrderViewModel> OrderViewModelById(int id);
        List<OrderViewModel> GetOrderByStatus(string orderStatus);
        void UpdateStatus(Order order, string status);
        void Add(OrderViewModel orderViewModel);
        void Delete(Order order);
        void Edit(Order order, OrderViewModel orderViewModel);
        void UpdateDelivery(Order order, int DeliveryId);

        public List<string> GenerateTable(OrdersPlusDeliverysViewModel OrdersPlusDeliverys);

    }
}