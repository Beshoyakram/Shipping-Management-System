using Shipping.Models;

namespace Shipping.Repository
{
    public class CityRepository : ICityRepository
    {
        MyContext _myContext;
        public CityRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void AddToState(int stateId,City city)
        {
            city.StateId = stateId;
            _myContext.Cities.Add(city);
            _myContext.SaveChanges();
        }

        public List<City> GetAllByState(int stateId)
        {
            return (_myContext.Cities.Where(c=>c.StateId==stateId).ToList());
        }
    }
}
