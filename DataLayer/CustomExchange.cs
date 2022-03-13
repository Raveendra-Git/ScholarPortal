using ScholarPortal.Library;
using ScholarPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ScholarPortal.DataLayer
{
    public class CustomExchange
    {
        dbclass _dbobj;
        SqlCommand cmd;
        //DataTable dt;
        //DataSet ds;

        //Check for folder with Provider's ID and if it not exists then create folder.
        public string CheckParticipantLogoDirectory(string ParticipantID = "")
    {
        if (ParticipantID == "")
            //ParticipantID = HttpContext.Current.Session["ProviderID"].ToString();
            ParticipantID = "1";

        //Check for folder with Provider's ID and if it not exists then create folder.
        string DirectoryPath = AppDomain.CurrentDomain.BaseDirectory + @"Images\ParticipantImg\" + ParticipantID + @"\";
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }
        return DirectoryPath;
    }

        #region "Error Handling"
        public ErrorLogSessionData SetSessionVariablesForThread()
        {
            ErrorLogSessionData errorData = new ErrorLogSessionData();
            if (HttpContext.Current.Session["loginid"] != null && HttpContext.Current.Session["loginid"].ToString() != "")
                errorData.LoginID = Convert.ToInt32(HttpContext.Current.Session["loginid"]);
            else
                errorData.LoginID = 0;
            errorData.PageUrl = HttpContext.Current.Request.RawUrl;
            errorData.UserIP = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR").ToString();
            errorData.Browser = HttpContext.Current.Request.Browser.Browser;
            return errorData;
        }

        //Write Error message into Errorlog
        public int ErrorLog(string message, Exception exception, string stacktrace, ErrorLogSessionData errorLogSessionData = null, string modulename = "")
        {
            int result = 0;
            decimal loginid = 0;
            string pageurl = "";
            string userip = "";
            string browser = "";

            try
            {
                if (HttpContext.Current == null && errorLogSessionData != null)
                {
                    loginid = errorLogSessionData.LoginID;
                    pageurl = errorLogSessionData.PageUrl;
                    userip = errorLogSessionData.UserIP;
                    browser = errorLogSessionData.Browser;
                }
                else
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["loginid"] != null && HttpContext.Current.Session["loginid"].ToString() != "")
                            loginid = Convert.ToDecimal(HttpContext.Current.Session["loginid"]);
                        else
                            loginid = 0;
                    }
                    else
                        loginid = 0;
                    if (HttpContext.Current != null)
                    {
                        pageurl = HttpContext.Current.Request.RawUrl;
                        userip = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR").ToString();
                        browser = HttpContext.Current.Request.Browser.Browser;
                    }
                }

                string innerexp = "";
                if (exception != null)
                    innerexp = exception.Message;

                _dbobj = new dbclass();
                cmd = new SqlCommand("Insert into ErrorLog (GeneratedOn,Message,PageURl,InnerException,StackTrace,UserIPAddress,LoginID,BrowserAgent,Module) values (dbo.FngetCurrentDatetime(),@Message,@PageURl,@InnerException,@StackTrace,@UserIPAddress,@LoginID,@BrowserAgent,@Module)");
                cmd.Parameters.AddWithValue("@Message", message);
                cmd.Parameters.AddWithValue("@PageURl", pageurl);
                cmd.Parameters.AddWithValue("@InnerException", innerexp);
                cmd.Parameters.AddWithValue("@StackTrace", stacktrace);
                cmd.Parameters.AddWithValue("@UserIPAddress", userip);
                cmd.Parameters.AddWithValue("@LoginID", loginid);
                cmd.Parameters.AddWithValue("@BrowserAgent", browser);
                cmd.Parameters.AddWithValue("@Module", modulename);
                result = _dbobj.ExcuteNonqueryCMD(cmd);

                #region "=============Log File of Errors============================="

                var line = Environment.NewLine;
                string filepath = AppDomain.CurrentDomain.BaseDirectory + @"Logs\";
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                filepath = filepath + "Errorlogs" + "-" + DateTime.Today.ToString("dd-MM-yy") + ".txt"; //Text file Name
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();
                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine("--------------------Log details on " + "" + DateTime.Now.ToString() + "---------------------------");
                    sw.WriteLine("-----------------------------*Start*----------------------------------");
                    sw.WriteLine("ParticipantID:-" + loginid);
                    sw.WriteLine("DateTime:-" + DateTime.UtcNow);
                    sw.WriteLine("Message:-" + message);
                    sw.WriteLine("pageURL:-" + pageurl);                    
                    sw.WriteLine("-----------------------------*End*------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();
                }

                #endregion "=============Log File of Errors============================="
            }
            catch (Exception) { }
            finally { _dbobj = null; cmd = null; }
            return result;
        }

        #endregion
    }
}