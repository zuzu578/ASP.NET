using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using web001.Models;

namespace web001.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var firstUser = new User();
            firstUser.UserNo = 1;
            firstUser.UserName = "Lee";
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;


            //viewBag 방법
            ViewBag.User = firstUser.UserName;
            //viewData --> java model( )
            // view 에다가 ViewData[ attribute ] = value ; 이런식으로
            //값을 던져줌 

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
            ViewData["String"] = "hello world";
            return View();
        }
    }
}
