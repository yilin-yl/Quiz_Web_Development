using System;
using Microsoft.AspNetCore.Mvc;

namespace ProjectQuiz.DTO
{
    
    public class Exam
	{
        [BindProperty]
        public List<FullQuestion> fullQuestion { get; set; }

        //public DateTime starttime { get; set; }
    }
}

