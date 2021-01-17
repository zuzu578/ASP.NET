using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Services;
using Solution001.Models;

namespace Solution001.Controllers
{
    public class HomeController : Controller
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(  UserModel userModel)
        {
            /*
            string Userid = Request.QueryString["Userid"];
            string password = Request.QueryString["password"];
            */
            //get 방법
            //1)get --> Request.QueryString[ ] ; --> viewData 를 이용해서 view 에 데이터 전송
           
            /*
            String fname = Request.QueryString["fname"];
            String lname = Request.QueryString["lname"];
            */
            //Post 방법
            //2) post --> model 객체를 생성 , 강력한 view 페이지 생성 , return , model 참조변수를 전달 해서
            //view 에서 @model.fname (ex) 이런식으로
            //사용하면된다.  
            String fname = userModel.fname;
            String lname = userModel.lname;
            Console.WriteLine(fname);
            Console.WriteLine(lname);
            if (!fname.Equals("dlwnghks") || !lname.Equals("123qwe")) 
            {
                Console.WriteLine("debug1");
                Console.WriteLine(fname);
                Console.WriteLine(lname);
                return View("Error", userModel);
            }
            //get 방법 
            /*
            ViewData["fname"] = fname;
          
            ViewData["lname"] = lname;
            */
            //* return view( ) view 메서드안에는 리턴하고싶은 View 를 입력하면된다.
            return View("Result", userModel);


            //return new RedirectResult(@"~\Home\Result");
            //return Redirect("/Home/Result");
        }
        public ActionResult Error()
        {
            return View();
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
