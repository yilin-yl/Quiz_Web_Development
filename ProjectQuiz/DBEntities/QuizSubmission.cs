using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectQuiz.DBEntities
{
    [Table("QuizSubmission")]
    public class QuizSubmission
	{
        [Key]
        [Required]
        public int id { get; set; }
        public DateTime starttime { get; set; }
        public DateTime? endtime { get; set; }
        public int? score { get; set; }
        public int userid { get; set; }
        public int quizid { get; set; }

        [ForeignKey("quizid")]
        public virtual Quiz quiz { get; set; }

        [ForeignKey("userid")]
        public virtual UserInfo user { get; set; }
    }
}

