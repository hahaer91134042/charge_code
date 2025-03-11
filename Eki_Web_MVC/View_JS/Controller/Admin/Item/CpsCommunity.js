


var vue = {

    el: '#cpsCommunityDiv',

    data: {
        show: true,
        //測試用
        //cpsCommunity: new ArrayList([
        //    new Community({ Id: 1, Name: '社區1', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: true, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區1', Serial: '123456', Account: 'BankAccount', Code: '123', Sub: '45' }, Meter: [{ "Id": 1, "PaySerial": "456789123", "MeterSerial": "123456789", "cDate": "/Date(1649670225873)/", "beEnable": true }] }),
        //    new Community({ Id: 2, Name: '社區2', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: true, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區2', Serial: '123456', Account: 'myAccount', Code: '123', Sub: '45' } }),
        //    new Community({ Id: 3, Name: '社區3', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: true, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區3', Serial: '123456', Account: 'myAccount', Code: '123', Sub: '45' } }),
        //    new Community({ Id: 4, Name: '社區4', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: true, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區4', Serial: '123456', Account: 'myAccount', Code: '123', Sub: '45' } }),
        //    new Community({ Id: 5, Name: '社區5', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: true, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區5', Serial: '123456', Account: 'myAccount', Code: '123', Sub: '45' } }),
        //    new Community({ Id: 6, Name: '社區6', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: true, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區6', Serial: '123456', Account: 'myAccount', Code: '123', Sub: '45' } }),
        //    new Community({ Id: 7, Name: '社區7', Pwd: '123456', LoginIP: '192.168.1.1', beEnable: false, cDate: '2020-03-31 12:00:00', Address: { Code: '23579', Detail: '106台北市大安區敦化南路1段246號6樓' }, Bank: { Name: '社區7', Serial: '123456', Account: 'myAccount', Code: '123', Sub: '45' } })
        //]),

        cpsCommunity: new ArrayList(),

        searchName: '',
        searchLoc: '',
        searchHouse: '',
        searchCp: "",

        selectCommunity: new Community(),
        selectLoc: new ArrayList(),
        //selectLoc: new ArrayList([
        //    { Id: 1, CpSerial: "serial1", Info: { Content: "車位_1" }, cDate: '2022-04-15 12:00:00', beEnable: true },
        //    { Id: 2, CpSerial: "serial2", Info: { Content: "車位_2" }, cDate: '2022-04-15 12:00:00', beEnable: true },
        //    { Id: 3, CpSerial: "serial3", Info: { Content: "車位_3" }, cDate: '2022-04-15 12:00:00', beEnable: true },
        //    { Id: 4, CpSerial: "serial4", Info: { Content: "車位_4" }, cDate: '2022-04-15 12:00:00', beEnable: true },
        //    { Id: 5, CpSerial: "serial5", Info: { Content: "車位_5" }, cDate: '2022-04-15 12:00:00', beEnable: true }
        //]),
        selectChargeConfig: new ChargeConfig(),
        selectHouse: new ArrayList(),
        selectCp: new ArrayList(),

        newAddCp: new ChargePoint(),
        newAddLoc: new CpsLocation(),
        newAddHouse: new CpsHouse(),
        newAddCommunity: new Community()
    },
    watch: {
        show: function (n, o) {
            if (n)
                vue.methods.onSelect()
        },
        selectCommunity: function (n, o) {
            if (n) {
                //刷新
                vue.data.newAddHouse = new CpsHouse()
                vue.data.newAddLoc = new CpsLocation()
                vue.data.newAddCp = new ChargePoint()

                //下載該社區車位
                sendReq(eki.req.GetAdminLocation.post({ cID: n.Id }), {
                    success(info) {
                        vue.data.selectLoc = new ArrayList(info.map(loc => new CpsLocation(loc)))
                        console.log(`get loc->${vue.data.selectLoc.jsonString()}`)
                        //console.log(`selectLoc->${vue.data.selectLoc.jsonString()}`)
                    },
                    fail(code, msg) {

                    }
                }, false)
                //下載該社區住戶資料
                sendReq(eki.req.GetAdminHouse.post({ cID: n.Id }), {
                    success(info) {
                        console.log(`get house->${info.jsonString()}`)
                        vue.data.selectHouse.clear()
                        vue.data.selectHouse.addAll(info.map(h => new CpsHouse(h)))
                    },
                    fail(code, msg) {

                    }
                }, false)
                //下載該社區充電樁
                sendReq(eki.req.GetCommunityCp.post({ cID: n.Id }), {
                    success(info) {
                        vue.data.selectCp.clear()
                        vue.data.selectCp.addAll(info.map(c => new ChargePoint(c)))
                        console.log(`get community cp->${vue.data.selectCp.jsonString()}`)
                    },
                    fail(code, msg) {

                    }
                }, false)

            }
        }

    },
    methods: {
        //menu被選擇到了觸發
        onSelect() {

            sendReq(eki.req.GetAdminCommunity.post(), {
                success(info) {
                    console.log(`init community->${info.jsonString()}`)
                    vue.data.cpsCommunity.clear()
                    vue.data.cpsCommunity.addAll(info.map(c => new Community(c)))
                },
                fail() {

                }
            })

        },

        onSelectCommunity(com) {
            console.log(`on select community->${com.jsonString()}`)
            //com.select = true
            //set.data.selectCommunity = com
            vue.data.selectCommunity = new Community(com)
            //set.data.cpsCommunity.forEach(c => {
            //    if (c.Id != com.Id)
            //        c.select = false
            //})

        },
        onEditSelectCommunity() {

            let param = vue.data.selectCommunity.toReq()

            //console.log(`onEditSelectCommunity() param->${param.jsonString()}`)
            sendReq(eki.req.EditAdminCommunity.post(param), {
                success(info) {
                    vue.data.cpsCommunity.clear()
                    vue.data.cpsCommunity.addAll(info.map(c => new Community(c)))
                    //清空
                    vue.data.selectCommunity = new Community()
                },
                fail() {

                }
            })

        },
        showMeterDetail(item) {
            eki.href.MeterDetail(item.PaySerial)
        },
        //點擊加入地點
        onAddNewLoc() {
            let param = vue.data.newAddLoc.toAddReq(vue.data.selectCommunity.Id)

            console.log(`onAddNewLoc->${param.jsonString()}`)

            sendReq(eki.req.AddAdminLocation.post(param), {
                success(info) {
                    vue.data.selectLoc.clear()
                    vue.data.selectLoc.addAll(info)
                    vue.data.newAddLoc = new CpsLocation()
                    vue.main.showToast({
                        msg: '該車位已新增完成!'
                    })
                },
                fail(code, msg) {

                }
            })

        },
        //修改選擇社區的車位
        onEditLocation(item) {
            let param = new CpsLocation(item).toReq(vue.data.selectCommunity.Id)

            sendReq(eki.req.EditAdminLocation.post(param), {
                success(info) {
                    vue.data.selectLoc.clear()
                    vue.data.selectLoc.addAll(info)
                    vue.main.showToast({
                        msg: '該車位已修改完成!'
                    })
                },
                fail(code, msg) {

                }
            })

        },
        onRemoveSelectLoc(item) {
            let yes = confirm(`確定要刪除 ${item.Info.Content} 這個電錶?\n 刪除之後將無法回復`)

            if (yes) {
                let param = new CpsLocation(item).toReq(vue.data.selectCommunity.Id)
                sendReq(eki.req.RemoveAdminLocation.post(param), {
                    success(info) {
                        vue.data.selectLoc.clear()
                        vue.data.selectLoc.addAll(info)
                        vue.main.showToast({
                            msg: '該車位已刪除完成!'
                        })
                    },
                    fail(code, msg) {

                    }
                })
            }
        },
        onSelectChargeConfig(locId) {
            let config = vue.data.selectLoc.first(l => l.Id == locId).Config
            config.LocationId = locId
            console.log(`select charge config->${config.jsonString()}`)
            vue.data.selectChargeConfig = new ChargeConfig(config)
        },
        onSetChargeConfig() {
            let param = vue.data.selectChargeConfig.toReq()
            console.log(`set charge config->${param.jsonString()}`)

            sendReq(eki.req.SetChargeConfig.post(param), {
                success(info) {
                    vue.data.selectLoc.clear();
                    vue.data.selectLoc = new ArrayList(info.map(loc => new CpsLocation(loc)))
                    vue.data.selectChargeConfig = new ChargeConfig()

                    showToast({ msg: '設定完成' })
                },
                fail(code, msg) {
                    showToast({ msg: msg })
                }
            })

        },
        //點擊加入選擇的社區電表
        addSelectMeter() {
            vue.data.selectCommunity.Meter.add(new ElectricMeter())
        },
        onAddSelectMeter() {
            if (vue.data.selectCommunity.Meter.filter(m => m.Id == 0).length < 1) {
                alert('沒有可新增電錶')
                return;
            }

            let param = vue.data.selectCommunity.toAddMeterReq()

            sendReq(eki.req.AddAdminMeter.post(param), {
                success(info) {
                    vue.data.cpsCommunity.clear()
                    vue.data.cpsCommunity.addAll(info.map(c => new Community(c)))

                    let oldSelect = vue.data.selectCommunity

                    vue.data.selectCommunity = new Community(vue.data.cpsCommunity.first(c => c.Id == oldSelect.Id))
                },
                fail() {

                }
            })

        },
        onEditSelectMeter(item) {
            console.log(`edit select meter->${item.jsonString()}`)

            //vue.main.showToast({
            //    msg: item.jsonString()
            //})



            var com = new Community(vue.data.selectCommunity)
            com.Meter.clear()
            com.Meter.add(new ElectricMeter(item))
            let param = com.toReq()

            sendReq(eki.req.EditAdminMeter.post(param), {
                success(info) {
                    vue.main.showToast({
                        msg: '該電表已修改完成!'
                    })
                },
                fail(code, msg) {
                    vue.main.showToast({
                        msg: '該電表修改失敗!'
                    })
                }
            })

        },
        onRemoveSelectMeter(item) {
            console.log(`remove select meter->${item.jsonString()}`)

            let yes = confirm(`確定要刪除 ${item.PaySerial} 這個電錶?\n 刪除之後將無法回復`)
            if (yes) {
                if (item.Id == 0) {//表示要刪除剛新增的而已
                    vue.data.selectCommunity.Meter.remove(m => m.PaySerial == item.PaySerial)

                } else {//表示要刪除資料庫的

                    //item.PaySerial = '444444444444'

                    var com = new Community(vue.data.selectCommunity)

                    com.Meter.clear()
                    com.Meter.add(new ElectricMeter(item))

                    let param = com.toReq()
                    console.log(`remove meter ->${param.jsonString()}`)

                    sendReq(eki.req.RemoveAdminMeter.post(param), {
                        success(info) {
                            vue.data.cpsCommunity.clear()
                            vue.data.cpsCommunity.addAll(info.map(c => new Community(c)))

                            let oldSelect = vue.data.selectCommunity

                            vue.data.selectCommunity = new Community(vue.data.cpsCommunity.first(c => c.Id == oldSelect.Id))
                        },
                        fail(code, msg) {
                            alert('該電表目前無法刪除\n請確認是否有充電樁在該電表底下!')
                        }
                    })

                }
            }
        },
        onAddNewConnunity() {

            var param = vue.data.newAddCommunity.toReq()
            //console.log(`new community->${param.jsonString()}`)

            sendReq(eki.req.AddAdminCommunity.post(param), {
                success(info) {
                    //console.log(`add Community->${resp.jsonString()}`)
                    vue.data.cpsCommunity.clear()
                    vue.data.cpsCommunity.addAll(info.map(c => new Community(c)))
                    vue.main.showToast({
                        msg: '社區已新增完成!'
                    })
                },
                fail() {

                }
            })

        },
        onAddNewMeter() {
            if (vue.data.newAddCommunity.Meter.length > 0) {
                let m = new ElectricMeter();
                m.Id = vue.data.newAddCommunity.Meter.length
                vue.data.newAddCommunity.Meter.add(m)
            } else {
                vue.data.newAddCommunity.Meter.add(new ElectricMeter())
            }
            //console.log(`add new meter->${set.data.newAddCommunity.Meter.jsonString()}`)
        },
        onRemoveNewMeter(item) {
            //console.log(`onRemoveNewMeter item->${item.jsonString()}`)
            vue.data.newAddCommunity.Meter.remove((m) => m.Id == item.Id)
        },
        onAddNewHouseLoc() {
            var input = $('#newHouseLoc').val().trim()
            //console.log(`new house loc->${input}`)
            if (input.length < 1) {
                showToast({ msg: '序號不得為空值' })
                return
            }

            vue.data.newAddHouse.Loc.add(input)

            $('#newHouseLoc').val('')
        },
        onRemoveNewHouseLoc() {
            var selectVal = $('select[id=newHouseLocSelect] option').filter(':selected').val()
            //console.log(`remove loc->${selectVal}`)
            if (selectVal)
                vue.data.newAddHouse.Loc.remove(s => s === selectVal)
        },
        onAddNewHouse() {
            let param = vue.data.newAddHouse.toReq(vue.data.selectCommunity.Id)
            //console.log(`on add new house->${param.jsonString()}`)

            sendReq(eki.req.AddAdminHouse.post(param), {
                success(info) {
                    showToast({ msg: '新增住戶成功!' })
                    //console.log(`add house ->${info.jsonString()}`)
                    vue.data.newAddHouse = new CpsHouse()
                    vue.data.selectHouse.clear()
                    vue.data.selectHouse.addAll(info.map(h => new CpsHouse(h)))
                },
                fail(code, msg) {

                    showToast({ msg: '新增住戶失敗!' })
                }
            })
        },
        onAddHouseLoc(hId) {
            //console.log(`add house loc id->${hId}`)
            var input = $('#addHouseLoc_' + hId).val().trim()
            //console.log(`add house loc input->${input}`)
            if (input.length < 1) {
                showToast({ msg: '序號不得為空值' })
                return
            }

            vue.data.selectHouse.first(h => h.Id == hId).Loc.add(input)

            $('#addHouseLoc_' + hId).val('')
        },
        onRemoveLoc(hId) {
            var selectVal = $(`select[id=houseLocSelect_${hId}] option`).filter(':selected').val()
            //console.log(`on remove loc->${selectVal}`)

            if (selectVal)
                vue.data.selectHouse.first(h => h.Id == hId).Loc.remove(ser => ser === selectVal)
        },
        onEditHouse(house) {

            //console.log(`on edit house->${house.jsonString()}`)
            var param = new CpsHouse(house).toReq(vue.data.selectCommunity.Id)

            //console.log(`on edit house param->${param.jsonString()}`)

            sendReq(eki.req.EditAdminHouse.post(param), {
                success(info) {
                    showToast({ msg: '修改住戶成功!' })
                    vue.data.selectHouse.clear()
                    vue.data.selectHouse.addAll(info.map(h => new CpsHouse(h)))
                },
                fail(code, msg) {
                    showToast({ msg: '修改住戶失敗!' })
                }
            })
        },
        onRemoveHouse(house) {
            let yes = confirm(`確定要刪除 ${house.Name} 這個住戶?\n刪除之後將無法回復`)
            if (!yes) return

            var param = new CpsHouse(house).toReq(vue.data.selectCommunity.Id)

            sendReq(eki.req.RemoveAdminHouse.post(param), {
                success(info) {
                    showToast({ msg: '刪除住戶成功!' })
                    vue.data.selectHouse.clear()
                    vue.data.selectHouse.addAll(info.map(h => new CpsHouse(h)))
                },
                fail(code, msg) {
                    showToast({ msg: '刪除住戶失敗!' })
                }
            })
        },
        onAddNewCp() {

            let param = vue.data.newAddCp.toReq();
            param.cID = vue.data.selectCommunity.Id

            if (param.serial.length < 1) {
                alert("請輸入序號")
                return
            }

            console.log(`on add new cp->${param.jsonString()}`)

            sendReq(eki.req.AddCommunityCp.post(param), {
                success(info) {
                    console.log(`add New cp info->${info.jsonString()}`)
                    vue.data.newAddCp = new ChargePoint()
                    vue.data.selectCp.clear()
                    vue.data.selectCp.addAll(info.map(c => new ChargePoint(c)))
                    showToast({msg:"充電樁新增完成"})
                },
                fail(code, msg) {
                    showToast({msg:"新增充電樁失敗"})
                }
            })
        },
        onEditCp(cp) {
            console.log(`onEditCp->${cp.jsonString()}`)
            var param = new ChargePoint(cp).toReq()

            sendReq(eki.req.EditCommunityCp.post(param), {
                success(info) {
                    console.log(`edit cp info->${info.jsonString()}`)
                    vue.data.selectCp.clear()
                    vue.data.selectCp.addAll(info.map(c => new ChargePoint(c)))
                    showToast({ msg: "充電樁修改完成" })
                },
                fail(code, msg) {
                    showToast({ msg: "修改充電樁失敗" })
                }
            })

        },
        onRemoveCp(cp) {
            console.log(`onRemoveCp->${cp.jsonString()}`)
            var param = new ChargePoint(cp).toReq()

            sendReq(eki.req.RemoveCommunityCp.post(param), {
                success(info) {
                    console.log(`remove cp info->${info.jsonString()}`)
                    vue.data.selectCp.clear()
                    vue.data.selectCp.addAll(info.map(c => new ChargePoint(c)))
                    showToast({ msg: "充電樁刪除完成" })
                },
                fail(code, msg) {
                    showToast({ msg: "刪除充電樁失敗" })
                }
            })
        }

    },
    filters: {

    },
    computed: {
        findCp() {
            if (vue.data.searchCp.length > 0)
                return vue.data.selectCp.filter(c => c.Serial.includes(vue.data.searchCp))
            return vue.data.selectCp.items
        },
        findHouse() {
            if (vue.data.searchHouse.length > 0)
                return vue.data.selectHouse.filter(h => h.Name.includes(vue.data.searchHouse))
            return vue.data.selectHouse.items
        },
        findLoc() {
            if (vue.data.searchLoc.length > 0)
                return vue.data.selectLoc.filter(loc => loc.SerNum.includes(vue.data.searchLoc))
            return vue.data.selectLoc.items
        },
        findCommunity() {
            if (vue.data.searchName.length > 0)
                return vue.data.cpsCommunity.filter(c => c.Name.includes(vue.data.searchName))
            return vue.data.cpsCommunity.items
        },
        newAddMeter() {

            return _.orderBy(vue.data.newAddCommunity.Meter.items, 'Id', 'desc')
        },
        selectMeter() {

            let list = _.orderBy(vue.data.selectCommunity.Meter.items, 'Id', 'asc')

            return list.map(m => {
                if (m.Id == 0) {
                    m.isNew = true
                } else {
                    m.isNew = false
                }
                return m
            })
        }
    },
    mounted() {
        eki.pwd.initView($('#communityPwdGroup'))
        eki.pwd.initView($('#newPwdGroup'))

        eki.req.GetAdminCommunity
            .post()
            .then(result => {
                let resp = eki.resp(result)

                console.log(`init community->${resp.jsonString()}`)

                resp.check({
                    success(info) {
                        console.log(`resp check success`)
                        let list = info.map(c => new Community(c))
                        vue.data.cpsCommunity.clear()
                        vue.data.cpsCommunity.addAll(list)
                    },
                    fail(code, msg) {
                        console.log(`resp check fail`)
                    }
                })

            }).catch(ex => {
                
            })

        //let list = [{ Id: 1, Name: 'test1' }, { Id: 2, Name: 'test2' }]
        //let newList = new ArrayList(list)

        //console.log(`newList ->${newList.jsonString()}`)
        //console.log(`newList size->${newList.size}`)
        //console.log(`newList latest->${newList.latest.jsonString()}`)
        //console.log(`newList first->${newList.first().jsonString()}`)
        //console.log(`newList id=2 first->${newList.first(v => v.Id == 2).jsonString()}`)

        //newList.remove(v => v.Id == 2)
        //console.log(`newList id=2 remove->${newList.items.jsonString()}`)

        //newList.forEach(item => {
        //    console.log(`1 list item->${item.jsonString()}`)
        //})        

        //for (var item in newList.items) {
        //    console.log(`2 list item->${item.jsonString()}`)
        //}

        //for (var item in list) {
        //    console.log(`3 list item->${item.jsonString()}`)
        //}

    },
    created() {

    }
}

export { vue }


