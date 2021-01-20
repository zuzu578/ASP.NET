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

        public ActionResult DoResult(MyModel myModel)
        {
            String Userid = myModel.Userid;
            String UserPassword = myModel.UserPassword;
            Console.WriteLine(Userid);
            Console.WriteLine(UserPassword);




            return View("Result", myModel);
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
        <form action="DoResult" method="post">
            <input type="text" name="Userid" />

            <input type="text" name="UserPassword" />
            <input type="submit" name="submit" value="submit" /> 


        </form>



    </div>
</body>
</html>

