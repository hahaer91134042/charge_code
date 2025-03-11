


class MeterCp {
    Img ='waiting.png'
    Cp = new ChargePoint()
    Loc = new CpsLocation()
    House = new CpsHouse() //這裡要注意 House裡面有沒有Loc
    constructor(mc) {
        if (mc) {
            this.Cp = new ChargePoint(mc.Cp)
            this.Loc = new CpsLocation(mc.Loc)
            this.House = new CpsHouse(mc.House)
        }
    }
}




//個別單元的vue實體
let userTop = new VueController({

    el: '#PageTop',

    data: {
        showSidebar: true,
        title: "充電樁監控"



    },
    watch: {


    },
    methods: {


    },
    filters: {

    },
    computed: {


        cpCount() {            
            return cpMonitor.data.meterCpList.length
        },
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
        //console.log(`PageTop created`)
    }
})

let menu = new VueController({

    el: '#sidebar',

    data: {        
        root: { name: '區塊電' },
        //無法使用這種做法了 adminkit對vue 跟asp.net相性 不好
        //menuOption: [
        //    { name: '電表',icon:null ,isHeader: true },
        //    { name: '電表報表', icon: 'bar-chart-2', isHeader: false },
        //    { name: '充電樁監控', icon: 'sliders', isHeader: false},
        //    { name: '充電樁報表', icon: 'bar-chart-2',isHeader: false}
        //]


    },
    watch: {


    },
    methods: {

        selectItem(item) {
            //console.log(`menu select item->${item.jsonString()}`)

            $('li.sidebar-item').removeClass('active')
            $(`li#${item.id}`).addClass('active')

            switch (item.name) {
                case '電表報表':
                    console.log('select 電表報表')
                    cpMonitor.data.show = false


                    break;
                case '充電樁監控':
                    console.log('select 充電樁監控')
                    cpMonitor.data.show = true


                    break;
                case '充電樁報表':
                    console.log('select 充電樁報表')
                    cpMonitor.data.show = false


                    break;
            }


        },

        closePage() {
            console.log('menu click close page')


        }

    },
    filters: {

    },
    computed: {

    },
    mounted() {

    },
    created() {

        
    }
})

let cpMonitor = {
    el: '#cpMonitorDiv',

    data: {
        show: true,
        selectMeter: '',
        meterCpList: new ArrayList(),
        cpDocList: new ArrayList()
    },
    watch: {


    },
    methods: {

        onHouseStatus(item) {


            console.log(`onHouseStatus->${item.jsonString()}`)
        }
    },
    filters: {
        //cpStatusImg(serial) {
        //    console.log(`cpStatusImg serial->${serial}`)

        //    return ''
        //}
    },
    computed: {
        cpChargingCount() {
            return cpMonitor.data.cpDocList.filter(d => d.status == OCPP_Status.Cp.Charging).length
        },
        cpAvailableCount() {
            let count = 0
            cpMonitor.data.cpDocList.forEach(d => {
                switch (d.status) {
                    case OCPP_Status.Cp.Finishing:
                    case OCPP_Status.Cp.Preparing:
                    case OCPP_Status.Cp.Available:
                        count++
                        break;
                }
            })
            return count
        },
        cpUnavailableCount() {
            return cpMonitor.data.cpDocList.filter(d => d.status == OCPP_Status.Cp.Unavailable).length
        }
    },
    mounted() {

    },
    created() {
        console.log(`cpMonitor select meter->${cpMonitor.data.selectMeter} `)
        var param = { serNum: [cpMonitor.data.selectMeter] }
        sendReq(eki.req.CpsMeterCp.post(param), {
            success(info) {
                //console.log(`Meter cp->${info.jsonString()}`)
                let data= info.first(d => d.Meter == cpMonitor.data.selectMeter)

                cpMonitor.data.meterCpList.addAll(data.Data.map(d => new MeterCp(d)))

                //console.log(`cpList->${cpMonitor.data.meterCpList.jsonString()}`)

                cpMonitor.data.cpDocList.addAll(
                    cpMonitor.data.meterCpList.map(data => db.community(user.Uid).cpSerial(data.Cp.Serial))
                )

                cpMonitor.startCpSnapshot()
                

                //db.community(user.Uid).cpSerial('serial123').get()
                //    .then((doc) => {

                //    console.log("fcm db ->" + doc.data().jsonString())

                //})                

            },
            fail(code, msg) {
                alert(msg)
            }
        })
    },

    startCpSnapshot() {
        cpMonitor.data.cpDocList.forEach(cpSerial => {
            console.log(`doc id->${cpSerial.id} status->${cpSerial.status}`)
            cpSerial.onSnapshot(doc => {
                console.log(`cpSerial onSnapshot id->${cpSerial.id} status->${cpSerial.status}`)

                //doc_cpSerialSnapshot(cpSerial)
                let cp = cpMonitor.data.meterCpList.first(d => d.Cp.Serial == cpSerial.id)

                switch (cpSerial.status) {
                    case OCPP_Status.Cp.Unavailable:
                        cp.Img = 'waiting.png'
                        break;
                    case OCPP_Status.Cp.Finishing:
                    case OCPP_Status.Cp.Preparing:
                    case OCPP_Status.Cp.Available:
                        cp.Img = 'finished.png'
                        break;
                    case OCPP_Status.Cp.Charging:
                        cp.Img = 'charging.png'
                        break;
                    default:
                        cp.Img = 'problem.png'
                        break;
                }

            })
        })
    }
}

//let doc_cpSerialSnapshot = function (doc) {

//    let cp = cpMonitor.data.meterCpList.first(d => d.Cp.Serial == doc.id)

//    switch (cpSerial.status) {
//        case OCPP_Status.Cp.Unavailable:
//            cp.Img = 'waiting.png'
//            break;
//        case OCPP_Status.Cp.Finishing:
//        case OCPP_Status.Cp.Preparing:
//        case OCPP_Status.Cp.Available:
//            cp.Img = 'finished.png'
//            break;
//        case OCPP_Status.Cp.Charging:
//            cp.Img = 'charging.png'
//            break;
//        default:
//            cp.Img='problem.png'
//            break;
//     }

//}

let user = new Community(Session.json(Flag.User))
let meterdata = new MeterOrder(Session.json())
// Initialize Cloud Firestore and get a reference to the service
const db = new Cps_Firestore_db('cps_test');

//這是共用功能
let main = new ViewController({
    data: {
        user: new Community()

    },
    //主要是初始化一些通用設定
    init() {

        main.data.user = user

        console.log(`MainMeter user->${user.jsonString()}`)
        console.log(`MainMeter data->${meterdata.jsonString()}`)

        //這裡 起始順序不要變動
        cpMonitor.data.selectMeter = new ArrayList(window.location.pathname.split('/')).latest
        eki.vue(cpMonitor)

        userTop.main = main
        eki.vue(userTop)

        menu.main = main
        eki.vue(menu)

    }
})


$(function () {

    NProgress.configure({
        parent: '#MainDiv'
    });

    //初始化PageTop
    main.init()

})