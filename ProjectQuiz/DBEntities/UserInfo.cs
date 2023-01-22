using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectQuiz.DBEntities
{
    [Table("UserInfo")]
    public class UserInfo
	{
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int status { get; set; }
        public string? address { get; set; }
        public string? email { get; set; }
    }
}

