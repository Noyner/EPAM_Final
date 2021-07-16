using System;
using Epam.Blog.Entities;

namespace Epam.Blog.DAL.Interfaces
{
    public interface IUserDAO
    {
        User AddUser(User user);

        void RemoveUser(int id);

        User GetUserById(int id);

        void EditUser(int id, string newName, DateTime newDateOfBirth);
    }
}
