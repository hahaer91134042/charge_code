
/*
 這是vue instance的模板
 */ 

var vue = {

    el: '#charge_status',

    data: {
        show: false,
        status: OCPP_Status.Cp.Unavailable,
        nowValue: -1,
        startWh: -1,
        startTime: "",//YYYY-MM-DD HH:mm:ss
        nowTime: moment()
    },
    watch: {
        //status: function (n, o) {
        //    if (n != OCPP_Status.Cp.Charging) {
        //        vue.data.startWh = -1;
        //        vue.data.startTime = null;
        //    }
        //},
        //nowValue: function (n, o) {
        //    switch (vue.data.status) {
        //        case OCPP_Status.Cp.Charging:
        //            if (vue.data.startWh == -1) {
        //                vue.data.startWh = n
        //                vue.data.startTime = moment()
        //            }
        //            break;
        //        default:
        //    }
        //}

    },
    methods: {
        

    },
    filters: {

    },
    computed: {
        chargeTitle() {
            switch (vue.data.status) {
                case OCPP_Status.Cp.Charging:
                    return "充電中"
                case OCPP_Status.Cp.Finishing:
                    return "充電完成"
                case OCPP_Status.Cp.Available:
                    return "請插入充電樁"
                default:
                    return "無法充電"
            }
        },
        chargeImg() {
            switch (vue.data.status) {
                case OCPP_Status.Cp.Charging:
                    return "充電中.gif"
                case OCPP_Status.Cp.Finishing:
                    return "充電完成.png"
                case OCPP_Status.Cp.Available:
                    return "充電槍插入.gif"
                    //return "充電槍插入.svg"
                default:
                    return "無法充電.png"
            }
        },
        chargeValue() {
            if (vue.data.nowValue == -1)
                return "- -"
            return `${round((vue.data.nowValue - vue.data.startWh) / 1000, 2)} kwh`
        },
        chargeDuring() {
            if (vue.data.startTime == "")
                return '- -'
            var diff = vue.data.nowTime.diff(moment(vue.data.startTime, 'YYYY-MM-DD HH:mm:ss'))

            return moment.utc(diff).format("HH:mm:ss")
            //moment(moment.duration(now.diff(then))).format("hh:mm:ss")
            //return moment.utc(moment.duration(diff)).format("HH:mm:ss")
        }
    },
    mounted() {

    },
    created() {
        var count=0
        setInterval(() => {
            vue.data.nowTime = moment()
        }, 1000)
    }
}

export { vue }


