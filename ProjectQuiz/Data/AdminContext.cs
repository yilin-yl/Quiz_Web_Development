using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectQuiz.DBEntities;

namespace ProjectQuiz.Data
{
	public class AdminContext:DbContext
	{
        private readonly IConfiguration _configuration;

        public AdminContext(IConfiguration configuration)
		{
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer(_configuration.GetConnectionString("default"));
        }

        public DbSet<Option> Options { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz_Question> Quiz_Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizSubmission> QuizSubmissions { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
    }
}

