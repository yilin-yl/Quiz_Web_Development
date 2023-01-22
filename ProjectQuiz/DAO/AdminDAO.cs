using System;
using ProjectQuiz.Data;
using ProjectQuiz.DBEntities;
using Microsoft.EntityFrameworkCore;

namespace ProjectQuiz.DAO
{
	public class AdminDAO
	{
		private readonly AdminContext _dbContext;

		public AdminDAO(AdminContext dbContext)
		{
			_dbContext = dbContext;
		}

		//get all questionSubmission & user
		public List<QuizSubmission> GetAllSubmission()
		{
            var res = _dbContext.QuizSubmissions
				.Include(x => x.user)
				.ToList();
			return res;
		}

		//get all quiz_question & question
		public List<Quiz_Question> GetAllQQ()
		{
			var res = _dbContext.Quiz_Questions
				.ToList();
			return res;
		}

		//construct dictionary of {qid: category}
		public Dictionary<int,string> AllCategoryByQId()
		{
			var category_dict = new Dictionary<int, string>()
			{
				{1,"C#"},
				{3,"SQL"},
				{4,"Python"}
			};
			var res = new Dictionary<int, string>();

            var all_q = _dbContext.Questions.ToList();
			foreach (var q in all_q)
			{
				res.Add(q.id, category_dict[q.categoryid]);
			}

			return res;
		}

		//get quiz_question & question & options by quizid
		public List<Quiz_Question> GetQuizDetailByQuizId(int quizid)
		{
			var res = _dbContext.Quiz_Questions
				.Where(qq => qq.quizid == quizid)
				.Include(x => x.question)
				.ThenInclude(y => y.options)
				.ToList();
			return res;
		}

		//get all user profile
		public List<UserInfo> GetAllUser()
		{
			var res = _dbContext.UserInfos.ToList();
			return res;
		}

		//update status
		public void UpdateStatus(int userid)
		{
			var user_res = _dbContext.UserInfos.SingleOrDefault(u => u.id == userid);
			if (user_res != null)
			{
                int old_satus = user_res.status;
                user_res.status = 1 - old_satus;
				_dbContext.SaveChanges();
			}
		}

		//get all questions with options
		public List<Question> GetAllQuestion()
		{
			var res = _dbContext.Questions.Include(q => q.options).ToList();
			return res;
		}

		//insert new question & options
		public void CreateQuestion(Question question)
		{
			_dbContext.Add(question);
			_dbContext.SaveChanges();
			
		}

		//get question by id
		public Question GetQuestionById(int qid)
		{
			var res = _dbContext.Questions
				.Include(q => q.options)
				.SingleOrDefault(q => q.id == qid);
            return res;
        }

		//update question
		public void UpdateQuestion(Question question)
		{
			var result = _dbContext.Questions
				.Include(q => q.options)
				.SingleOrDefault(q => q.id == question.id);
			if (result != null)
			{
                result.content = question.content;

                for (int i=0;i<3;i++)
				{
					result.options[i].value = question.options[i].value;
                    result.options[i].iscorrect = question.options[i].iscorrect;

                }
                _dbContext.SaveChanges();
            }
			
		}
	}
}

