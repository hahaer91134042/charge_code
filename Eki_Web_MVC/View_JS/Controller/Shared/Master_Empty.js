


function SetLoginUser() {
    eki.req.GetAdminUser.post()
        .then(result => {

            var data = result.data;
            //console.log(`user data->${JSON.stringify(data)}`)

            //$('#ltName').text(data.Name)
            //$('#hidUserId').text(data.Id)
            //$('#hidLoginUid').text(data.Account)

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

