using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectQuiz.DAO;
using ProjectQuiz.Services;
using ProjectQuiz.DBEntities;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectQuiz.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly AdminDAO _adminDAO;
        private readonly AdminService _adminService;

        public AdminController(AdminDAO admindao, AdminService adminService)
        {
            _adminDAO = admindao;
            _adminService = adminService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/result")]
        public ActionResult GetAllSummary()
        {
            var res = _adminService.GetAllSummary();
            return View(res);
        }

        [HttpGet("admin/result/{quizid}")]
        public ActionResult GetDetailByQuizId(int quizid)
        {
            var res = _adminService.GetQuizDetail(quizid);
            return View(res);
        }


        // user profile
        [HttpGet("admin/profile")]
        public ActionResult GetAllUser()
        {
            var res = _adminDAO.GetAllUser();
            return View(res);
        }

        // activate/suspend user 
        [HttpGet]
        public IActionResult UpdateStatus(int userid)
        {
            _adminDAO.UpdateStatus(userid);
            //return Ok(_adminDAO.GetAllUser());
            return RedirectToAction("GetAllUser");
        }

        // view all questions
        [HttpGet]
        public IActionResult GetAllQuestion()
        {
            var res = _adminDAO.GetAllQuestion();
            return View(res);
        }

        // update
        [HttpGet]
        public IActionResult UpdateQuestion(int qid)
        {
            var model = _adminDAO.GetQuestionById(qid);
            //if (model == null)
            //    return NotFound();
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateQuestion(Question question)
        {
            _adminDAO.UpdateQuestion(question);
            return RedirectToAction("GetAllQuestion");
        }

        // add question
        [HttpGet]
        public IActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQuestion(Question question)
        {
            _adminDAO.CreateQuestion(question);
            return RedirectToAction("GetAllQuestion"); ;
        }

        // change question status
        [HttpGet]
        public IActionResult UpdateQuestionStatus(int id)
        {
            _adminDAO.UpdateQuestionStatus(id);
            return RedirectToAction("GetAllQuestion");
        }
    }
}

