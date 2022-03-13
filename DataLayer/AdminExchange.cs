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
    public class AdminExchange
    {
        dbclass _dbobj;
        SqlCommand cmd;
        DataTable dt;
        DataSet ds;

        public async Task<List<UserDetails>> ManageUserDetails(string pagesize, string pageno, string UserID, string Gender, string Name, string Address)
        {
            List<UserDetails> userDetails = new List<UserDetails>();
            _dbobj = new dbclass();
            try
            {
                cmd = new SqlCommand("Proc_ManageUsers");
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.Parameters.AddWithValue("@PageNum", pageno);
                //cmd.Parameters.AddWithValue("@UserID", UserID);
                //cmd.Parameters.AddWithValue("@Gender", Gender);
                //cmd.Parameters.AddWithValue("@Name", Name);
                //cmd.Parameters.AddWithValue("@Address", Address);
                cmd.Parameters.AddWithValue("@Inputtype", "Manage");

                dt = new DataTable();
                dt = await _dbobj.ExcuteDT_SPAsync(cmd);
                if (dt != null && dt.Rows.Count > 0)
                {
                    userDetails = dt.AsEnumerable().Select(row => new UserDetails
                    {
                        ID = row.Field<Int64>("ID"),
                        Name = row.Field<string>("Name"),
                        Address = row.Field<string>("Address"),
                        DOB = row.Field<string>("DOB"),
                        Age = row.Field<Int32>("Age"),
                        Email = row.Field<string>("Email"),
                        PhoneNo = row.Field<Int64>("PhoneNo"),
                        Country = row.Field<string>("Country"),
                        State = row.Field<string>("State"),
                        Gender = row.Field<string>("Gender"),
                        Picture = row.Field<string>("Picture"),
                        LastModifiedDate = row.Field<string>("LastModifiedDate"),
                        RowNum = row.Field<Int64>("RowNum"),
                        RowCnt = row.Field<Int32>("RowCnt")
                    }).ToList();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            _dbobj = null; cmd = null; dt = null;
            return userDetails;
        }

        //Delete UserInfo
        public async Task<int> DeleteUserInfo(string ID)
        {
            int result = 0;
            _dbobj = new dbclass();
            try
            {
                cmd = new SqlCommand("Proc_ManageUsers");
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Inputtype", "Delete");
                result = await _dbobj.ExcuteNonquerySPAsync(cmd);
            }
            catch (Exception e) { Console.WriteLine(e); }
            _dbobj = null; cmd = null; dt = null;
            return result;
        }

        //Get Equipment Details
        public async Task<List<UsersReport>> GetUsersDetailDownload()
        {
            List<UsersReport> userslist = new List<UsersReport>();
            _dbobj = new dbclass();
            cmd = new SqlCommand("proc_GetUsersDetailList");
            cmd.Parameters.AddWithValue("@InputType", "AllUsers");
            ds = new DataSet();
            ds = await _dbobj.ExcuteDS_SPAsync(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    userslist = dt.AsEnumerable().Select(row => new UsersReport
                    {
                        ID = row.Field<Int64>("ID"),
                        Name = row.Field<string>("Name") ?? "",
                        Address = row.Field<string>("Address") ?? "",
                        DOB = row.Field<string>("DOB") ?? "",
                        Email = row.Field<string>("Email"),
                        PhoneNo = row.Field<Int64>("PhoneNo"),
                        Country = row.Field<string>("Country") ?? "",
                        State = row.Field<string>("State") ?? "",
                        Gender = row.Field<string>("Gender") ?? "",
                        Age = row.Field<Int32>("Age")

                    }).ToList();
                }
            }
            _dbobj = null; cmd = null; ds = null; dt = null;
            return await Task.Run(() => { return userslist; });
        }

        #region "Add Subject"
        public async Task<int> SaveSubject(string SubID, string SubName)
        {
            int res = 0;
            _dbobj = new dbclass();
            cmd = new SqlCommand("Insert into Subjects values(@SubID,@SubName)");
            cmd.Parameters.AddWithValue("@SubID", SubID);
            cmd.Parameters.AddWithValue("@SubName", SubName);
            res = await _dbobj.ExcuteNonqueryCMDAsync(cmd);
            cmd = null;

            return res;
        }

        public async Task<List<SubjectList>> GetSubjectList(string pagesize, string pageno)
        {
            List<SubjectList> subjects = new List<SubjectList>();
            _dbobj = new dbclass();
            try { 
            cmd = new SqlCommand("Proc_ManageSubjects");
            cmd.Parameters.AddWithValue("@PageSize", pagesize);
            cmd.Parameters.AddWithValue("@PageNum", pageno);
            cmd.Parameters.AddWithValue("@Inputtype", "Manage");
            ds = new DataSet();
            ds = await _dbobj.ExcuteDS_SPAsync(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SubjectList subject = new SubjectList();
                        subject.ID = Convert.ToInt64(dr["ID"]);
                        subject.RowNum = Convert.ToInt64(dr["RowNum"]);
                        subject.SubjectCode = dr["SubjectCode"].ToString();
                        subject.SubName = dr["SubjectName"].ToString();
                        subject.RowCnt = Convert.ToInt32(dr["RowCnt"]);
                        subjects.Add(subject);
                    }
                }
            }
            }
            catch(Exception e) { Console.WriteLine(e); }
            _dbobj = null; cmd = null; ds = null; dt = null;
            return subjects;
        }

        public async Task<List<StaffInfo>> GetStaffInfo()
        {
            List<StaffInfo> staffInfos = new List<StaffInfo>();
            _dbobj = new dbclass();
            ds = new DataSet();
            cmd = new SqlCommand("Select ID,(FirstName+' '+LastName) as Name from Participant where UserType=2 order by Name");
            ds = await _dbobj.ExcuteDS_CMDAsync(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    staffInfos = dt.AsEnumerable().Select(row => new StaffInfo
                    {
                        ID = row.Field<Int64>("ID"),
                        Name = row.Field<string>("Name")
                    }).ToList();
                }
            }
            _dbobj = null; cmd = null; ds = null; dt = null;
            return staffInfos;
        }

        public async Task<List<SubjectInfo>> GetSubjectIfo()
        {
            List<SubjectInfo> subjectInfos = new List<SubjectInfo>();
            _dbobj = new dbclass();
            ds = new DataSet();
            cmd = new SqlCommand("Select ID,SubjectName from Subjects order by ID desc");
            ds = await _dbobj.ExcuteDS_CMDAsync(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    subjectInfos = dt.AsEnumerable().Select(row => new SubjectInfo
                    {
                        SubjectID = row.Field<Int64>("ID"),
                        SubjectName = row.Field<string>("SubjectName")
                    }).ToList();
                }
            }
            _dbobj = null; cmd = null; ds = null; dt = null;
            return subjectInfos;
        }

        public async Task<List<SubjectInfo>> GetSubjectIfoBYID(string StaffID)
        {
            List<SubjectInfo> subjectInfos = new List<SubjectInfo>();
            _dbobj = new dbclass();
            ds = new DataSet();
            cmd = new SqlCommand("Select S.ID,S.SubjectName from Subjects S join AssignSubject A on A.AssignedSubject=S.ID where A.StaffName =@StaffID ");
            cmd.Parameters.AddWithValue("@StaffID", StaffID);
            ds = await _dbobj.ExcuteDS_CMDAsync(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = new DataTable();
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    subjectInfos = dt.AsEnumerable().Select(row => new SubjectInfo
                    {
                        SubjectID = row.Field<Int64>("ID"),
                        SubjectName = row.Field<string>("SubjectName")
                    }).ToList();
                }
            }
            _dbobj = null; cmd = null; ds = null; dt = null;
            return subjectInfos;
        }

        public async Task<int> assignSubject(string Staff, string Subject)
        {
            int res = 0;
            _dbobj = new dbclass();
            cmd = new SqlCommand("Insert into AssignSubject values(@Staff,@Subject)");
            cmd.Parameters.AddWithValue("@Staff", Staff);
            cmd.Parameters.AddWithValue("@Subject", Subject);
            res = await _dbobj.ExcuteNonqueryCMDAsync(cmd);
            cmd = null;

            return res;
        }

        public async Task<List<StaffwithSubList>> GetStaffWithSub(string pagesize, string pageno)
        {
            List<StaffwithSubList> staffwithSubs = new List<StaffwithSubList>();
            _dbobj = new dbclass();
            try
            {
                cmd = new SqlCommand("Proc_ManageSubjects");
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.Parameters.AddWithValue("@PageNum", pageno);
                cmd.Parameters.AddWithValue("@Inputtype", "GetStaffwithSub");
                ds = new DataSet();
                ds = await _dbobj.ExcuteDS_SPAsync(cmd);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            StaffwithSubList staffwithSub = new StaffwithSubList();
                            staffwithSub.ID = Convert.ToInt64(dr["ID"]);
                            staffwithSub.RowNum = Convert.ToInt64(dr["RowNum"]);
                            staffwithSub.StaffName = dr["StaffName"].ToString();
                            staffwithSub.SubName = dr["SubjectName"].ToString();
                            staffwithSub.RowCnt = Convert.ToInt32(dr["RowCnt"]);
                            staffwithSubs.Add(staffwithSub);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            _dbobj = null; cmd = null; ds = null; dt = null;
            return staffwithSubs;
        }
        #endregion
        }
}