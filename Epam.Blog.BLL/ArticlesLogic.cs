using Epam.Blog.BLL.Interfaces;
using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace Epam.Blog.BLL
{
    public class ArticlesLogic : IArticlesLogic
    {
        public IArticleDAO _articlesDAO;

        public ArticlesLogic(IArticleDAO articlesDao)
        {
            _articlesDAO = articlesDao;
        }

        public Article AddArticle(string title, string text, DateTime creationDate) =>
            _articlesDAO.AddArticle(title, text, creationDate);

        public void RemoveArticle(int id) =>
            _articlesDAO.RemoveArticle(id);

        public void EditArticle(int id, string newTitle, string newText) =>
            _articlesDAO.EditArticle(id, newTitle, newText);

        public Article GetArticle(int id) => _articlesDAO.GetArticle(id);

        public IEnumerable<Article> GetArticles(bool orderedByCreationDate = true) => _articlesDAO.GetArticles(orderedByCreationDate);

        public int TotalArticles() => _articlesDAO.TotalArticles();

        public void FillTagMap(int articleId, int tagId) => _articlesDAO.FillTagMap(articleId, tagId);

        public int GetArticleIdByName(string name) => _articlesDAO.GetArticleIdByName(name);

        public void AddFullArticleWithTags(string title, string text, DateTime creationDate, List<string> tags) => _articlesDAO.AddFullArticleWithTags(title, text, creationDate, tags);
    }
}
