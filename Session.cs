using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MyMVCProjects001.Models;

namespace MyMVCProjects001.Controllers
{
    public class HomeController : Controller
    {
        
        [HttpPost]
        public ActionResult Result(UserInfo userinfo)
        {
            //Session 사용 방법 //
            Session["UserId"] = userinfo.UserId.ToString();
            //세션에서 SessionUserId 에다가 userinfo.UserId.ToString(); 형식으로 세션의 아이디 문자열을 담음 
            String SessionUserId = Session["UserId"].ToString();
            String UserId = SessionUserId;
            
            String UserPassword = userinfo.UserPassword;
            //login Failed 
            if (!UserId.Equals("dlwnghks6821") || !UserPassword.Equals("123qwe"))
            {
                return View("NotFound", userinfo);
            }
            //login Success --> Return Session
           
            return View("Result", userinfo);
        }
      


         
        public ActionResult Login()
        {
            return View("Login");
        }


      
    }
}

