using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.UserServices
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        long CountThemAll();

        User GetSingle(string id);

        User GetSingleWherePredicate(Expression<Func<User, bool>> predicate);

        void AddUser(User u);

        void UpdateUser(User u);

        void RemoveUser(User u);

        IEnumerable<User> GetAllUsersSearch(string input);

    }
}
