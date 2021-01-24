@{
    Layout = null;
}
@using MyWebApi001.Models;
@model IEnumerable<Data>
@inject DataFinder Finder 

<!DOCTYPE html>
<html>
<head>
    <title>Index</title>
</head>
<body>
    <h1> All List</h1>
    <u1>

        @foreach (var data in Model)
        {
            <li>@data.Id: @data.Name,@data.Title</li>
        }



    </u1>
    <h2> List2</h2>
    @{
        var first = await Finder.GetDataById(1);
    }
    <div>
        first: @first.Id,@first.Name,@first.Title
    </div>

</body>
</html>

