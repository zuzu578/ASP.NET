using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Post001.Models;

namespace Post001.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MyPostForm()
        {
            return View("MyPostForm");
        }

        public ActionResult DoResult()
        {
            //String Userid = myModel.Userid;
            //String UserPassword = myModel.UserPassword;

            String Userid = Request.QueryString["Userid"];
            String UserPassword = Request.QueryString["UserPassword"];
            Console.WriteLine(Userid);
            Console.WriteLine(UserPassword);
            ViewData["Userid"] = Userid;
            ViewData["UserPassword"] = UserPassword;
            //List<string> MyModel = new List<string>();
         







            









            return View("Result");
        }
    }




}












@model Post001.Models.MyModel

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>MyPostForm</title>
</head>
<body>
    <div class="myForm">
        <form action="DoResult" method="get">
            <input type="text" name="Userid" />

            <input type="text" name="UserPassword" />
            <input type="submit" name="submit" value="submit" />


        </form>



    </div>
</body>
</html>




@model Post001.Models.MyModel

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>Result</title>
</head>
<body>
    <h1> Result </h1>

  <h1> @ViewData["Userid"]</h1>
    <h1> @ViewData["UserPassword"]</h1>
</body>
</html>

