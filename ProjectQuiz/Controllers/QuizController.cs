using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectQuiz.DAO;
using ProjectQuiz.DTO;
using ProjectQuiz.Models;

namespace ProjectQuiz.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly QuizDAO _quizdao;
        public QuizController(QuizDAO quizDAO)
        {
            _quizdao = quizDAO;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Quiz/GetQuiz/{categoryid}")]
        //[HttpGet]
        public IActionResult GetQuiz(int categoryid)
        {
            int quizid = _quizdao.AddQuiz();
            Exam exam = new Exam();
            var list_fq = _quizdao.GetQuiz(categoryid);
            foreach (var item in list_fq)
            {
                item.quizid = quizid;
            }
            exam.fullQuestion = list_fq;
            var submission = new QuizSubmission();
            submission.quizid = quizid;
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            submission.userid = Convert.ToInt32(userid.Value);
            submission.starttime = DateTime.Now; 
            //submission.endtime = endtime;
            _quizdao.AddSubmission(submission);

            return View(exam);
        }

        [HttpPost]
        public IActionResult GetQuiz(Exam exam)
        {
            var quizid = exam.fullQuestion[0].quizid;
            var endtime = DateTime.Now;
            
            foreach (var item in exam.fullQuestion)
            {
                var qq = new Quiz_Question();

                var test = item.no;

                qq.quizid = quizid;
                qq.questionid = item.question.id;
                qq.selected_optionid = item.selected_oid;
                qq.question_no = item.no;
                
                _quizdao.AddQuizQuestion(qq);
            }

            _quizdao.UpdateSubmission(endtime, (int)quizid);


            return RedirectToAction("GetResult", new { quizid = quizid });
        }


        // show result 
        [HttpGet]
        public IActionResult GetResult(int quizid)
        {
            var question_dict = new Dictionary<int, Question>();
            var option_dict = new Dictionary<int, Option>();
            var q_to_o_dict = new Dictionary<int, List<Option>>();
            //var submission = new QuizSubmission();
            var model = new Result();
            int score = 0;
            DateTime dt = DateTime.Now;

            var list = _quizdao.RetrieveQQ(quizid);
            var all_option = _quizdao.RetrieveAllOption();
            var all_question = _quizdao.RetrieveAllQuestion();

            //construct question dict
            foreach (var q in all_question)
            {
                question_dict.Add(q.id, q);
            }

            //construct option dict
            foreach (var item in all_option)
            {
                option_dict.Add(item.id, item);
                if (q_to_o_dict.ContainsKey(item.questionid))
                {
                    q_to_o_dict[item.questionid].Add(item);
                } else
                {
                    var lst = new List<Option>();
                    lst.Add(item);
                    q_to_o_dict.Add(item.questionid, lst);
                }
                    
            }

            foreach (var item in list)
            {
                if (item.selected_optionid != null && option_dict[(int)item.selected_optionid].iscorrect == 1)
                {
                    score++;
                }
            }
            _quizdao.UpdateSubmission_Score(score, quizid);
            QuizSubmission qs_get = _quizdao.RetrieveSubmission(quizid);

            // do with Result object -> model
            Quiz quiz = _quizdao.RetrieveQuiz(quizid);
            var userid = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //var userid_type = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            //submission.userid = Convert.ToInt32(userid.Value);
            UserInfo u = _quizdao.RetrieveUserInfo(userid);
            model.firstname = u.firstname;
            model.lastname = u.lastname;
            model.score = score;
            model.starttime = qs_get.starttime;
            model.endtime = qs_get.endtime;
            model.quizname = quiz.quizname;
            model.question_submissions = new List<QuestionSubmission>();
            foreach (var item in list)
            {
                var qs = new QuestionSubmission
                {
                    no = item.question_no,
                    question = question_dict[item.questionid],
                    option1 = q_to_o_dict[item.questionid][0],
                    option2 = q_to_o_dict[item.questionid][1],
                    option3 = q_to_o_dict[item.questionid][2],
                    selected_oid = item.selected_optionid
                };

                if (qs.selected_oid != null && option_dict[(int)item.selected_optionid].iscorrect == 1)
                {
                    qs.iscorrect = 1;
                }
                else { qs.iscorrect = 0; }

                model.question_submissions.Add(qs);
            }

            return View(model);
        }

    }
}

