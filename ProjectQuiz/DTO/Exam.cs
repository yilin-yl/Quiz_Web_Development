using System;
using Microsoft.AspNetCore.Mvc;

namespace ProjectQuiz.DTO
{
    
    public class Exam
	{
        [BindProperty]
        public List<FullQuestion> fullQuestion { get; set; }

        //public int CurrentPageIndex { get; set; }

        //public int PageCount { get; set; }

    }
}

