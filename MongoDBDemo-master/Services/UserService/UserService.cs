using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        string collection = "Users";

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repo.GetAll(collection);
        }

        public long CountThemAll()
        {
            return _repo.Count(collection);
        }

        public User GetSingle(string id)
        {
            return _repo.GetSingle(id, collection);
        }

        public User GetSingleWherePredicate(Expression<Func<User, bool>> predicate)
        {
            return _repo.GetSingleItemPredicate(predicate, collection);
        }
        public IEnumerable<User> GetAllUsersSearch(string input)
        {
           var list = _repo.GetAll(collection);
           return list.Where(y => (y.EmailAdress.Contains(input)) || (y.FirstName.Contains(input) || (y.EmailAdress.Contains(input.ToLower())) || (y.FirstName.Contains(input.ToLower()))));
        }

        public void AddUser(User user)
        {
            _repo.Add(user, collection);
        }

        public void UpdateUser(User user)
        {
            _repo.Update(user, collection);
        }

        public void RemoveUser(User user)
        {
            _repo.Delete(user, collection);
        }
    }
}
