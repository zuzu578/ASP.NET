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
    <div id="table">
        @{
            for(int i = 2; i<10; i++)
            {
                for(int j = 1; j<10; j++)
                {
                    <h1>@i*@j=@(i*j)</h1>
                }
            }
        } 
        
    </div>
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
                    alert("success");
                    console.log(data);

                }
                 

            
            });
            });


</script>

