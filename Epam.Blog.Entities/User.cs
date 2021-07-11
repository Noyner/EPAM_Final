using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.Entities
{
    public class User
    {
        public User(string name, DateTime dateOfBirth)
        {
            ID = ID;
            Name = name;
            DateOfBirth = dateOfBirth;
        }
        public int ID { get; private set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
