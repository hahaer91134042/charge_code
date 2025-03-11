


var vue = new VueController({

    el: '#MeterDiv',

    data: {
        MeterData: new ArrayList()


    },
    watch: {


    },
    methods: {
        onMeterSelect(item) {
            console.log(`on meter select->${item.jsonString()}`)
            Session.setJson(item)
            vue.main.open(CpsHref.MeterSerial(item.MeterSerial))
            
        }
    },
    filters: {

    },
    computed: {
        meterList() {

            return vue.data.MeterData.items
        },
        noPayOrder() {
            let orders = []
            vue.data.MeterData.forEach(m => {
                //console.log(`find order->${m.findOrder(['Status', 1]).jsonString()}`)
                orders = orders.concat(m.findOrder(['Status', 1]))
            })
            return orders
        }
    },
    mounted() {

    },
    created() {
        //console.log(`Meter creat  Time->${eki.time(moment())}`)
        //console.log(`Time span->${timeSpan(new TimeRange('2022-05-23 00:00:00', eki.time(moment())), TimeUnit.Day)}`)
        //console.log(`Time duration->${duration(3600, TimeUnit.Sec)}`)

        let now = moment()

        sendReq(eki.req.CpsMeterOrder.post({ time: eki.time(now) }), {
            success(info) {
                vue.data.MeterData.addAll(info.map(m => new MeterOrder(m)))
                console.log(`Meter Order->${vue.data.MeterData.jsonString()}`)
            },
            fail(code, msg) {
                alert(msg)
            }
        })

    }
})

export { vue }


