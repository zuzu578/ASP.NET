using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MvcCalculator.Models;

namespace MvcCalculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Form()
        {

            return View("Form");
        }
        [HttpPost]
        public ActionResult Transfer(MyNumber model)
        {
            int number = model.number;

            return View("Result",model);
        }
    }
}




@model MvcCalculator.Models.MyNumber

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>Form</title>
</head>
<body>
    <form action="Transfer" method="post">
        <input type="text" name="number"id="number"/>
        <input type="submit" name="submit" id="submit" value="submit" />
    </form>
</body>
</html>





@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>Result</title>
</head>
<body>
    <h1> Calculator Result </h1>
    @for(int i= 1 ; i<=9;i++){
        <h1>@Model.number*@i=@(Model.number*i)</h1>
        
    }
     
</body>

</html>

