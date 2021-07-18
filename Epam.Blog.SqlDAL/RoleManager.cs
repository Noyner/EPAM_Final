using System;
using System.Web.Security;
using Epam.Blog.DAL.Interfaces;
using Epam.Blog.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Epam.Blog.SqlDAL
{
    public class RoleManager : RoleProvider
    {
        public static string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

        public override bool IsUserInRole(string userlogin, string roleName)
        {
            bool result;
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query1 = "SELECT Id FROM dbo.Users" + $"WHERE Login = '{userlogin}'";
                var query2 = "SELECT RoleId FROM dbo.Roles" + $"WHERE Name='{roleName}'";

                var query3 = "SELECT Id FROM dbo.UserToRoles" + $"WHERE UserId=({query1}) AND RoleId=({query2})";

                var command = new SqlCommand(query3, _connection);

                _connection.Open();

                var reader = command.ExecuteReader();
                result = reader.HasRows;
            }
            return result;
        }

        public override string[] GetRolesForUser(string userlogin)
        {
            List<string> myList = new List<string>();
            using (var _connection = new SqlConnection(_connectionString))
            {
                var user = new UserSqlDAO().GetUserByName(userlogin);
                var query = $"SELECT Name FROM dbo.Roles WHERE Id IN (SELECT RoleId FROM dbo.UserToRoles WHERE UserId={user.Id})";

                var command = new SqlCommand(query, _connection);

                _connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myList.Add(reader["Name"].ToString());
                }
            }
            return myList.ToArray();
        }
        public void AddUserToRole(string userLogin, string roleName)
        {
            var userId = new UserSqlDAO().GetUserByName(userLogin).Id;

            using (var _connection = new SqlConnection(_connectionString))
            {
                var subQuery = "SELECT Id FROM dbo.Roles " + $"WHERE Name='{roleName}'";
                var query = $"INSERT INTO dbo.UserToRoles(UserId, RoleId) SELECT {userId}, Id FROM dbo.Roles WHERE Name='{roleName}'";
                var command = new SqlCommand(query, _connection);

                _connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public override void CreateRole(string roleName)
        {
            using (var _connection = new SqlConnection(_connectionString))
            {
                var query = $"INSERT Name INTO dbo.Roles VALUES({roleName})";
                var command = new SqlCommand(query, _connection);
                _connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        #region NOT_IMPLEMENTED

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}