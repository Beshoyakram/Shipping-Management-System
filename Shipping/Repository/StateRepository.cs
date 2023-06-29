using Microsoft.EntityFrameworkCore;
using Shipping.Models;

namespace Shipping.Repository
{
    public class StateRepository : IStateRepository
    {
        MyContext _myContext;
        public StateRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void Add(State state)
        {
            _myContext.States.Add(state);
            _myContext.SaveChanges();
        }

        public List<State> GetAll()
        {
            return(_myContext.States.ToList());
        }

        public async Task<State> GetById(int id)
        {
            return await _myContext.States.FirstOrDefaultAsync(s => s.Id == id);
        }



        public void UpdateStatus(State state, bool status)
        {
            if (state!= null)
            {
                state.Status = status;
                _myContext.SaveChanges();
            }
        }
    }


}
