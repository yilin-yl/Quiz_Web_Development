using System;
using ProjectQuiz.Models;
using ProjectQuiz.DTO;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace ProjectQuiz.DAO
{
	public class QuizDAO
	{
        private readonly IConfiguration _configuration;
        private Random random = new Random();

        public QuizDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //insert into Quiz table and get quizid back 
        public int AddQuiz()
        {
            string query_insert = "INSERT INTO Quiz VALUES ('Quiz','MC') SELECT @@IDENTITY";
            int quizid;
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(query_insert, conn);
                //cmd.Parameters.AddWithValue("@Name", category.Name);
                quizid = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return quizid;  

        }

        //insert into Quiz_Question table
        public int AddQuizQuestion(Quiz_Question qq)
        {
            string query_insert = "INSERT INTO Quiz_Question VALUES (@quizid, @questionid, @selected_optionid, @question_no)";
            //int quizid = AddQuiz();
            int rowaffected;

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(query_insert, conn);
                cmd.Parameters.AddWithValue("@questionid", qq.questionid);
                cmd.Parameters.AddWithValue("@quizid", qq.quizid);
                cmd.Parameters.AddWithValue("@question_no", qq.question_no);
                if (qq.selected_optionid != null)
                {
                    cmd.Parameters.AddWithValue("@selected_optionid", qq.selected_optionid);
                } else {
                    cmd.Parameters.AddWithValue("@selected_optionid", DBNull.Value);
                }
                
                rowaffected = cmd.ExecuteNonQuery();
            }
            return rowaffected;

        }

        //insert into QuizSubmission table -> starttime, userid, quizid
        public int AddSubmission(QuizSubmission submission)
        {
            string query_insert = "INSERT INTO QuizSubmission (starttime, quizid, userid) VALUES (@starttime, @quizid, @userid)";
            int rowaffected;

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(query_insert, conn);
                cmd.Parameters.AddWithValue("@starttime", submission.starttime);
                //cmd.Parameters.AddWithValue("@endtime", submission.endtime);
                //cmd.Parameters.AddWithValue("@score", submission.score);
                cmd.Parameters.AddWithValue("@userid", submission.userid);
                cmd.Parameters.AddWithValue("@quizid", submission.quizid);
                rowaffected = cmd.ExecuteNonQuery();
            }
            return rowaffected;

        }

        //update QuizSubmission table based on quizid -> endtime
        public int UpdateSubmission(DateTime endtime, int quizid)
        {
            string query_insert = "UPDATE QuizSubmission SET endtime = @endtime WHERE quizid = @quizid";
            int rowaffected;

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(query_insert, conn);
                cmd.Parameters.AddWithValue("@endtime", endtime);
                cmd.Parameters.AddWithValue("@quizid", quizid);
                rowaffected = cmd.ExecuteNonQuery();
            }
            return rowaffected;

        }

        //update QuizSubmission table based on quizid -> score 
        public int UpdateSubmission_Score(int score, int quizid)
        {
            string query_insert = "UPDATE QuizSubmission SET score = @score WHERE quizid = @quizid";
            int rowaffected;

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(query_insert, conn);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.Parameters.AddWithValue("@quizid", quizid);
                rowaffected = cmd.ExecuteNonQuery();
            }
            return rowaffected;

        }


        //get a list of question & options (answer) objects 
        public List<FullQuestion> GetQuiz(int categoryid)
        {
            string query = "SELECT q.*, o.* FROM Question q JOIN Options o ON q.id=o.questionid WHERE q.categoryid=@categoryid";
            //var all_question = new List<Question>();
            var dict = new Dictionary<int,List<Option>>();
            var all_qid = new HashSet<int>(); //get a set of all question id -> for random select
            var qid_rand_set = new HashSet<int>(); //store 10 unique qid
            var res = new List<FullQuestion>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@categoryid", categoryid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            /*Question q = new Question()
                            {
                                id = Convert.ToInt32(reader[0]),
                                content = Convert.ToString(reader["content"]),
                                categoryid = Convert.ToInt32(reader["categoryid"])
                            };*/
                            int qid = Convert.ToInt32(reader[0]);
                            Option o = new Option()
                            {
                                id = Convert.ToInt32(reader[3]),
                                value = Convert.ToString(reader["value"]),
                                iscorrect = Convert.ToInt32(reader["iscorrect"]),
                                questionid = Convert.ToInt32(reader["questionid"])
                            };
                            all_qid.Add(qid);

                            if (dict.ContainsKey(qid))
                            {
                                dict[qid].Add(o);
                            } else
                            {
                                var list = new List<Option>();
                                list.Add(o);
                                dict.Add(qid, list);
                            }    
                        }
                    }
                }
            }

            // random select 10
            int max = all_qid.MaxBy(x => x);
            while (qid_rand_set.Count < 10)
            {
                var qid = random.Next(0, max);
                if (all_qid.Contains(qid)) qid_rand_set.Add(qid);
            }

            int i = 1; //question no.
            string query_allq = "SELECT * FROM Question";
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                var cmd = new SqlCommand(query_allq, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var qid = Convert.ToInt32(reader["id"]);
                        if (qid_rand_set.Contains(qid))
                        {
                            FullQuestion fq = new FullQuestion();
                            fq.no = i;
                            fq.question = new Question()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                content = Convert.ToString(reader["content"]),
                                categoryid = Convert.ToInt32(reader["categoryid"])
                            }; ;
                            fq.option1 = dict[qid][0];
                            fq.option2 = dict[qid][1];
                            fq.option3 = dict[qid][2];
                            res.Add(fq);
                            i++;
                        }
                           
                            
                    }
                }
            }
            return res;
        }


        //get Quiz_Question given quizid
        public List<Quiz_Question> RetrieveQQ(int quizid)
        {
            string query = "SELECT * FROM Quiz_Question WHERE quizid=@quizid";
            var res = new List<Quiz_Question>();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@quizid", quizid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            res.Add(
                                new Quiz_Question
                            {
                                id = Convert.ToInt32(reader[0]),
                                quizid = Convert.ToInt32(reader[1]),
                                questionid = Convert.ToInt32(reader[2]),
                                question_no = Convert.ToInt32(reader[4]),
                                selected_optionid = (reader[3] != DBNull.Value) ? Convert.ToInt32(reader[3]): null
                        });
                        }
                    }
                }
            }
            return res;
        }

        //get a unique submission given quizid
        public QuizSubmission RetrieveSubmission(int quizid)
        {
            string query = "SELECT * FROM QuizSubmission WHERE quizid=@quizid";
            var res = new QuizSubmission();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@quizid", quizid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            return new QuizSubmission
                            {
                                id = Convert.ToInt32(reader[0]),
                                starttime = Convert.ToDateTime(reader["starttime"]),
                                endtime = Convert.ToDateTime(reader["endtime"]),
                                score = Convert.ToInt32(reader["score"]),
                                userid = Convert.ToInt32(reader["userid"]),
                                quizid = Convert.ToInt32(reader["quizid"])
                            };
                        }
                    }
                }
            }
            return res;
        }

        //get a unique user given userid
        public UserInfo RetrieveUserInfo(int userid)
        {
            string query = "SELECT * FROM UserInfo WHERE id=@userid";
            var res = new UserInfo();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@userid", userid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            return new UserInfo
                            {
                                id = Convert.ToInt32(reader[0]),
                                username = Convert.ToString(reader["username"]),
                                password = Convert.ToString(reader["password"]),
                                firstname = Convert.ToString(reader["firstname"]),
                                lastname = Convert.ToString(reader["lastname"])
                                
                            };
                        }
                    }
                }
            }
            return res;
        }

        //get a unique quiz given quizid
        public Quiz RetrieveQuiz(int quizid)
        {
            string query = "SELECT * FROM Quiz WHERE id=@quizid";
            var res = new Quiz();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@quizid", quizid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            return new Quiz
                            {
                                id = Convert.ToInt32(reader[0]),
                                quizname = Convert.ToString(reader["quizname"]),
                                quiztype = Convert.ToString(reader["quiztype"])

                            };
                        }
                    }
                }
            }
            return res;
        }

        //get Option correct dict 
        public Dictionary<int, int> RetrieveAllOption_Dict()
        {
            string query = "SELECT * FROM Options";
            var res = new Dictionary<int,int>();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader[0]);
                            int iscorrect = Convert.ToInt32(reader["iscorrect"]);
                            res.Add(id,iscorrect);
                        }
                    }
                }
            }
            return res;
        }

        public List<Option> RetrieveAllOption()
        {
            string query = "SELECT * FROM Options";
            var res = new List<Option>();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            res.Add(
                                new Option
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    value = Convert.ToString(reader["value"]),
                                    iscorrect = Convert.ToInt32(reader["iscorrect"]),
                                    questionid = Convert.ToInt32(reader["questionid"])
                                }
                                );
                        }
                    }
                }
            }
            return res;
        }

        public List<Question> RetrieveAllQuestion()
        {
            string query = "SELECT * FROM Question";
            var res = new List<Question>();
            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            res.Add(
                                new Question
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    content = Convert.ToString(reader["content"]),
                                    categoryid = Convert.ToInt32(reader["categoryid"])
                                }
                                );
                        }
                    }
                }
            }
            return res;
        }
        /*//Get question from random selection
        public List<Question> GetQuestion(int categoryid)
        {
            string query = "SELECT * FROM Question WHERE categoryid=@categoryid";
            var all_question = new List<Question>();
            var all_qid = new List<int>(); //get a list of all question id -> for random select
            var idx_set = new HashSet<int>(); //store 10 unique index
            var res = new List<Question>();

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@categoryid", categoryid);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows) //check if the reader is empty or not
                    {
                        while (reader.Read())
                        {
                            all_qid.Add(Convert.ToInt32(reader[0]));
                            all_question.Add(
                                new Question()
                                {
                                    id = Convert.ToInt32(reader[0]),
                                    content = Convert.ToString(reader["content"]),
                                    categoryid = Convert.ToInt32(reader["categoryid"])
                                });
                        }
                    }
                }
            }

            // random select 10
            int size = all_qid.Count;
            while (idx_set.Count < 10)
            {
                idx_set.Add(random.Next(0, size));
            }

            int i = 1; //question no.
            foreach (int idx in idx_set)
            {
                Question q = all_question[idx];
                i++;
            }

            return res;
        }


        //get a list of corresponding options
        public List<Option> GetOption(List<Question> questions)
        {
            var res = new List<Option>();

            string query_option = "SELECT * FROM Options WHERE questionid IN ({0})";
            string[] paramNames = questions.Select((q, i) => (q.id, i)).Select((qid, i) => "@questionid" + i.ToString()).ToArray();
            string inClause = string.Join(",", paramNames);

            using (var conn = new SqlConnection(_configuration.GetConnectionString("default")))
            {
                conn.Open();
                using (var cmd = new SqlCommand(string.Format(query_option, inClause), conn))
                {
                    for (int i = 0; i < paramNames.Length; i++)
                    {
                        cmd.Parameters.AddWithValue(paramNames[i], questions[i].id);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows) //check if the reader is empty or not
                        {
                            while (reader.Read())
                            {
                                res.Add(
                                    new Option()//value
                                    {
                                        id = Convert.ToInt32(reader["id"]),
                                        value = Convert.ToString(reader["value"]),
                                        iscorrect = Convert.ToInt32(reader["iscorrect"]),
                                        questionid = Convert.ToInt32(reader["quesionid"])
                                    });
                            }
                        }
                    }
                }
            }
            return res;
        }


        //get a list of question & options (answer) objects
        /*public List<FullQuestion> GetQuiz(int categoryid)
        {
            var res = new List<FullQuestion>();
            int i = 1;
            var all_q = this.GetQuestion(categoryid);
            var all_o = this.GetOption(all_q);

            foreach (var item in all_q)
            {
                var options = all_o.Where(x => x.questionid == item.id).Select(x => x).ToArray();
                res.Add(
                    new FullQuestion()
                    {
                        no = i,
                        question = item,
                        option1 = options[0],
                        option2 = options[1],
                        option3 = options[2]
                    });
            }
            return res;
        }*/
    }
}

