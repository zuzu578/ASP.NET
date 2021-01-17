using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Solution001.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            string fname = Request.QueryString["fname"];
            string lname = Request.QueryString["lname"];
        
            ViewData["fname"] = fname;
            ViewData["lname"] = lname;
            return new RedirectResult(@"~\Home\Result");
            //return Redirect("/Home/Result");
        }
        
        public ActionResult Result()
        {
            return View();
        }
        
        public ActionResult New()
        {
            
            return View();

        }

       
    }
}
