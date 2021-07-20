using System;
using System.Collections.Generic;
using Epam.Blog.Entities;

namespace Epam.Blog.DAL.Interfaces
{
    public interface IArticleDAO
    {
        Article AddArticle(string text, string title, DateTime creationDate);

        IEnumerable<Article> GetArticles(bool orderedByCreationDate = true);

        Article GetArticle(int id);

        void RemoveArticle(int id);

        void EditArticle(int id, string newText, string newTitle);

        int TotalArticles();

        void FillTagMap(int articleId, int tagId);

        int GetArticleIdByName(string name);

        void AddFullArticleWithTags(string title, string text, DateTime creationDate, List<string> tags);

    }
}
