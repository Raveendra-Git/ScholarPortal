using ScholarPortal.Library;
using ScholarPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScholarPortal.DataLayer
{
    public class HomeExchange
    {
        dbclass _dbobj;
        SqlCommand cmd;
        DataTable dt;
        DataSet ds;
        public async Task<List<CountyNames>> GetCountry()
        {
            List<CountyNames> counties = new List<CountyNames>();
            _dbobj = new dbclass();

            cmd = new SqlCommand("Select id,name from Country order by Name");
            dt = new DataTable();
            dt = await _dbobj.ExcuteDT_CMDAsync(cmd);
            if(dt !=null && dt.Rows.Count > 0)
            {
                counties = dt.AsEnumerable().Select(row => new CountyNames
                {
                    ID=row.Field<int>("id"),
                    CountryName=row.Field<string>("name")
                }).ToList();
            }
            _dbobj = null; dt = null; cmd = null;
            return counties;
        }

        public async Task<List<States>> GetStates(int CountryID)
        {
            List<States> states = new List<States>();
            _dbobj = new dbclass();

            cmd = new SqlCommand("select ID,Name from State where CountryID=@CountryID");
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            dt = new DataTable();
            dt = await _dbobj.ExcuteDT_CMDAsync(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                states = dt.AsEnumerable().Select(row => new States
                {
                    ID = row.Field<int>("ID"),
                    StateName = row.Field<string>("Name")
                }).ToList();
            }

            _dbobj = null; cmd = null; dt = null;
            return states;
        }
        
        public async Task<LoginSession> Logincheck(string UserId="", string Password="")
        {
            //string res = "0";
            LoginSession ls=new LoginSession();
            _dbobj = new dbclass();
            ds = new DataSet();
            try { 
            cmd = new SqlCommand("Proc_LoginSession");
            cmd.Parameters.AddWithValue("@UserID", UserId);
            cmd.Parameters.AddWithValue("@Password", Password);
            ds = await _dbobj.ExcuteDS_SPAsync(cmd);
                if(ds.Tables.Count > 0 && ds != null)
                {
                    dt = ds.Tables[0];
                    if(dt.Rows.Count > 0 && dt != null)
                    {
                        ls.Name = dt.Rows[0]["Name"].ToString();
                        ls.UserType = Convert.ToInt32(dt.Rows[0]["UserType"]);
                        HttpContext.Current.Session["ID"] = dt.Rows[0]["ID"].ToString();
                        HttpContext.Current.Session["LoginID"] = dt.Rows[0]["LoginID"].ToString();
                        HttpContext.Current.Session["Name"] = dt.Rows[0]["Name"].ToString();
                        HttpContext.Current.Session["UserType"] = dt.Rows[0]["UserType"].ToString();
                        //HttpContext.Current.Session["UsesrPicture"] = dt.Rows[0]["Picture"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Exception Iexp = ex.InnerException;
                CustomExchange cexp = new CustomExchange();
                ErrorLogSessionData errorData = cexp.SetSessionVariablesForThread();
                cexp.ErrorLog(ex.Message, Iexp, ex.StackTrace, errorData);
            }
            return ls;
        }
        public async Task<string> SaveRegistryinfo(Registerinfo info)
        {
            string result = "0";
            _dbobj = new dbclass();
            try { 
            cmd = new SqlCommand("Proc_RegisterInfo");
            //cmd.Parameters.AddWithValue("@ID", info.ID);
            cmd.Parameters.AddWithValue("@FirstName", info.FirstName);
            cmd.Parameters.AddWithValue("@LastName", info.LastName);
            cmd.Parameters.AddWithValue("@DOB", info.DOB);
            cmd.Parameters.AddWithValue("@Email", info.Email);
            cmd.Parameters.AddWithValue("@PhoneNo", info.PhoneNo);
            cmd.Parameters.AddWithValue("@Gender", info.Gender);
            cmd.Parameters.AddWithValue("@Language", info.Language);
            cmd.Parameters.AddWithValue("@OtherLanguage", info.OtherLanguage);
            cmd.Parameters.AddWithValue("@Address", info.Address);
            cmd.Parameters.AddWithValue("@Picture", info.Picture);
            //cmd.Parameters.AddWithValue("@UserType", info.UserType);
            cmd.Parameters.AddWithValue("@UserId", info.UserId);
            cmd.Parameters.AddWithValue("@Password", info.Password);
            cmd.Parameters.AddWithValue("@ConfirmPassword", info.ConfirmPassword);
            cmd.Parameters.AddWithValue("@Country", info.Country);
            cmd.Parameters.AddWithValue("@State", info.State);
                if (info.ID > 0)
                    cmd.Parameters.AddWithValue("@InputType", "Update");
                else
                    cmd.Parameters.AddWithValue("@InputType", "Insert");
                result = Convert.ToString(await _dbobj.ExcuteScalerSPAsync(cmd));
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            _dbobj = null; cmd = null;
            return result;
        }
    }
}