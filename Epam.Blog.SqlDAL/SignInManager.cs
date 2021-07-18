using System;
using System.Web.Security;
using Epam.Blog.DAL.Interfaces;
using Epam.Blog.Entities;
using System.Configuration;
using System.Data.SqlClient;

namespace Epam.Blog.SqlDAL
{
    public class SignInManager
    {
        public bool SignIn(string login, string password)
        {
            var dao = new UserSqlDAO();
            bool check = dao.VerifyHashedPassword(dao.Hash(password), password);
            return check;
        }

        public void SignOut(string login)
        {

        }

    }
}
