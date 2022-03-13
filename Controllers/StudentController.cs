using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Rotativa;
using Rotativa.Options;
using ScholarPortal.DataLayer;
using ScholarPortal.Filters;
using ScholarPortal.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScholarPortal.Controllers
{
    [Error]
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExamPortal()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetQuestions()
        {
            List<Questionslist> questionslist = new List<Questionslist>();

            int QuestionID = Convert.ToInt32(Request["QuesID"]);
            int SubjectID = Convert.ToInt32(Request["SubjectID"]);

            StudentExchange SE = new StudentExchange();
            questionslist = await SE.GetQuestionlists(QuestionID, SubjectID);
            return Json(questionslist);
        }

        [HttpPost]
        public async Task<ActionResult> SaveExamQuestion()
        {
            int res = 0;

            int Question = Convert.ToInt32(Request["Question"]);
            int Option = Convert.ToInt32(Request["Option"]);

            StudentExchange SE = new StudentExchange();
            res = await SE.SaveExamQuestion(Question, Option);
            return Json(res);
        }
        [HttpPost]
        public async Task<ActionResult> SubmitTest()
        {
            string res = "";

            int Question = Convert.ToInt32(Request["Question"]);
            int Option = Convert.ToInt32(Request["Option"]);

            StudentExchange SE = new StudentExchange();
            res = await SE.SubmitTest(Question, Option);
            return Json(res);
        }

        public ActionResult ResultPrint(string ParticipantID)
        {
            //Get Student Details
            StudentInfo studentobj = new StudentInfo();
            studentobj.FullName = "Suresh Kumar";
            studentobj.Score = 8;
            //StudentExchange SE = new StudentExchange();
            //res = await SE.SubmitTest(Question, Option);
            //Generate PRINT:-
            return new ViewAsPdf("ResultPrint", studentobj)
            {
                MinimumFontSize = 15,
                PageSize = Rotativa.Options.Size.Letter,
                PageOrientation = Orientation.Portrait,
                CustomSwitches = "--page-offset 1 --footer-right [page] --footer-font-size 10",
                IsJavaScriptDisabled = true,
                PageMargins = new Rotativa.Options.Margins(10, 10, 10, 10),
            };
        }

        //public ActionResult ResultPrint(string eid)
        //{
        //long Id = 0;
        //PrintGRN pf = new PrintGRN();
        //if (Session["LoginID"] != null && Session["LoginID"].ToString() != "")
        //{
        //    if (eid != null && eid != "")
        //    {
        //        CustomExchange ce = new CustomExchange();
        //        Id = Convert.ToInt64(ce.fnDecrypt(eid));
        //        GRNExchange ge = new GRNExchange();
        //        pf = ge.GRn(Id);
        //        ViewBag.GrnList = pf.parts;
        //        ViewBag.SumList = pf.sum;
        //    }
        //}
        //return View(pf);
        //}

        public ActionResult DirectPrint(string eid)
        {
            long Id = 0;                 
            string html = GetContent(Id);
            string fileName = "Trial.pdf";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(html);
            using (MemoryStream memStream = new MemoryStream())
            {
                using (Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memStream);
                    document.Open();
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(html);
                    MemoryStream ms = new MemoryStream(byteArray);
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, ms, System.Text.Encoding.UTF8);
                    ms.Close();
                    document.Close();
                }
                bytes = memStream.ToArray();
                memStream.Close();
            }
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public string GetContent(long ID)
        {
            StringBuilder content = new StringBuilder();
            //content.Append("<td align=\"center\" style=\"border:1px solid #333;width:150px;\"><b>Internal Part Code</b></td>");
            //content.Append("<td align=\"center\" style=\"border:1px solid #333;width:60px;\"><b>UOM</b></td>");
            content.Append("<h2>Could write any content here</h2>");

            return content.ToString();
        }

        }
}