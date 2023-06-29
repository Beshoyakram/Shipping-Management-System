using Shipping.Models;

namespace Shipping.Repository
{
    public interface ICityRepository
    {
        List<City> GetAllByState(int stateId);
        void AddToState(int stateId,City city);
    }
}
