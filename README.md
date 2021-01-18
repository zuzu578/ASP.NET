# ASP.NET

1) ASP.Net --> Mvc Pattern 

Form 
1) POST / GET 

* POST --> 걍력한 view page 생성 
 [AcceptVerbs(HttpVerbs.Post)] 또는 [httpPost] 를 method 위에 명시 (Annotation) 
Model 객체를 생성 -- > property (Getter , setter) 
ex) 
using System;
namespace Solution001.Models
{
    public class UserModel
    {
        public String fname{get;set;}
        public String lname { get; set; }
    }
    }
}
model 에 post 로 전달한 값을 담아주고 그값을 매개변수로 원하는 페이지로 return 을 해주면 된다 
public ActionResult Index(UserModel userModel){
//--> UserModel (내가 Post 로 던져준 데이터를 받기위한 model 객체 ( 사용자 정의 모델 객체 ) )
ex) --> view 에서 post 방식으로 fname 이라는 데이터를 여기 컨트롤러에 전달한다 --> model 객체 정의  --> 변수에 model을 담아서 
return 


String fname = userModel.fname;
return View("Error", userModel);
}

view 단에서는 어떻게 대응해야 하는가 ? 
*-->
@model Solution001.Models.UserModel( 강력한 뷰생성 )


@model Solution001.Models.UserModel

@{
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>Result</title>
</head>
<body>
    <h1> Result Page </h1>
    <p> 아이디:@Model.fname</p>
    <p> 비밀번호: @Model.lname </p>

</body>
</html>

view 에서는 컨트롤러에서 return 한 값을 사용하기위해서 @Model.fname 이런식으로 출력을 해주면 된다 .



*주의사항
! @Model 과 @model 은 서로 다르다 
@model @model을 하나의 view 에서 2개 이상을 쓸경우 오류가난다 .


*Ajax 비동기 통신방법추가 
