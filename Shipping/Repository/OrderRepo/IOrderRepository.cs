using Shipping.Models;
using Shipping.ViewModels;

namespace Shipping.Repository.OrderRepo
{
    public interface IOrderRepository
    {
        List<OrderViewModel> GetAllOrders();
        Task<Order> GetOrderById(int id);
        List<OrderViewModel> GetOrderByStatus(OrderStatus orderStatus);
        void UpdateStatus(Order order, OrderStatus status);
        void Add(OrderViewModel orderViewModel);
    }
}
