
import * as Meter from './Item/Meter.js';


//個別單元的vue實體
let meter = Meter.vue;


let userTop = new VueController({

    el: '#PageTop',

    data: {
        showSidebar: false,
        title: "電表"


    },
    watch: {


    },
    methods: {


    },
    filters: {

    },
    computed: {
        meterCount() {            
            return userTop.main.data.user.Meter.length
        },
        name() {
            return userTop.main.data.user.Name
        },
        notifyCount() {
            return 0;
        }
    },
    mounted() {
        
    },
    created() {

    }
})

//這是共用功能
let main = new ViewController({
    
    data: {
        user: new Community()
    },
    //主要是初始化一些通用設定
    init() {

        //user 不能用紀錄的 要是切換使用者會錯誤

        sendReq(eki.req.CpsUser.post(), {
            success(info) {
                console.log(`Cps load user->${info.jsonString()}`)
                main.data.user = new Community(info)

                userTop.main = main
                eki.vue(userTop)
            },
            fail(code, msg) {
                alert(msg)
            }
        }, false)


        meter.main = main
        eki.vue(meter)

    },
    onOpenPage() {
        console.log('Main->onOpenPage')
        Session.setJson(main.data.user, Flag.User)
    }


})


$(function () {

    NProgress.configure({
        parent: '#MainDiv'
    });

    //初始化PageTop
    main.init()

    

})
