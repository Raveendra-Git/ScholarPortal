using ScholarPortal.Library;
using ScholarPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScholarPortal.DataLayer
{
    public class StudentExchange
    {
        dbclass _dbobj;
        SqlCommand cmd;
        DataTable dt;
        DataSet ds;

        public async Task<List<Questionslist>> GetQuestionlists(int QuestionID,int SubjectID)
        {
            List<Questionslist> Questions = new List<Questionslist>();
            _dbobj = new dbclass();
            try
            {
                _dbobj = new dbclass();
                ds = new DataSet();
                cmd = new SqlCommand("Proc_ManageQuestions");
                if(QuestionID >0)
                cmd.Parameters.AddWithValue("@QuestionNum", QuestionID);

                cmd.Parameters.AddWithValue("@SubjectID", SubjectID);
                cmd.Parameters.AddWithValue("@Inputtype", "GetQuestions");
                ds = await _dbobj.ExcuteDS_SPAsync(cmd);
                if(ds.Tables.Count >0 && ds != null)
                {
                    dt = new DataTable();
                    dt = ds.Tables[0];
                    if(dt.Rows.Count >0 && dt != null)
                    {
                        Questions = dt.AsEnumerable().Select(row => new Questionslist
                        {
                            QuestionNum = row.Field<Int64>("QuestionNum"),
                            Question = row.Field<string>("Question"),
                            OptionA = row.Field<string>("OptionA"),
                            OptionB = row.Field<string>("OptionB"),
                            OptionC = row.Field<string>("OptionC"),
                            OptionD = row.Field<string>("OptionD"),
                        }).ToList();
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            cmd = null;
            return Questions;

        }

        public async Task<int> SaveExamQuestion(int Question,int Option)
        {
            int res = 0;
            List<Questionslist> Questions = new List<Questionslist>();
            _dbobj = new dbclass();
            try
            {
                _dbobj = new dbclass();
                ds = new DataSet();
                cmd = new SqlCommand("Proc_ManageQuestions");
                cmd.Parameters.AddWithValue("@ParticipantID", HttpContext.Current.Session["ID"]);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@QuestionNum", Question);
                cmd.Parameters.AddWithValue("@Option", Option);
                cmd.Parameters.AddWithValue("@Inputtype", "SaveExamQues");
                res = await _dbobj.ExcuteNonquerySPAsync(cmd);

            }
            catch (Exception e) { Console.WriteLine(e); }
            cmd = null;
            return res;
        }

        public async Task<string> SubmitTest(int Question, int Option)
        {
            string res = "";
            _dbobj = new dbclass();
            dt = new DataTable();
            cmd = new SqlCommand("Proc_ManageQuestions");
            cmd.Parameters.AddWithValue("@ParticipantID", HttpContext.Current.Session["ID"]);
            cmd.Parameters.AddWithValue("@QuestionNum", Question);
            cmd.Parameters.AddWithValue("@Option", Option);
            cmd.Parameters.AddWithValue("@Inputtype", "GetScore");
            res = await _dbobj.ExcuteScalerSPAsync(cmd);

            _dbobj = null;  dt = null;  cmd = null;
              return res;
        }

    }
}