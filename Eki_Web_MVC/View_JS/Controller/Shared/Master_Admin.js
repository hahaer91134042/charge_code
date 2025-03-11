




//GetUserLogin
//function GetUserLogin() {
//    //var apiUri = serverURL + "api/HomeAPI/GetUserLogin";
//    var apiUri = serverURL + "Admin/GetUserLogin";
//    $.ajax({
//        url: apiUri,
//        type: "post",
//        async: false,                       //確定先讀完
//        contentType: 'application/json',
//        data: JSON.stringify(para),
//        success: function (result) {
//            //let UserLogin = result;
//            vm_UserLogin.UserLogin = result;


//            if (vm_UserLogin.UserLogin.managerID == 0 || vm_UserLogin.UserLogin.name === '') {
//                alert('請先登入!');
//                location.href = '/Admin/Login';
//            }else {
//                //dosomething();
//                //console.log(`userLogin result->${JSON.stringify(result)}`)

//                $('#ltName').text(result.name)
//                $('#hidUserId').text(result.managerID)
//                $('#hidLoginUid').text(result.loginUid)
//                $('#ltTime').text(result.LoginTime)
//            }

//        },
//        error: function (ex) {
//            alert('請先登入!');
//            location.href = '/Admin/Login';
//        }
//    });
//}





function SetLoginUser() {
    eki.req.GetAdminUser.post()
        .then(result => {

            var data = result.data;
            //console.log(`user data->${JSON.stringify(data)}`)

            $('#ltName').text(data.Name)
            $('#hidUserId').text(data.Id)
            $('#hidLoginUid').text(data.Account)
            $('#ltTime').text(eki.time(data.LoginTime))

            //console.log(`init user->${JSON.stringify(eki.adminUser)}`)
            eki.adminUser = data//暫存
            //console.log(`Master save user->${JSON.stringify(eki.adminUser)}`)

        }).catch(ex => {

        })
}



$(function ()
{

    //GetUserLogin();


    SetLoginUser();
    

    

});

