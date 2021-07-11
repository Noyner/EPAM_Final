using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.Entities
{
    public class PersonalPage
    {
        public Guid ID { get; }

        public string Title { get; private set; }

       // User user = new User("Test");

        public PersonalPage(Guid iD, string title)
        {
            ID = iD;
            Title = title;
        }
    }
}
