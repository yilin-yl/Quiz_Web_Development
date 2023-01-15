using System;
namespace ProjectQuiz.Models
{
	public class Quiz_Question
	{
        public int id { get; set; }
        public int? quizid { get; set; }
        public int questionid { get; set; }
        public int? selected_optionid { get; set; }
        public int question_no { get; set; }
    }
}

