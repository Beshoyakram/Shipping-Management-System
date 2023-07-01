using Shipping.Models;
using Shipping.ViewModels;

namespace Shipping.Repository.OrderRepo
{
    public interface IOrderRepository
    {
        List<OrderViewModel> GetAllOrders();
        Task<Order> GetOrderById(int id);
        List<OrderViewModel> GetOrderByStatus(string orderStatus);
        void UpdateStatus(Order order, string status);
        void Add(OrderViewModel orderViewModel);
    }
}
