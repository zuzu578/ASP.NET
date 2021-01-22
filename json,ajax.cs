using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MyJson001.Models;

namespace MyJson001.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MyJson()
        {
            return View("MyJson");
        }
        [HttpPost]
        public ActionResult MyJson(UserInfo model)
        {
            string userid = model.userid;
            string userpassword = model.userpassword;
            Console.WriteLine(userid);
            Console.WriteLine(userpassword);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}



@model MyJson001.Models.UserInfo

@{ Layout = null; }

<!DOCTYPE html>
<html>
<head>
    <title>MyJson</title>
</head>
<body>
    <input type="text" name="userid" id="userid" /><br />
    <input type="text" name="userpassword" id="userpassword" /><br />
    <input type="button" name="button" value="button" id="button" />
    <h1> result</h1>
    <input type="text" name="result" id="result" readonly />
    <input type="text" name="result2" id="result2" readonly />

</body>
</html>
<script src="https://code.jquery.com/jquery-3.5.0.js"></script>
<script>
    $(document)
        .on("click", "#button", function () {

           //Json 방법으로 출력하는법
           //1) success --> (json) --> controller 받아오는 값이 여러개 (json)
           //2) json.object1 json.object2 --> 이렇게 사용


            $.ajax({
                url: "MyJson",
                type: "post",
                //dataType : "json",
                data: {
                    "userid": $("#userid").val(), "userpassword": $("#userpassword").val()
                },

                success: function (json) {
                    alert("success");
                    console.log(json.userid);
                    $("#result").val(json.userid);
                    $("#result2").val(json.userpassword);


                }
            });
            });


</script>



