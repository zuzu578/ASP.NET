@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>List2</title>

</head>
<body>
    @{
        List<string> list = new List<string>();
        list.Add("hello");
        list.Add("world");


    }
    @foreach (var item in list)
    {
        <h1>@item</h1>
    }

</body>
</html>

