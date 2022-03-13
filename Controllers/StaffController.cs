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
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddQuestions()
        {
            return View();
        }

        [HttpPost]
        public async Task<int> SaveQuestions(QuestionObj obj)
        {
            int res = 0;
            //QuestionObj obj = new QuestionObj();
            StaffExchange se = new StaffExchange();
            res =await se.SaveQuestions(obj);

            return res;
        }
    }
}