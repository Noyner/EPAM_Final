using Epam.Blog.BLL.Interfaces;
using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace Epam.Blog.BLL
{
    class ArticlesLogic : IArticlesLogic
    {
        private IArticleDAO _articlesDAO;

        public ArticlesLogic(IArticleDAO articlesDao)
        {
            _articlesDAO = articlesDao;
        }

        public Article AddArticle(string text, string title, DateTime creationDate) =>
            _articlesDAO.AddArticle(text, title, creationDate);

        public void RemoveArticle(int id) =>
            _articlesDAO.RemoveArticle(id);

        public void RemoveNote(Article article) => RemoveArticle(article.ID);

        public void EditArticle(int id, string newTitle, string newText) =>
            _articlesDAO.EditArticle(id, newTitle, newText);

        public Article GetArticle(int id) => _articlesDAO.GetArticle(id);

        public IEnumerable<Article> GetArticles(bool orderedById = true) => _articlesDAO.GetArticles(orderedById);
    }
}
