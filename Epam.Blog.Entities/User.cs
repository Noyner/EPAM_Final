using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.Entities
{
    public class User
    {
        public User(string login, string password, int id = -1)
        {
            Id = id;
            Login = login;
            PasswordHash = password;
            NormalizedLogin = Login.ToUpper();
        }

        public int Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string NormalizedLogin { get; set; }

    }
}
