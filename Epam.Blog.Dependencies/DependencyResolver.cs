using System;
using Epam.Blog.BLL.Interfaces;
using Epam.Blog.DAL.Interfaces;
using Epam.Blog.BLL;
using Epam.Blog.SqlDAL;


namespace Epam.Blog.Dependencies
{
    public class DependencyResolver
    {
        private static DependencyResolver _instance;

        public static DependencyResolver GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DependencyResolver();
            }

            return _instance;
        }

        public IUserDAO UserDAO => new UserSqlDAO();

        public IUserLogic UserLogic => new UserLogic(UserDAO);

        public IArticleDAO ArticleDAO => new ArticlesSqlDAO();

        public IArticlesLogic ArticleLogic => new ArticlesLogic(ArticleDAO);

        public ITagDAO TagDAO => new TagsSqlDAO();

        public ITagsLogic TagLogic => new TagsLogic(TagDAO);
    }
}
