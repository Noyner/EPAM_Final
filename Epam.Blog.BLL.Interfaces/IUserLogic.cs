using System;
using System.Collections.Generic;
using Epam.Blog.Entities;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.BLL.Interfaces
{
    public interface IUserLogic
    {
        User AddUser(User user);

        void RemoveUser(int id);

        User GetUserById(int id);

        void EditUser(int id, string newName, DateTime newDateOfBirth);
    }
}
