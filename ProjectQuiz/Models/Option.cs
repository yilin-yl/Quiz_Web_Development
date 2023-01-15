using System;
namespace ProjectQuiz.Models
{
	public class Option
	{
        public int id { get; set; }
        public string value { get; set; }
        public int iscorrect { get; set; }
        public int questionid { get; set; }
    }
}

