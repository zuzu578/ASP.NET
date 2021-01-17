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
        public String Index(int Id)
        {
            /*
            string fname = Request.QueryString["fname"].ToString();
            string lname = Request.QueryString["lname"].ToString();
            */
            if (Id > 0)
            {
                return "hello world";
            }
            else
            {



                var mvcName = typeof(Controller).Assembly.GetName();
                var isMono = Type.GetType("Mono.Runtime") != null;

                ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
                ViewData["Runtime"] = isMono ? "Mono" : ".NET";



                return "sub number";
            }
        }
    }
}
