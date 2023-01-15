using System;
namespace ProjectQuiz.Models
{
	public class QuizSubmission
	{
        public int id { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int score { get; set; }
        public int userid { get; set; }
        public int? quizid { get; set; }
    }
}

