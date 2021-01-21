using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using AjaxCalculator.Models;

namespace AjaxCalculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult myAjax()
        {
            return View("myAjax");
        }
        public int doCalculator(MyNumber model)
        {
            int number = model.number;
            Console.WriteLine(number);

            return number;
        }
    }
}




@model AjaxCalculator.Models.MyNumber

@{ Layout = null; }

<!DOCTYPE html>
<html>
<head>
    <title>myAjax</title>


</head>
<body>
    <input type="text" name="number" id="number" />
    <input type="button" name="button" value="button" id="button" />

    <table id="result">
        <tr>


        </tr>


    </table>
</body>
</html>
<script src="https://code.jquery.com/jquery-3.5.0.js"></script>
<script>
    $(document)
        .on("click", "#button", function () {

            $.ajax({
                url: "doCalculator",
                type: "post",
                //dataType : "json",
                data: {
                    "number": $("#number").val()
                },

                success: function (txt) {
                    alert("success");
                    for (var i = 1; i <= 9; i++) {
                        $("#result").append(txt+"X"+i+"="+(txt*i));
                    }


                }
            });
            });


</script>


