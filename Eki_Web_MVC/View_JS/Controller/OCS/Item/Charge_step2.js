
let event = {
    //通知外部這step完成
    onFinish: function (chargeMin) {

    }
}

var vue = new VueController({

    el: '#charge_step2',

    data: {
        show: false,
        chargeType: ChargeType.Time,
        chargeConfig: new ChargeConfig(),

        chargeMin: 0,
        chargeDegree:0,

    },
    watch: {
        show: function (n, o) {
            if (n)
                vue.methods.onSelect()
        }

    },
    methods: {
        //menu被選擇到了觸發
        onSelect() {


        },

        onNext() {

            
            if (vue.data.chargeType == ChargeType.Time) {
                if (vue.data.chargeMin < 1) {
                    alert("請先輸入時間")
                    return
                }
                vue.data.show = false
                event.onFinish(parseInt(vue.data.chargeMin))
            } else {
                if (vue.data.chargeDegree < 1) {
                    alert("請先輸入度數")
                    return
                }
                vue.data.show = false
                event.onFinish(degreeToMin(vue.data.chargeDegree))
            }
        }
    },
    filters: {

    },
    computed: {
        payByMin() {
            return round((vue.data.chargeMin / 60) * vue.data.chargeConfig.Price, 0)
        },
        payByDegree() {
            return round((degreeToMin(vue.data.chargeDegree) / 60) * vue.data.chargeConfig.Price, 0)
        },
        degreeTime() {
            return degreeToMin(vue.data.chargeDegree)
        }
    },
    mounted() {
        //這邊不知為何 無法用id處理 jquery會抓不到 只能用這方式 
        $('.numpicker').numbercontrol({
            'onAfterSetNewvalue': function (that, event, container, value) {
                //console.log(`num value->${value}`)
                if (vue.data.chargeType == ChargeType.Time) {
                    vue.data.chargeMin = value
                } else {
                    vue.data.chargeDegree=value
                }                
            }
        });


    },
    created() {

    }
})

export { vue, event }


