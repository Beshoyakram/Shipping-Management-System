using Shipping.ViewModels;
using Shipping.Models;

namespace Shipping.Repository
{
    public interface IDeliveryRepository
    {
        Task<List<DeliveryViewModel>> GetAll(string Name);
        Task<DeliveryViewModel> GetById(string id);
        Task<bool> AddDelivery(DeliveryViewModel deliveryViewModel);
        void EditDelivery(Delivery delivery ,DeliveryViewModel deliveryViewModel);
        void Delete(Delivery delivery);
        void UpdateStatus(Delivery delivery,bool status);
        Task<Delivery> GetDeliveryById(string id);
        List<Branch> GetAllBranches();
    }
}
