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
            List<Article> articleList = new List<Article>();
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT a.Id, a.Title, a.Text, a.CreationDate, at.TagId, t.Name FROM dbo.Articles AS a LEFT JOIN dbo.ArticleTagMap as at " +
                    " ON ArticleId = a.Id LEFT JOIN dbo.Tags as t ON t.Id = TagId";
         

                var command = new SqlCommand(query, _connection);

                _connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var article = new Article(
                        id: (int)reader["ID"],
                        title: reader["Title"] as string,
                        text: reader["Text"] as string,
                        creationDate: (DateTime)reader["CreationDate"]);
                    articleList.Add(article);
                }

                List<Article> articleList2 = new List<Article>();
                for (int i = 0; i < articleList.Count; i++)
                {
                    var article = articleList[i];
                    if (articleList2.FirstOrDefault(a => a.ID == article.ID) == null)
                    {
                        articleList2.Add(article);
                    }
                }
                _connection.Close();
                _connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!string.IsNullOrWhiteSpace(reader["TagId"].ToString()))
                    {
                        var tag = new Tag(reader["Name"].ToString(), (int)reader["TagId"]);
                        articleList2.First(u => u.ID == (int)reader["Id"]).Tags.Add(tag);
                    }
                }
                articleList = articleList2;
            }
            return articleList;
        }

        public Article GetArticle(int id)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Title, Text, CreationDate FROM Articles" + $"WHERE Id='{id}'";

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
                var query = "SELECT Id FROM Articles " + $"WHERE Name='{name}'";
                var command = new SqlCommand(query, _connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Name", name);

                _connection.Open();

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return (int)reader["Id"];
                }

                throw new InvalidOperationException("Cannot find Article with Name = " + name);
            }
        } 


        public void RemoveArticle(int id)
        {
            string sql = $"Delete From dbo.Articles Where Id ='{id}'";
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

        public void AddFullArticleWithTags(string title, string text, DateTime creationDate, List<string> tags)
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
                string artId = result.ToString();
                
                var query2 = "SELECT * FROM dbo.Tags WHERE Name IN (";
                for(int i = 0; i<tags.Count; i++)
                {
                    var tag = tags[i];
                    query2 += $"'{tag}'";

                    if (i<tags.Count-1)
                    {
                        query2 += ',';
                    }
                }
                query2 += ")";

                command = new SqlCommand(query2, _connection);
                var reader = command.ExecuteReader();

                List<string> toInsertList = new List<string>();
                List<string> results = new List<string>();
                while (reader.Read())
                {
                    results.Add(reader["Name"].ToString());
                }
             
                _connection.Close();

                foreach(var a in tags)
                {
                    if (!results.Contains(a))
                    {
                        toInsertList.Add(a);
                    }
                }

                if (toInsertList.Count>0)
                {
                    _connection.Open();
                    var query3 = "INSERT INTO dbo.Tags(Name)" +
                    " VALUES ";

                    for (int i = 0; i < toInsertList.Count; i++)
                    {
                        string value = $"('{toInsertList[i]}')";
                        query3 += value;
                        if (i < toInsertList.Count - 1)
                        {
                            query3 += ',';
                        }
                    }

                    command = new SqlCommand(query3, _connection);
                    command.ExecuteScalar();
                    _connection.Close();
                }

                _connection.Open();

                var query4 = "SELECT Id FROM dbo.Tags WHERE Name IN (";
                for (int i = 0; i < tags.Count; i++)
                {
                    var tag = tags[i];
                    query4 += $"'{tag}'";

                    if (i < tags.Count - 1)
                    {
                        query4 += ',';
                    }
                }
                query4 += ")";

                command = new SqlCommand(query4, _connection);
                reader = command.ExecuteReader();

                var query5 = "INSERT INTO dbo.ArticleTagMap(ArticleId, TagId)" +
                    " VALUES ";

                while (reader.Read())
                {
                    string value = $"({artId},'{reader["Id"]}'), ";
                    query5 += value;
                }
                query5 = query5.Substring(0, query5.Length - 2);

                _connection.Close();
                _connection.Open();

                command = new SqlCommand(query5, _connection);
                command.ExecuteScalar();
            }
        }
    }
}
