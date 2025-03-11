
let data = {
    cp: new ChargePoint(),
    uid:''
}

let event = {
    //通知外部這step完成
    onFinish: function (phone, chargeType) {

    }
}

var vue = new VueController({

    el: '#charge_step1',

    data: {
        show: false,
        showRegisterMsg:false,
        chargeConfig: new ChargeConfig(),

        userPhone: '',
        chargeType: ChargeType.Time,

        selectStyle: {
            border: '4px solid',
            borderColor:'rgb(153 203 81)'
        },
        unSelectStyle: {
            border: '2px solid',
            borderColor: 'rgb(217 217 217)'
        }
    },
    watch: {
        show: function (n, o) {
            if (n)
                vue.methods.onSelect()
        },
        userPhone: function (n, o) {
            if (n.length > 0)
                vue.data.showRegisterMsg = false
            else
                vue.data.showRegisterMsg=true
        }

    },
    methods: {
        //menu被選擇到了觸發
        onSelect() {            

        },
        onRegister() {
            location.href = `/ocs/register/${data.uid}`
        },
        onSelectType(type) {
            vue.data.chargeType = type
            //console.log(`onSelectType->${type}`)
        },
        onNext() {
            if (vue.data.userPhone.length < 1) {
                showToast({ msg: "手機號碼不可為空白" })
                return
            }
            if (!vue.data.userPhone.isPhone()) {
                alert("手機號碼格式錯誤")
                return
            }

            let param = { serial: vue.data.userPhone }
            //確認該手機是否為會員
            sendReq(eki.req.CheckUserPhone.post(param), {
                success(info) {
                    vue.data.show = false
                    event.onFinish(vue.data.userPhone, vue.data.chargeType)
                },
                fail(code, msg) {//手機非會員
                    vue.data.userPhone = ''
                    vue.data.showRegisterMsg = true;
                    showToast({msg:'非會員 請先進行註冊'})
                }
            })

        }
    },
    filters: {

    },
    computed: {

    },
    mounted() {

    },
    created() {
        console.log("charge step1 created")
    }
})

export { vue, data, event }


