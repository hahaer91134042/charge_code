import * as viewData from '../../view/viewdata.js';
 

var vue = {

    el: '#MeterDiv',

    data: {
        community: new Community(),
        meter: new ElectricMeter(),
        cpList: new ArrayList(),
        cpAuth: new ArrayList([
            new CpAuth({ Id: 1, CpSerial: '111', Auth: 'dsfasdf', beEnable: true, cDate: '2022-04-19 12:00:00' }),
            new CpAuth({ Id: 2, CpSerial: '222', Auth: 'dsfasdf', beEnable: true, cDate: '2022-04-19 12:00:00' }),
            new CpAuth({ Id: 3, CpSerial: '333', Auth: 'dsfasdf', beEnable: true, cDate: '2022-04-19 12:00:00' }),
            new CpAuth({ Id: 4, CpSerial: '444', Auth: 'dsfasdf', beEnable: true, cDate: '2022-04-19 12:00:00' }),
            new CpAuth({ Id: 5, CpSerial: '555', Auth: 'dsfasdf', beEnable: true, cDate: '2022-04-19 12:00:00' }),
            new CpAuth({ Id: 6, CpSerial: '666', Auth: 'dsfasdf', beEnable: false, cDate: '2022-04-19 12:00:00' }),
        ]),
        //cpList: new ArrayList([
        //    new ChargePoint({ Id: 0, Serial: '1111', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '222', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '3333', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '4444', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '5555', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '666', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '777', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '888', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '999', beEnable: true, cDate: '2022-04-19 12:00:00' }),
        //    new ChargePoint({ Id: 0, Serial: '0000', beEnable: true, cDate: '2022-04-19 12:00:00' })
        //]),

        searchCp: '',
        searchAuth:'',

        selectCp:null,
        newAddCp: new ChargePoint(),
        newAddAuth: new CpAuth()
    },
    watch: {


    },
    methods: {
        initData(data) {
            console.log(`Meter detail data->${data.jsonString()}`)
            vue.data.community = new Community(data.Community)
            vue.data.meter = new ElectricMeter(data.Meter)
            vue.data.cpList.addAll(data.CP.map(c => new ChargePoint(c)))
        },
        onSelectCp(item) {
            vue.data.newAddAuth = new CpAuth({ CpSerial: item.Serial, Auth:'', beEnable:true})

            let param = new ChargePoint(item).toReq()
            sendReq(eki.req.GetCpAuth.post(param), {
                success(info) {
                    vue.data.selectCp = new ChargePoint(item)
                    vue.data.cpAuth.clear()
                    vue.data.cpAuth.addAll(info.map(a => new CpAuth(a)))
                },
                fail(code, msg) {

                }
            })
        },
        onAddNewCp() {
            let param = vue.data.newAddCp.toReq(vue.data.meter.Id)
            //console.log(`add cp param->${param.jsonString()}`)
            sendReq(eki.req.AddMeterCp.post(param), {
                success(info) {
                    //console.log(`add new cp info->${info.jsonString()}`)
                    showToast({msg:'新增充電樁完成!'})
                    vue.data.newAddCp = new ChargePoint()
                    vue.data.cpList.clear()
                    vue.data.cpList.addAll(info.map(c => new ChargePoint(c)))
                },
                fail(code, msg) {

                }
            })
        },
        onAddNewAuth() {
            let param = vue.data.newAddAuth.toReq()
            console.log(`on add new Auth->${param.jsonString()}`)

            sendReq(eki.req.AddCpAuth.post(param), {
                success(info) {
                    vue.data.newAddAuth.Auth = ''
                    vue.data.newAddAuth.beEnable = true

                    vue.data.cpAuth.clear()
                    vue.data.cpAuth.addAll(info.map(a => new CpAuth(a)))
                },
                fail(code, msg) {

                }
            })
        },
        onRemoveAuth(item) {
            let yes = confirm(`確定要刪除 ${item.Auth} 這個卡片?\n刪除之後將無法回復`)
            if (yes) {
                let param = new CpAuth(item).toReq()
                sendReq(eki.req.RemoveCpAuth.post(param), {
                    success(info) {
                        showToast({ msg: '刪除卡片完成!' })
                        vue.data.cpAuth.clear()
                        vue.data.cpAuth.addAll(info.map(a => new CpAuth(a)))
                    },
                    fail(code, msg) {

                    }
                })
            }
        },
        onRemoveCp(item) {
            let yes = confirm(`確定要刪除 ${item.Serial} 這個充電樁?\n刪除之後將無法回復`)
            if (yes) {
                var param = new ChargePoint(item).toReq(vue.data.meter.Id)
                //console.log(`remove cp param->${param.jsonString()}`)
                sendReq(eki.req.RemoveMeterCp.post(param), {
                    success(info) {
                        console.log(`remove info->${info.jsonString()}`)
                        showToast({ msg: '刪除充電樁完成!' })
                        vue.data.cpList.clear()
                        vue.data.cpList.addAll(info.map(c => new ChargePoint(c)))
                    },
                    fail(code, msg) {

                    }
                })
            }
        },
        onEditAuth(item) {
            console.log(`on edit auth ->${item.Auth}`)
            let param = new CpAuth(item).toReq()
            sendReq(eki.req.EditCpAuth.post(param), {
                success(info) {
                    showToast({ msg: '修改卡片完成!' })
                },
                fail(code, msg) {

                }
            })
        },
        onEditCp(item) {
            var param = new ChargePoint(item).toReq(vue.data.meter.Id)
            sendReq(eki.req.EditMeterCp.post(param), {
                success(info) {
                    //console.log(`edit info->${info.jsonString()}`)
                    showToast({ msg: '修改充電樁完成!' })
                    vue.data.cpList.clear()
                    vue.data.cpList.addAll(info.map(c => new ChargePoint(c)))
                },
                fail(code, msg) {

                }
            })

        }

    },
    filters: {

    },
    computed: {
        findCp() {
            if (vue.data.searchCp.length > 0)
                return vue.data.cpList.filter(c => c.Serial.includes(vue.data.searchCp))
            return vue.data.cpList.items
        },
        findAuth() {
            if (vue.data.searchAuth.length > 0)
                return vue.data.cpAuth.filter(c => c.Auth.includes(vue.data.searchAuth))
            return vue.data.cpAuth.items
        },
    },
    mounted() {

    },
    created() {

    }
}



$(function () {

    eki.vue(vue)

})


