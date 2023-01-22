using System;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectQuiz.DBEntities
{
    [Table("Question")]
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string content { get; set; }
        public int categoryid { get; set; }
        public int status { get; set; }

        [InverseProperty("Question")]
        public virtual List<Option> options { get; set; }

        //[InverseProperty("Question")]
        //public List<Quiz_Question> quiz_Questions { get; set; }
    }

    
}


