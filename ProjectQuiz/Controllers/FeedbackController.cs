using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectQuiz.DAO;
using ProjectQuiz.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

//AddFeedback action 
namespace ProjectQuiz.Controllers
{
	public class FeedbackController: Controller
	{
        private readonly FeedbackDAO _feedbackdao;
        public FeedbackController(FeedbackDAO feedbackDAO)
		{
            _feedbackdao = feedbackDAO;
		}

        [Authorize]
        [HttpGet]
        public IActionResult AddFeedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFeedback(Feedback feedback)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            feedback.userid = Convert.ToInt32(userid.Value);
            var rowaffected = _feedbackdao.AddFeedback(feedback);
            if (rowaffected != 0) { return Redirect("/Home/Index"); }
            else { return View();  }
            
        }

    }
}

