using System;
using ProjectQuiz.DAO;
using ProjectQuiz.DTO;
using ProjectQuiz.DBEntities;

namespace ProjectQuiz.Services
{
	public class AdminService
	{
		private readonly AdminDAO _adminDAO;

		public AdminService(AdminDAO adminDAO)
		{
			_adminDAO = adminDAO;
		}

		//get result summary
		public List<AdminResultSummary> GetAllSummary()
		{
			var all_submission = _adminDAO.GetAllSubmission();
			var all_qq = _adminDAO.GetAllQQ();
			var dict_q_cat = _adminDAO.AllCategoryByQId();//questionid:categoryname

			var dict_quiz_q = new Dictionary<int, List<Quiz_Question>>();
			var res = new List<AdminResultSummary>();

			//construct quiz_question {quizid: List<qq>}
			foreach (var item in all_qq)
			{
				if (dict_quiz_q.ContainsKey(item.quizid))
				{
					dict_quiz_q[item.quizid].Add(item);
				}
				else
				{
					var newlist = new List<Quiz_Question>();
					newlist.Add(item);
					dict_quiz_q.Add(item.quizid, newlist);
				}
			}

			foreach (var item in all_submission)
			{
				if (!dict_quiz_q.ContainsKey(item.quizid)) continue;

				AdminResultSummary record = new AdminResultSummary();
				int quizid = item.quizid;
				record.quizid = quizid;
                record.categoryname = dict_q_cat[dict_quiz_q[quizid][0].questionid];
				record.starttime = item.starttime;
				record.endtime = item.endtime;
				record.username = item.user.username;
				record.firstname = item.user.firstname;
				record.lastname = item.user.lastname;
				record.score = item.score;
				int cnt = 0;
				foreach(var qq in dict_quiz_q[quizid])
				{
					if (qq.selected_optionid != null) cnt++;
				}

				record.completion = cnt;
				res.Add(record);
			}

			return res.OrderByDescending(x => x.starttime).ToList();

        }


		//get result detail by quizid
		public List<AdminDetail> GetQuizDetail(int quizid)
		{
			var quiz_details = _adminDAO.GetQuizDetailByQuizId(quizid);
			var res = new List<AdminDetail>();

			foreach (var item in quiz_details)
			{
				var record = new AdminDetail();
				record.no = item.question_no;
				record.question = item.question;
				record.options = new List<Option>();
				record.iscorrect = 0;
                record.selected_oid = item.selected_optionid;
                foreach (var option in item.question.options)
				{
					record.options.Add(option);
					if (record.selected_oid != null &&
						option.id == record.selected_oid &&
						option.iscorrect == 1)
					{
						record.iscorrect = 1;
					}
				}
				res.Add(record);
			}

			return res;
		} 


	}
}

