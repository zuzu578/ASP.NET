[HttpPost]
        public ActionResult DoLogin(UserInfo userInfo)
        {
            String UserId = userInfo.UserId;
            String UserPassword = userInfo.UserPassword;
            Console.WriteLine(UserId);
            Console.WriteLine(UserPassword);

            return View("Result", userInfo);
        }





<!DOCTYPE html>
<html>
<head>
    <title>Result</title>
</head>
<body>
    <!-- post -->
    <h1> @Model.UserId</h1>
    <h1> @Model.UserPassword</h1>
</body>
</html>
