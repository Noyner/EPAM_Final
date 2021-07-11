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
        }

        public Article(string title, string text, int id)
        {
            ID = id;
            Title = title;
            Text = text;
            CreationDate = DateTime.Now;
        }

        public Article(string title, string text)
        {
            ID = -1;
            Title = title;
            Text = text;
            CreationDate = DateTime.Now;
        }

        public int ID { get; }

        public string Title { get; set; }

        public string Text { get; private set; }

        public DateTime CreationDate { get; private set; }

        public void Edit(string newText, string newTitle)
        {
            if (newText == null)
                throw new ArgumentNullException("str", "Text string cannot be null!");

            Text = newText;
            Title = newTitle;
        }

    }
}
