using System;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Xml.Linq;

namespace ProjectQuiz.Models
{
	public class Question
    {
        public int id { get; set; }
        public string content { get; set; }
        public int categoryid { get; set; }
        public int status { get; set; }

    }

    /*
    public class QuestionEqualityComparer : IEqualityComparer<Question>
    {
        public bool Equals(Question x, Question y)
        {
            if (x == null || y == null)
                return false;

            return (x.id == y.id && x.content == y.content && x.categoryid == y.categoryid);
        }

        public int GetHashCode(Question q)
        {
            return q.GetHashCode();
        }
    }*/
}


