using BlackHole.Domain.Entities;

namespace BlackHole.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User Get(string phoneNumber);
    }
}
