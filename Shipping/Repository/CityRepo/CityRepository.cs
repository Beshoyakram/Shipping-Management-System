using Shipping.Models;

namespace Shipping.Repository.CityRepo
{
    public class CityRepository : ICityRepository
    {
        MyContext _myContext;
        public CityRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void AddToState(int stateId, City city)
        {
            city.StateId = stateId;
            _myContext.Cities.Add(city);
            _myContext.SaveChanges();
        }

        public List<City> GetAllByState(int stateId)
        {
            return _myContext.Cities.Where(c => c.StateId == stateId).ToList();
        }

        #region GetById
        public City GetById(int id)
        {
            return _myContext.Cities.Where(p => p.Id == id && p.IsDeleted == false).FirstOrDefault();
        }
        #endregion
        #region Update

        public void Update(int id, City city)
        {
            City oldCity = GetById(id);
            oldCity.Name = city.Name;
            oldCity.Status = city.Status;
            oldCity.StateId = city.StateId;
            oldCity.PickUpPrice = city.PickUpPrice;
            oldCity.ShippingPrice = city.ShippingPrice;
            oldCity.IsDeleted = city.IsDeleted;

            _myContext.SaveChanges();
        }

        #endregion
    }
}
