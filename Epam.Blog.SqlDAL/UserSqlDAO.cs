using System;
using System.Collections.Generic;
using System.Linq;
using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;

namespace Epam.Blog.SqlDAL
{
    public class UserSqlDAO : IUserDAO
    {

        public string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString; 
            public User AddUser(User user)
            {
                using (var _connection = new SqlConnection(_connectionString))
                {
                    var query = "INSERT INTO dbo.Users(Id, Name, DateOfBirth) " +
                        "VALUES(@Id,@Name, @DateOfBirth)";
                    var command = new SqlCommand(query, _connection);

                    command.Parameters.AddWithValue("@Id", user.ID);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

                    _connection.Open();
                    command.ExecuteNonQuery();

                    return new User(
                            id: user.ID,
                            name: user.Name,
                            dateOfBirth: user.DateOfBirth);
                }
            }

            public User GetUserById(int id)
            {
                using (var _connection = new SqlConnection(_connectionString))
                {
                    var stProc = "Users_GetUserById";

                    var command = new SqlCommand(stProc, _connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@id", id);

                    _connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return new User(
                            id: (int)reader["Id"],
                            name: reader["Name"] as string,
                            dateOfBirth: (DateTime)reader["DateOfBirth"]);
                    }
                    _connection.Close();

                    throw new InvalidOperationException("Cannot find User with ID = " + id);
                }
            }

            public void RemoveUser(int id)
            {
                string sql = $"Delete From Users Where Id='{id}'";
                using (var _connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand(sql, _connection);

                    _connection.Open();
                    command.ExecuteNonQuery();
                    _connection.Close();
                }
            }

            public void EditUser(int id, string newName, DateTime newDateTimeOfBirth)
            {
                using (var _connection = new SqlConnection(_connectionString))
                {
                    var query = $"UPDATE dbo.Users SET Name='{newName}', DateOfBirth='{newDateTimeOfBirth}'" +
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
