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




@model POSTAPI.Models.Userinfo

@{ Layout = null; }

<!DOCTYPE html>
<html>
<head>
    <title>NewAjax</title>
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
                url: "DoAjax",
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

