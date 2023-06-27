using Shipping.Models;
using Shipping.ViewModels;

namespace Shipping.Repository
{
    public interface IMerchantRepository
    {

        Task<List<MerchantViewModel>> GetAll(string Name);

        Task<MerchantViewModel> GetById(string id);
        Task<bool> AddMechant(MerchantViewModel merchantViewModel);

        void EditMerchant(Merchant merchant, MerchantViewModel merchantViewModel);

        void UpdateStatus(Merchant merchant, bool status);


    }
}
