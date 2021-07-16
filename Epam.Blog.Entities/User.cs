using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.Entities
{
    public class User
    {
        public User(int id, string name, DateTime dateOfBirth)
        {
            ID = id;
            Name = name;
            DateOfBirth = dateOfBirth;
        }
        public int ID { get; private set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
