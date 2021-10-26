using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using System.Linq;

namespace BlackHole.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BlackHoleContext context) : base(context) { }


        public User Get(string phoneNumber)
        {
            return _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }
    }
}
