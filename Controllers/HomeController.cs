using ScholarPortal.DataLayer;
using ScholarPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ScholarPortal.Filters;

namespace ScholarPortal.Controllers
{
    [Error]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult CreateRegistry()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetCountryNames()
        {
            List<CountyNames> countries = new List<CountyNames>();
            HomeExchange He = new HomeExchange();

            countries = await He.GetCountry();

            return Json(new { countries }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GetStateNames()
        {
            List<States> states = new List<States>();
            int CountryID = Convert.ToInt32(Request["CountryID"]);

            HomeExchange He = new HomeExchange();
            states = await He.GetStates(CountryID);

            return Json(new { states }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Loginctr()
        {            
            LoginSession loginSession = new LoginSession();
            string UserId = Request["UserId"];
            string Password = Request["Password"];
            HomeExchange He = new HomeExchange();
            
            loginSession = await He.Logincheck(UserId, Password);

            return Json(new { loginSession });
        }
        #region Register
        //[HttpPost]
        //public async Task<JsonResult> SaveRegistry(object sender, EventArgs e, Registerinfo info)
        //{
        //    string result = "0";

        //    HomeExchange He = new HomeExchange();
        //    result = await He.SaveRegistryinfo(info);
        //    if (result == "0")
        //    {
        //        string ParticipantID = "";
        //        CustomExchange ce = new CustomExchange();
        //        string dircetotyPath = ce.CheckParticipantLogoDirectory(ParticipantID);

        //        byte[] bytes = Convert.FromBase64String(Request["file"]);
        //        System.Drawing.Image img;
        //        using (MemoryStream ms = new MemoryStream(bytes))
        //        {
        //            img = System.Drawing.Image.FromStream(ms);
        //            string filePath = dircetotyPath + "Logo.png";
        //            img.Save(filePath, ImageFormat.Png);
        //        }
        //        ////Store Logo Url in session. 
        //        //Session["ParticipantLogo"] = @"/Images/ProviderLogo/" + ParticipantID + @"/Logo.png";
        //    }

        //    //return Json(result);
        //    return Json(new { result }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<JsonResult> SaveRegistry(Registerinfo info)
        {
            string result = "0";
            if (ModelState.IsValid)
            {
                HomeExchange He = new HomeExchange();
                result = await He.SaveRegistryinfo(info);
                if (Convert.ToInt32(result) > 0)
                {
                    CustomExchange ce = new CustomExchange();
                    string dircetotyPath = ce.CheckParticipantLogoDirectory(result);

                    byte[] bytes = Convert.FromBase64String(Request["file"]);
                    System.Drawing.Image img;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        img = System.Drawing.Image.FromStream(ms);
                        string filePath = dircetotyPath + info.Picture;
                        img.Save(filePath);
                    }

                }

            }
            //Response.Redirect("/Home/Index");
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> UploadProfile(Registerinfo info)
        {
            string result = "0";
            //Registerinfo info = new Registerinfo();
            string ParticipantID = Request["ParticipantID"];

            if (info.ID > 0)
            {
                //string ParticipantID = "";
                CustomExchange ce = new CustomExchange();
                string dircetotyPath = ce.CheckParticipantLogoDirectory(ParticipantID);

                byte[] bytes = Convert.FromBase64String(Request["file"]);
                System.Drawing.Image img;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    img = System.Drawing.Image.FromStream(ms);
                    string filePath = dircetotyPath + "Logo.png";
                    //string filePath = dircetotyPath + bytes;
                    img.Save(filePath, ImageFormat.Png);
                }

            }
            HomeExchange He = new HomeExchange();
            result = await He.SaveRegistryinfo(info);

            ////Store Logo Url in session. 
            //Session["ParticipantLogo"] = @"/Images/ProviderLogo/" + ParticipantID + @"/Logo.png";

            return Json(result);
            }

        #endregion


        public ActionResult Pageddl()
        {
            return View();
        }
    }
}