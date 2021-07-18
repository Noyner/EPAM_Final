using System;
using Epam.Blog.Entities;

namespace Epam.Blog.DAL.Interfaces
{
    public interface IUserDAO
    {
        User AddUser(User user);

        void RemoveUser(int id);

        User GetUserById(int id);

        User GetUserByName(string login);

        void EditUser(int id, string newLogin);

        bool SignIn(string login, string password);
    }
}
