 
        public ActionResult DoLogin()
        {
            String UserId = Request.QueryString["UserId"];
            String UserPassword = Request.QueryString["UserPassword"];
            ViewData["UserId"] = UserId;
            ViewData["UserPassword"] = UserPassword;
            Console.WriteLine(UserId);
            Console.WriteLine(UserPassword);
            return View("Result");
        }





<!DOCTYPE html>
<html>
<head>
    <title>Result</title>
</head>
<body>
   
    <h1> @ViewData["UserId"]</h1>
    <h1> @ViewData["UserPassword"]</h1>
</body>
</html>


      