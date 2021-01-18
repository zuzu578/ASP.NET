using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Script.Services;
using System.Web.Services;
using MyMVCProjects001.Models;

namespace MyMVCProjects001.Controllers
{
    public class HomeController : Controller
    {
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //post --> Ajax

        //1) Model -- > String / int --> result --> return(Ajax View) 
        [HttpPost]

        public String DoLogin(UserInfo userInfo)
        {
            String UserId = userInfo.UserId;
            String UserPassword = userInfo.UserPassword;
            Console.WriteLine(UserId);
            Console.WriteLine(UserPassword);

            if(UserId.Equals("dlwnghks6821")&&UserPassword.Equals("123qwe"))
            {
                String result = "1";
                Console.WriteLine("login-success");
                //return View("Result");
                return result;
            }
            else
            {
                Console.WriteLine("Undefined User");
                return "0";
            }

        }
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
        public ActionResult Result()
        {
            return View("Result");
        }
        public ActionResult Login()
        {
            return View("Login");
        }


      
    }
}







@{ Layout = null; }

<!DOCTYPE html>
<html>
<head>
    <title>Login</title>
</head>
<body>
    
   
        <input type="text" id="UserId" name="UserId" /><br>
        <input type="text" id="UserPassword" name="UserPassword" />
        <input type="button" value="submit" id="button">

  
</body>
</html>
<script src="https://code.jquery.com/jquery-3.5.0.js"></script>
<script>
    $(document)
        .on("click", "#button", function () {
            $.ajax({
                url: "DoLogin",
                type: "post",
                //dataType : "json",
                data: {
                    "UserId": $("#UserId").val(), "UserPassword": $("#UserPassword").val()
                },

                success: function (data) {
                   
                    
                    if (data == '1') {
                        alert("login success.");
                        window.location.href = "http://127.0.0.1:8080/Home/Result";
                        

                    } else if (data == '0') {

                        alert("undefined User");
                        window.location.href = "http://127.0.0.1:8080/home/NotFound";
                    }
                    
                }
            })
        });
   
       
    
</script>



