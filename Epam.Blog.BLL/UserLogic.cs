using System;
using Epam.Blog.Entities;
using Epam.Blog.BLL.Interfaces;
using Epam.Blog.DAL.Interfaces;
using System.Collections.Generic;

namespace Epam.Blog.BLL
{
    public class UserLogic : IUserLogic
    {
        private IUserDAO _userDAO;

        public UserLogic(IUserDAO userDao)
        {
            _userDAO = userDao;
        }
        public User AddUser(User user)
        {
            _userDAO.AddUser(user);
            return user;
        }

        public void RemoveUser(int id)
        {
            _userDAO.RemoveUser(id);
        }


        public void EditUser(int id, string newName)
        {
            _userDAO.EditUser(id, newName);
        }

        public User GetUserById(int id)
        {
            return _userDAO.GetUserById(id);
        }

        public User GetUserByName(string login)
        {
            return _userDAO.GetUserByName(login);
        }

        public bool SignIn(string login, string password)
        {
            return _userDAO.SignIn(login, password);
        }
    }
}
