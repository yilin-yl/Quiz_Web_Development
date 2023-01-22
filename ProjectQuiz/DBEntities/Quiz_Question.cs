using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectQuiz.DBEntities
{
    [Table("Quiz_Question")]
    public class Quiz_Question
	{

        public int id { get; set; }
        public int quizid { get; set; }
        public int questionid { get; set; }
        public int? selected_optionid { get; set; }
        public int? question_no { get; set; }

        [ForeignKey("questionid")]
        public virtual Question question { get; set; }

        [ForeignKey("quizid")]
        [JsonIgnore]
        public virtual Quiz quiz { get; set; }
    }
}

