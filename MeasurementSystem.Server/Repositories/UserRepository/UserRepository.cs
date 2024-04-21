using InfluxDB.Client.Core.Exceptions;
using MeasurementSystemWebAPI.Contexts;
using MeasurementSystemWebAPI.Models;
using System.Data;

namespace MeasurementSystemWebAPI.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgresContext dbContext;

        public UserRepository(PostgresContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<User> Select() => dbContext.Users.AsEnumerable();

        public User Select(string username)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Username == username) 
                ?? throw new NotFoundException($"Пользователь не найден");
            return user;
        }

        public void Insert(User user)
        {
            if (dbContext.Users.Any(u => u.Username == user.Username))
            {
                throw new DuplicateNameException($"Пользователь с именем {user.Username} уже существует");
            }

            dbContext.Users.Add(user);
        }

        public void Delete(Guid id)
        {
            var user = dbContext.Users.Find(id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
            }
        }

        public void Save() => dbContext.SaveChanges();
    }
}
