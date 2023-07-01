using Shipping.Models;

namespace Shipping.Repository.CityRepo
{
    public interface ICityRepository
    {
        List<City> GetAllByState(int stateId);
        void AddToState(int stateId, City city);
        City GetById(int id);


        void Update(int id, City city);
    }
}
