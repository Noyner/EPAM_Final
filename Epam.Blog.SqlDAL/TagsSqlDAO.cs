using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Epam.Blog.SqlDAL
{
    public class TagsSqlDAO : ITagDAO
    {
        public static string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

        public Tag AddTag(string name)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {

                var query = "INSERT INTO dbo.Tags(Name)" +
                    "VALUES(@Name); SELECT CAST(scope_identity() AS INT) AS NewID";
                var command = new SqlCommand(query, _connection);

                command.Parameters.AddWithValue("@Name", name);

                _connection.Open();

                var result = command.ExecuteScalar();

                if (result != null)
                    return new Tag(
                        id: (int)result,
                        name: name);

                throw new InvalidOperationException(
                    string.Format("Cannot add Tag with name: {0};",
                    name));
            }
        }

        public IEnumerable<Tag> GetTags(bool orderedById = true)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Name FROM Tags"
                    + (orderedById ? " ORDER BY Id" : "");

                var command = new SqlCommand(query, _connection);

                _connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Tag(
                        id: (int)reader["Id"],
                        name: reader["Name"] as string);
                }
            }
        }

        public int GetTagIdByName(string name)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id FROM Tags " + $"WHERE Name='{name}'";

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

                throw new InvalidOperationException("Cannot find Tag with Name = " + name);
            }
        }
    }
}
