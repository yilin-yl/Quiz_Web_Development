using System;
using ProjectQuiz.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace ProjectQuiz.DAO
{
	public class FeedbackDAO
	{
        private readonly IConfiguration _configuration;

        public FeedbackDAO(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public int AddFeedback(Feedback feedback)
        {
            string insert_query = "INSERT INTO Feedback VALUES (@rating, @content, @userid)";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(insert_query, conn);
                cmd.Parameters.AddWithValue("@rating", feedback.rating);
                cmd.Parameters.AddWithValue("@content", feedback.content);
                cmd.Parameters.AddWithValue("@userid", feedback.userid);

                try {
                    var rowaffected = cmd.ExecuteNonQuery();
                    return rowaffected;
                } catch (Exception e)
                {
                    return 0;
                }
                
            }
        }
    }
}

