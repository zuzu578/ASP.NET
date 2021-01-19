using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace WebAspNetAjax001.Controllers
{
    public class HomeController : Controller
    {
        //이동을 하게끔하는 Method,Parameter( ) 를 form 으로 받는 method 
        public ActionResult Ajax()
        {
            // <form> //
            String Param1 = Request.QueryString["Param1"];
            Console.WriteLine(Param1);
            ViewData["Param1"] = Param1;
            return View("Ajax");

        }
        //$ajax //
        public int ExAjax()
        {
            String Param1 = Request.QueryString["Param1"];
            Console.WriteLine(Param1);
            int num = Int32.Parse(Param1);
            int sum = 1;
            for(int i = num; i > 0; i--)
            {
                sum = sum *i;
            }
            return sum;

        }
    }
}






@{ Layout = null; }

<!DOCTYPE html>
<html>
<head>
    <title>Ajax</title>
    <style>
        .Ajax-Example{

            width:500px;
            margin:0 auto;
        }


    </style>
</head>
<body>

    <div class="Ajax-Example">
        <h1> Factorial Calculator</h1>
        <input type="text" name="param1" id="param1" /><br />

        <input type="text" name="read" id="read" readonly /><br />
        <input type="button" name="button" id="button" value="button" />
    </div>
    <table id="table">

        
    </table>
</body>
</html>
<script src="https://code.jquery.com/jquery-3.5.0.js"></script>
<script>
    $(document)
        .on("click", "#button", function () {
            $.ajax({
                url: "ExAjax",
                type: "get",
                //dataType : "",
                data: {
                    "param1": $("#param1").val()
                },

                success: function (data) {
                    console.log(data);
                    alert("success");
                    $("#read").val(data);

                  

                    

                }
            });
            });


</script>