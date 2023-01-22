using System;
using ProjectQuiz.DBEntities;
namespace ProjectQuiz.DTO
{
	public class AdminDetail
	{
        public int? no { get; set; }
        public Question question { get; set; }
        public List<Option> options { get; set; }
        public int? selected_oid { get; set; }
        public int iscorrect { get; set; }
    }
}

