using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MyPractice0003.Models;

namespace MyPractice0003.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MyList()
        {
            List<DemoModel> models = new List<DemoModel>()
            {
                //List 출력방법 --> view에 IEnumerable<MyPractice0003.Models.DemoModel> 를 명시해주어야한다.
                new DemoModel {Id = 1, Name ="hello"},
                new DemoModel {Id = 2, Name ="world"},
                new DemoModel {Id = 3, Name ="Csharp"}



            };

            return View("MyList",models);
        }
    }
}






@model IEnumerable<MyPractice0003.Models.DemoModel>

@{
    Layout = null;
}



<!DOCTYPE html>
<html>
<head>
    <title>MyList</title>
</head>
<body>
    <h1> my List</h1>
    @foreach (var m in Model)
    {
        <li>@m.Id, @m.Name</li>
    }
</body>
</html>

