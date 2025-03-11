


let smsCode = AppConfig.defaultSmsCode
//let smsCode = ''
/*
 紀錄充電裝UID or  序號
 */
let uid=''

var vue = {

    el: '#login_box',

    data: {
        validPhone: false,
        errorMsg: '',
        userCode: '',
        userPhone:'',
        userMail:''
    },
    watch: {
        

    },
    methods: {
        onCheckPhone() {
            if (vue.data.userPhone.length < 1) {
                vue.data.errorMsg="請先輸入手機號碼"
                return
            }

            if (!vue.data.userPhone.isPhone()) {
                vue.data.errorMsg='手機格式錯誤'
                return
            }

            vue.data.errorMsg=''
            vue.data.validPhone = true

            let param = { countryCode: '886', phone: vue.data.userPhone, lan: 'TC' }

            sendReq(eki.req.SmsConfirm.post(param), {
                success(info) {
                    console.log(`sms resp->${info.jsonString()}`)
                    //showToast({msg:"完成"})
                    smsCode = info.Code
                },
                fail(code, msg) {
                    console.log(`sms error code->${code} msg->${msg}`)
                    showToast({msg:msg})
                }
            })

        },
        onRefreshCheck() {
            vue.data.validPhone = false
            smsCode=''

        },
        onRegisterUser() {
            if (!vue.data.userPhone.isPhone()) {
                vue.data.errorMsg = "手機格式錯誤"
                return
            }

            if (!vue.data.userMail.isEmail()) {
                vue.data.errorMsg = "信箱格式錯誤"
                return
            }

            if (vue.data.userCode != smsCode) {
                vue.data.errorMsg="驗證碼錯誤"
                return
            }

            console.log(`code check->${smsCode == vue.data.userCode}`)

            //if (uid.length < 1) {
            //    location.replace("/ocs")
            //} else {
            //    location.replace(`/ocs/charge/${uid}`)
            //}
            //return

            vue.data.errorMsg = ''

            let param = { phone: vue.data.userPhone, mail: vue.data.userMail }

            sendReq(eki.req.RegisterUser.post(param), {
                success(info) {
                    alert("註冊成功!")
                    if (uid.length < 1) {
                        location.replace("/ocs")
                    } else {
                        location.replace(`/ocs/charge/${uid}`)
                    }
                },
                fail(code, msg) {
                    alert(msg)
                }
            })

        }

    },
    filters: {

    },
    computed: {
        goRegister() {
            if (vue.data.userCode.length < 1)
                return false


            return true
        }


    },
    mounted() {

    },
    created() {
        console.log("login_box created ")
        

        let url = new URL(window.location)

        console.log(`url->${window.location}  obj->${url}  path->${url.pathname}`)

        let arr = url.pathname.split('/')

        if (arr.length>3)
            uid = arr[arr.length - 1]
        console.log(`uid->${uid}`)
    }
}

$(function () {

    NProgress.configure({
        parent: '#login_box'
    });

    eki.vue(vue)

})


