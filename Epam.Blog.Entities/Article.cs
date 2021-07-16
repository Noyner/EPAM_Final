using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.Entities
{
    public class Article
    {
        public Article(string title, string text, int id, DateTime creationDate)
        {
            ID = id;
            Title = title;
            Text = text;
            CreationDate = creationDate;
            Tags = new List<Tag>();
        }

        public Article(string title, string text, int id)
        {
            ID = id;
            Title = title;
            Text = text;
            CreationDate = DateTime.Now;
            Tags = new List<Tag>();
        }

        public Article(string title, string text, DateTime creationDate)
        {
            ID = -1;
            Title = title;
            Text = text;
            CreationDate = creationDate;
            Tags = new List<Tag>();
        }
        public Article(string title, string text)
        {
            ID = -1;
            Title = title;
            Text = text;
            CreationDate = DateTime.Now;
            Tags = new List<Tag>();
        }

        public int ID { get; }

        public string Title { get; set; }

        public string Text { get; set; }

        public virtual IList<Tag> Tags
        { get; set; }

        public DateTime CreationDate { get; set; }

        public void Edit(string newText, string newTitle)
        {
            if (newText == null)
                throw new ArgumentNullException("str", "Text string cannot be null!");

            Text = newText;
            Title = newTitle;
        }
    }
}
