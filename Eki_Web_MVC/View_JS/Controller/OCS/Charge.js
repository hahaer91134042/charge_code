
import * as viewData from '../../View/ViewData.js';
import * as step1 from './Item/Charge_step1.js';
import * as step2 from './Item/Charge_step2.js';
import * as step3 from './Item/Charge_step3.js';
import * as step4 from './Item/Charge_step4.js';
import * as statusView from './Item/Charge_status.js';

/**
 *網頁上 cp uid
 * */
let uid = ''
/**
 * Community Uid
 * */
let ComUid=''
let cp = new ChargePoint()
let config = new ChargeConfig()
let chargeType = ChargeType.Time;

const db = new Cps_Firestore_db('cps_test');

let chargeReq = {
    phone: '',//使用者手機號
    cp: '',
    chargeMin: 0,
    payMethod: PayMethod.Credit,
    invo: {}
}


let stepTop = new VueController({

    el: '#step_top',

    data: {
        show:false,
        /**
         現在進行步驟 1~4
         來變更圖片
         */
        nowStep: 1

    },
    watch: {


    },
    methods: {
        initData(data) {
            console.log(`charge cp->${data.jsonString()}`)
            cp = new ChargePoint(data.Cp)
            config = new ChargeConfig(data.Config)
            ComUid = data.Community

            initFirestore()

            console.log(`price min->${config.minPrice}  degree->${config.degreePrice}`)

            step1.data.cp = cp;

            step1.vue.data.chargeConfig = config
            step2.vue.data.chargeConfig = config

            step1.event.onFinish = function (phone, type) {
                console.log(`step1 finish user phone->${phone} type->${type}`)

                chargeReq.phone = phone
                chargeType = type

                step2.vue.data.chargeType = type

                step2.vue.data.show = true
                stepTop.data.nowStep = 2
            }

            step2.event.onFinish = function (min) {
                //傳回使用者充電時數
                console.log(`user charge min->${min}`)

                chargeReq.chargeMin = min

                step3.vue.data.show = true
                stepTop.data.nowStep=3
            }

            step3.event.onFinish = function (payMethod) {

                chargeReq.payMethod = payMethod

                step4.vue.data.show = true
                stepTop.data.nowStep=4
            }

            step4.event.onFinish = function (invo) {
                chargeReq.invo = invo
                console.log(`go checkout invo->${chargeReq.jsonString()}`)

                sendReq(eki.req.OcsCheckOut.post(chargeReq), {
                    success(info) {
                        console.log(`pay result->${info.jsonString()}`)

                        location.href = info.Url;
                        
                    },
                    fail(code, msg) {

                    }
                })

            }

        },
        onBack() {


            stepTop.data.nowStep--

            console.log('back step ->' + stepTop.data.nowStep)

            showStep(stepTop.data.nowStep)

        }

    },
    filters: {

    },
    computed: {

    },
    mounted() {

    },
    created() {
        console.log('step top created')
    }
})

function showStep(i) {
    step1.vue.data.show = false
    step2.vue.data.show = false
    step3.vue.data.show = false
    step4.vue.data.show = false
    switch (i) {
        case 1:
            step1.vue.data.show = true
            break;
        case 2:
            step2.vue.data.show = true
            break;
        case 3:
            step3.vue.data.show = true
            break;
        case 4:
            step4.vue.data.show = true
            break;
    }
}

function initFirestore(finish) {
    db.community(ComUid)
        .cpSerial(cp.Serial)
        .onSnapshot(doc => {
            console.log(`onSnap status->${doc.Status}`)

            statusView.vue.data.status = doc.Status
            statusView.vue.data.nowValue = doc.NowValue
            statusView.vue.data.startWh = doc.StartValue
            statusView.vue.data.startTime=doc.StartTime

            if (doc.Status == OCPP_Status.Cp.Preparing) {
                showCheckout()
            } else {               
                showStatus()
            }
                     
        })
}
function showStatus() {
    stepTop.data.show = false
    showStep(0)
    statusView.vue.data.show = true
}

function showCheckout() {
    stepTop.data.show = true
    stepTop.data.nowStep=1
    showStep(1)
    statusView.vue.data.show = false
}

let main = new ViewController({

    data: {

    },
    init() {
        let url = new URL(window.location)

        console.log(`url->${window.location}  obj->${url}  path->${url.pathname}`)

        let arr = url.pathname.split('/')

        uid = arr[arr.length - 1]
        console.log(`uid->${uid}`)
        step1.data.uid = uid;
        chargeReq.cp = uid;

        eki.vue(step1.vue)
        eki.vue(step2.vue)
        eki.vue(step3.vue)
        eki.vue(step4.vue)
        eki.vue(statusView.vue)
    }    

})


$(function () {

    NProgress.configure({
        parent: '#MainDiv'
    });

    eki.vue(stepTop)


    main.init()



})