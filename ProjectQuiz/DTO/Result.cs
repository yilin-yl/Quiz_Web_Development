using System;
namespace ProjectQuiz.DTO
{
    public class Result
    {
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int score { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string quizname { get; set; }

        public List<QuestionSubmission> question_submissions {get;set;}
    }
}

