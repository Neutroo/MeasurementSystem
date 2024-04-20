using MeasurementSystemWebAPI.Models;

namespace MeasurementSystemWebAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        IEnumerable<User> Select();
        User Select(string username);
        void Insert(User user);
        void Delete(Guid id);
        void Save();
    }
}
