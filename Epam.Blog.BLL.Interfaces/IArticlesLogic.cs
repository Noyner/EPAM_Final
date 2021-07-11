using System;
using System.Collections.Generic;
using Epam.Blog.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.Blog.BLL.Interfaces
{
    public interface IArticlesLogic
    {
        Article AddArticle(string text, string title, DateTime creationDate);

        IEnumerable<Article> GetArticles(bool orderedById = true);

        Article GetArticle(int id);

        void RemoveArticle(int id);

        void EditArticle(int id, string newText, string newTitle);
    }
}
