using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Epam.Blog.SqlDAL
{
    class ArticlesSqlDAO : IArticleDAO
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

        private static SqlConnection _connection = new SqlConnection(_connectionString);

        public IEnumerable<Article> GetArticles(bool orderedById = true)
        {
            using (_connection)
            {
                var query = "SELECT ID, Title, Text, CreationDate FROM Articles"
                    + (orderedById ? " ORDER BY Id" : "");

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

        public Article AddArticle(string text, string title, DateTime creationDate)
        {
            using (_connection)
            {
                var query = "INSERT INTO dbo.Articles(Title, Text, CreationDate) " +
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

        public Article GetArticle(int id)
        {
            using (_connection)
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

        public void RemoveArticle(int id)
        {
            string sql = $"Delete From Articles Where Id='{id}'";
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
