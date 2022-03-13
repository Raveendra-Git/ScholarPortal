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
    public class StaffExchange
    {
        dbclass _dbobj;
        SqlCommand cmd;
        DataTable dt;
        DataSet ds;

        public async Task<int> SaveQuestions(QuestionObj obj)
        {
            int res = 0;
            try
            {
                _dbobj = new dbclass();
                cmd = new SqlCommand("Proc_ManageQuestions");
                cmd.Parameters.AddWithValue("@Question", obj.Question);
                cmd.Parameters.AddWithValue("@OptionA", obj.OptionA);
                cmd.Parameters.AddWithValue("@OptionB", obj.OptionB);
                cmd.Parameters.AddWithValue("@OptionC", obj.OptionC);
                cmd.Parameters.AddWithValue("@OptionD", obj.OptionD);
                cmd.Parameters.AddWithValue("@CorrectAns", obj.CorrectAns);
                cmd.Parameters.AddWithValue("@StaffID", obj.StaffID);
                cmd.Parameters.AddWithValue("@SubjectID", obj.SubjectID);
                cmd.Parameters.AddWithValue("@LastModifiedBy", HttpContext.Current.Session["ID"]);
                cmd.Parameters.AddWithValue("@Inputtype", "Insert");
                res = await _dbobj.ExcuteNonquerySPAsync(cmd);

            }
            catch (Exception e) { Console.WriteLine(e); }
            cmd = null;
            return res;
        }
    }
}