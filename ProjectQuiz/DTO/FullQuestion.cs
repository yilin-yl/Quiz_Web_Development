using System;
using ProjectQuiz.Models;

namespace ProjectQuiz.DTO
{
	public class FullQuestion
	{
		public int? quizid { get; set; }
		public int no { get; set; }
		public Question question { get; set; }
		public Option option1 { get; set; }
        public Option option2 { get; set; }
        public Option option3 { get; set; }
		public int? selected_oid { get; set; }
    }
}

