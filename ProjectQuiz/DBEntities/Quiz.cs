using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectQuiz.DBEntities
{
    [Table("Quiz")]
    public class Quiz
	{
        public int id { get; set; }
        public string quizname { get; set; }
        public string quiztype { get; set; }

        [InverseProperty("Quiz")]
        public virtual List<Quiz_Question> quiz_Questions { get; set; }
    }
}

