using System;
namespace ProjectQuiz.DTO
{
	public class AdminResultSummary
	{
        public int quizid { get; set; }
        public string username { get; set; }
        public DateTime starttime { get; set; }
        public DateTime? endtime { get; set; }
        public int? score { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string categoryname { get; set; }
        public int completion { get; set; }
    }
}

