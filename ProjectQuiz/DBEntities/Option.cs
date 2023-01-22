using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectQuiz.DBEntities
{
    [Table("Options")]
	public class Option
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string value { get; set; }
        public int iscorrect { get; set; }
        public int questionid { get; set; }

        [ForeignKey("questionid")]
        [JsonIgnore]
        public virtual Question question { get; set; }
    }
}

