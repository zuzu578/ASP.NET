using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using POSTAPI.Models;

namespace POSTAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Typing()
        {
            return View("Typing");
        }
        public ActionResult Result()
        {
            return View("Result");
        }
        [HttpPost]

        public ActionResult DoPost(Userinfo model)
        {
            //list 출력시 
            // List<Userinfo> a1 = new List<Userinfo>();
            string userid = model.userid;
            string userpassowrd = model.userpassword;
            Console.WriteLine(userid);
            Console.WriteLine(userpassowrd);


            return View("Result", model);
        }
        public ActionResult NewAjax()
        {
            return View("NewAjax");
        }
        [HttpPost]
        public ActionResult DoAjax(Userinfo model)
        {

            String userid = model.userid;
            String userpassword = model.userpassword;
            Console.WriteLine(userid);
            Console.WriteLine(userpassword);
            //json 사용법//
            return Json(model, JsonRequestBehavior.AllowGet);
           
        }
    }
}
