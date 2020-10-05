using Models;

namespace DAL
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {

        public UserRepository() : base()
        {
        }
    }
}
