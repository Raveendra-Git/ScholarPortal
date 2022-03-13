using CareMatrix.Filters;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ScholarPortal.DataLayer;
using ScholarPortal.Filters;
using ScholarPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScholarPortal.Controllers
{
    [Error]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Admin()
        {
            //if (Session["LoginID"] != null && Session["LoginID"].ToString() != "")
            //{
                return View();
            //}           
            //    return RedirectToAction("Index","Home");        
        }

        public ActionResult TilePage()
        {
            //if (Session["LoginID"] != null && Session["LoginID"].ToString() != "")
            //{
                return View();
            //}
            //return RedirectToAction("Index", "Home");
        }


        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult AddStaff()
        {
            return View();
        }

        public ActionResult AddSubjects()
        {
            return View();
        }
        public ActionResult TrailANDError()
        {
            return View();
        }

        #region "Staff Management"
        [HttpPost]
        public async Task<ActionResult> ManageUsers(FormCollection frm)
        {
            List<UserDetails> userDetails = new List<UserDetails>();
            string pagesize = Request["PageSize"];
            string pageno = Request["PageNo"];
            string UserID = Request["UserID"];
            string Gender = Request["Gender"];
            string Name = Request["Name"];
            string Address = Request["Address"];
            AdminExchange AE = new AdminExchange();
            userDetails = await AE.ManageUserDetails(pagesize, pageno,UserID,Gender,Name,Address);

            return Json(new { userDetails }, JsonRequestBehavior.AllowGet);
        }

        //Delete 
        [HttpPost]
        public async Task<JsonResult> DeleteUserInfo(string ID)
        {
            string result = string.Empty;

            AdminExchange AE = new AdminExchange();
            int rst = await AE.DeleteUserInfo(ID);
            if (rst != 0)
            {
                result = "yes";
            }
            return Json(result);
        }

        //Export Equipments Details to CSV            
        public async Task<ActionResult> ExportUsersDetailToCSV(string AllMembers="")
        {
            List<UsersReport> userslists = new List<UsersReport>();
            AdminExchange ae = new AdminExchange();
            if (Session["LoginID"] != null && Session["LoginID"].ToString() != "")
            {
                userslists = await ae.GetUsersDetailDownload();

                if (userslists.Count == 0)
                    return RedirectToAction("AddStaff");

                string Name = "Users" + "_" + "Report";

                ExcelPackage Ep = new ExcelPackage();
                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Users Report");

                Sheet.Cells["A1"].Value = "ID";
                Sheet.Cells["B1"].Value = "Name";
                Sheet.Cells["C1"].Value = "Address";
                Sheet.Cells["D1"].Value = "DOB";
                Sheet.Cells["E1"].Value = "Age";
                Sheet.Cells["F1"].Value = "Gender";
                Sheet.Cells["G1"].Value = "PhoneNo";
                Sheet.Cells["H1"].Value = "Country";
                Sheet.Cells["I1"].Value = "State";
                Sheet.Cells["J1"].Value = "Email";

                int row = 2;
                foreach (var item in userslists)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    if (item.Name != "") { Sheet.Cells[string.Format("B{0}", row)].Value = (item.Name); } else { Sheet.Cells[string.Format("B{0}", row)].Value = "-"; }
                    if (item.Address != "") { Sheet.Cells[string.Format("C{0}", row)].Value = (item.Address); } else { Sheet.Cells[string.Format("C{0}", row)].Value = "-"; }
                    if (item.DOB != "") { Sheet.Cells[string.Format("D{0}", row)].Value = item.DOB; } else { Sheet.Cells[string.Format("D{0}", row)].Value = "-"; }
                    if (item.Age != 0) { Sheet.Cells[string.Format("ED{0}", row)].Value = item.Age; } else { Sheet.Cells[string.Format("E{0}", row)].Value = "-"; }
                    if (item.Gender != "") { Sheet.Cells[string.Format("F{0}", row)].Value = item.Gender; } else { Sheet.Cells[string.Format("F{0}", row)].Value = "-"; }
                    if (item.PhoneNo != 0) { Sheet.Cells[string.Format("G{0}", row)].Value = item.PhoneNo; } else { Sheet.Cells[string.Format("G{0}", row)].Value = "-"; }
                    if (item.Country != "") { Sheet.Cells[string.Format("H{0}", row)].Value = item.Country; } else { Sheet.Cells[string.Format("H{0}", row)].Value = "-"; }
                    if (item.State != "") { Sheet.Cells[string.Format("I{0}", row)].Value = item.State; } else { Sheet.Cells[string.Format("I{0}", row)].Value = "-"; }
                    if (item.Email != "") { Sheet.Cells[string.Format("J{0}", row)].Value = item.Email + "\r"; } else { Sheet.Cells[string.Format("J{0}", row)].Value = "-"; }
                    row++;
                }

                string CellRange = "A1:E1";

                using (ExcelRange Rng = Sheet.Cells[CellRange]) //"A1:E1"
                {
                    Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Font.Color.SetColor(System.Drawing.Color.WhiteSmoke);
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DimGray);
                }
                ExcelRow r1 = Sheet.Row(1);
                r1.Height = 40;

                Response.Clear();
                Response.ContentType = "text/CSV";
                Response.AddHeader("content-disposition", "attachment; filename=" + Name + ".csv");
                byte[] excelToCsv = EpplusCsvConverter.ConvertToCsv(Ep);
                Response.BinaryWrite(excelToCsv);
                Response.End();
            }
            return new EmptyResult();
        }
        #endregion "Staff Management"

        #region "Add Stubjects"

        //[HttpPost]
        //public async Task<JsonResult> GetStaffList()
        //{
        //    List<Stafflist> stafflists = new List<Stafflist>();
        //    AdminExchange AE = new AdminExchange();
        //    stafflists = await AE.StaffList();
        //    return stafflists;
        //}
        [HttpPost]
        public async Task<int> SaveSubject()
        {
            int res = 0;
            var Subid = Request["SubID"];
            var Subname = Request["SubName"];

            AdminExchange AE = new AdminExchange();
            res = await AE.SaveSubject(Subid,Subname);
            return res;
        }

        [HttpPost]
        public async Task<JsonResult> GetSubjects()
        {
            List<SubjectList> subjectLists = new List<SubjectList>();
            string pagesize = Request["PageSize"];
            string pageno = Request["PageNo"];
            AdminExchange AE = new AdminExchange();
            subjectLists = await AE.GetSubjectList(pagesize, pageno);
            return Json(new { subjectLists},JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public async Task<JsonResult> GetStaffSubject()
        //{
        //    List<StaffSubject> staffSubjects = new List<StaffSubject>();
        //    AdminExchange AE = new AdminExchange();
        //    staffSubjects.Add(new StaffSubject() { stafflist = await AE.GetStaffInfo(), subjectlist = await AE.GetSubjectIfo() });

        //    return Json(staffSubjects, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<JsonResult> GetStafflist()
        {
            List<StaffInfo> StaffInfo = new List<StaffInfo>();
            AdminExchange AE = new AdminExchange();
            StaffInfo = await AE.GetStaffInfo();
            return Json(StaffInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetSubjectlist()
        {
            List<SubjectInfo> SubjectInfo = new List<SubjectInfo>();
            string StaffID = Request["StaffID"];
            int ID = Convert.ToInt16(Request["StaffID"]);
            AdminExchange AE = new AdminExchange();
            SubjectInfo = await AE.GetSubjectIfo();
            return Json(SubjectInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetSubjectlistBYStaffID()
        {
            List<SubjectInfo> SubjectInfo = new List<SubjectInfo>();
            string StaffID = Request["StaffID"];
            int ID = Convert.ToInt16(Request["StaffID"]);
            AdminExchange AE = new AdminExchange();
            SubjectInfo = await AE.GetSubjectIfoBYID(StaffID);
            return Json(SubjectInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> AssignSubject()
        {
            int res = 0;
            string Staff = Request["Staff"];
            string Subject = Request["Subject"];

            AdminExchange AE = new AdminExchange();
            res = await AE.assignSubject(Staff, Subject);
            return Json(res);
        }

        [HttpPost]
        public async Task<JsonResult> GetStaffwithSubject()
        {
            List<StaffwithSubList> staffwithSubLists = new List<StaffwithSubList>();
            string pagesize = Request["PageSize"];
            string pageno = Request["PageNo"];
            AdminExchange AE = new AdminExchange();
            staffwithSubLists = await AE.GetStaffWithSub(pagesize, pageno);
            return Json(new { staffwithSubLists }, JsonRequestBehavior.AllowGet);
        }
        #endregion "Add Subjects"
    }
}