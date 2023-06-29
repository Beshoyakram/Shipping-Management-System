using Shipping.Models;

namespace Shipping.Repository
{
    public interface IStateRepository
    {
        List<State> GetAll();
        void Add(State state);


        Task<State> GetById(int id);
        void UpdateStatus(State state, bool status);
    }
}
