using System;
namespace ProjectQuiz.Models
{
	public class Feedback
	{
        public int id { get; set; }
        public int rating { get; set; }
        public string content { get; set; }
        public int userid { get; set; }
    }
}

