using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Epam.Blog.SqlDAL
{
    public class ArticlesSqlDAO : IArticleDAO
    {
        private static List<Article> posts = new List<Article>();

        public static string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

        //public static SqlConnection _connection = new SqlConnection(_connectionString);

        public IEnumerable<Article> GetArticles(bool orderedByCreationDate = true)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT ID, Title, Text, CreationDate FROM Articles"
                    + (orderedByCreationDate ? " ORDER BY CreationDate" : "");

                var command = new SqlCommand(query, _connection);

                _connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Article(
                        id: (int)reader["ID"],
                        title: reader["Title"] as string,
                        text: reader["Text"] as string,
                        creationDate: (DateTime)reader["CreationDate"]);
                }
            }
        }

        public Article GetArticle(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Title, Text, CreationDate FROM Articles" + $"WHERE Id='{id}'"; ;

                var command = new SqlCommand(query, _connection);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Article(
                        id: (int)reader["Id"],
                        title: reader["Title"] as string,
                        text: reader["Text"] as string,
                        creationDate: (DateTime)reader["CreationDate"]);
                }
                _connection.Close();

                throw new InvalidOperationException("Cannot find Article with ID = " + id);
            }
        }

        public void FillTagMap(int articleId, int tagId)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO dbo.ArticleTagMap(ArticleId, TagId) " +
                    "VALUES(@ArticleId,@TagId)";

                var command = new SqlCommand(query, _connection);

                command.Parameters.AddWithValue("@ArticleId", articleId);
                command.Parameters.AddWithValue("@TagId", tagId);

                _connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int TotalArticles() // Проверить
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) FROM Articles";
                var command = new SqlCommand(query, _connection);
                _connection.Open();

                int totalArticles = (int)command.ExecuteScalar();
                return totalArticles;
            }
        }

        public Article AddArticle(string title, string text, DateTime creationDate)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO dbo.Articles(Title, Text, CreationDate)" +
                    "VALUES(@Title, @Text, @CreationDate); SELECT CAST(scope_identity() AS INT) AS NewID";
                var command = new SqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Text", text);
                command.Parameters.AddWithValue("@CreationDate", creationDate);

                _connection.Open();

                var result = command.ExecuteScalar();

                if (result != null)
                    return new Article(
                        id: (int)result,
                        title: title,
                        text: text,
                        creationDate: creationDate);

                throw new InvalidOperationException(
                    string.Format("Cannot add Article with parameters: {0}, {1}, {2};",
                    title, text, creationDate));
            }
        }

        public Article GetArticleById(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var stProc = "Articles_GetById";

                var command = new SqlCommand(stProc, _connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@id", id);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Article(
                        id: (int)reader["Id"],
                        title: reader["Title"] as string,
                        text: reader["Text"] as string,
                        creationDate: (DateTime)reader["CreationDate"]);
                }

                throw new InvalidOperationException("Cannot find Article with ID = " + id);
            }
        }

        public int GetArticleIdByName(string name)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id FROM Articles" + $"WHERE Name='{name}'";

                var command = new SqlCommand(query, _connection);

                _connection.Open();

                int articleId = (int)command.ExecuteScalar();
                return articleId;

                throw new InvalidOperationException("Cannot find Article with Name = " + name);
            }
        }

        public void RemoveArticle(int id)
        {
            string sql = $"Delete From dbo.Articles Where Id='{id}'";
            using (var _connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, _connection);

                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public void EditArticle(int id, string newText, string newTitle)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = $"UPDATE dbo.Articles SET Title='{newTitle}', Text='{newText}'" +
                    $"WHERE Id = '{id}'";
                var command = new SqlCommand(query, _connection);

                try
                {
                    _connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}
