using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ProjectQuiz.Models;

namespace ProjectQuiz.DAO
{
    public class UserDAO
    {
        private readonly IConfiguration _configuration;

        public UserDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Get a user object based on username provided
        public UserInfo GetUser(string username)
        {
            string connString = _configuration.GetConnectionString("default");
            string query = "SELECT * FROM UserInfo WHERE username = @username";
            var res = new UserInfo();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.id = Convert.ToInt32(reader["id"]);
                        res.username = Convert.ToString(reader["username"]);
                        res.password = Convert.ToString(reader["password"]);
                        res.firstname = Convert.ToString(reader["firstname"]);
                        res.lastname = Convert.ToString(reader["lastname"]);
                        res.admin = Convert.ToInt32(reader["admin"]);
                        break;
                    }
                }
            }
            return res;
        }

        //Add a user (register)
        public void AddUser(UserInfo user)
        {
            string insert_query = "INSERT INTO UserInfo VALUES (@username, @password, @firstname, @lastname, @status, @address, @email)";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(insert_query, conn);
                cmd.Parameters.AddWithValue("@username", user.username);
                cmd.Parameters.AddWithValue("@password", user.password);
                cmd.Parameters.AddWithValue("@firstname", user.firstname);
                cmd.Parameters.AddWithValue("@lastname", user.lastname);
                cmd.Parameters.AddWithValue("@status", 1);
                cmd.Parameters.AddWithValue("@address", user.address);
                cmd.Parameters.AddWithValue("@firstname", user.email);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

