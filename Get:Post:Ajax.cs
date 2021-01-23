using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using myPractice001.Models;

namespace myPractice001.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MyAjax()
        {
            return View("MyAjax");
        }
        [HttpPost]
        public ActionResult DoMyAjax(UserInfo model)
        {
            //1) Ajax
            string userid = model.userid;
            string userpassword = model.userpassword;
            Console.WriteLine(userid);
            Console.WriteLine(userpassword);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoGetApi()
        {
            //2) Get API()
            string userid = Request.QueryString["userid"];
            string userpassword = Request.QueryString["userpassword"];
            ViewData["userid"] = userid;
            ViewData["userpassword"] = userpassword;
            return View("Result"); 
        }
        public ActionResult DoPostApi(UserInfo model)
        {
            //3) Post API()
            string userid = model.userid;
            string userpassword = model.userpassword;
            return View("Result", model);
        }
    }
}
