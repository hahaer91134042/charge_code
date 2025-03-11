

class SysMenuItem {
    Id = 0
    ParentId = 0
    GroupLv = -1
    Name = ''
    Path = ''
    Type = 0
    beEnable = true
    Sort = 0
    Parent = {}
    constructor(menu) {
        if (menu) {
            this.Id = menu.Id
            this.ParentId = menu.ParentId
            this.GroupLv = menu.GroupLv
            this.Name = menu.Name
            this.Path = menu.Path
            this.Type = menu.Type
            this.beEnable = menu.beEnable
            this.Sort = menu.Sort
        }
    }


    //轉換
    toReq() {
        return {
            id: this.Id,
            parentId: this.Parent.Id ? this.Parent.Id : this.ParentId,
            name: this.Name,
            path: this.Path,
            groupLv: this.GroupLv,
            type: this.Type,
            sort: this.Sort,
            beEnable: this.beEnable
        }
    }
}

var vue = {

    el: '#sysMenuDiv',

    data: {
        show: false,
        sysGroup: [],
        sysMenu: [],
        //測試用
        //sysMenu: [
        //    {
        //        "Id": 1, "ParentId": 0, "GroupLv": 0, "Name": "平台管理",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 2, "ParentId": 0, "GroupLv": 10, "Name": "系統管理",
        //        "Path": "", "Type": 1, "beEnable": true, "Sort": 1
        //    },
        //    {
        //        "Id": 5, "ParentId": 2, "GroupLv": 10, "Name": "群組權限管理",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 2
        //    },
        //    {
        //        "Id": 6, "ParentId": 2, "GroupLv": 10, "Name": "單元管理",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 3
        //    },
        //    {
        //        "Id": 7, "ParentId": 2, "GroupLv": 10, "Name": "帳號管理",
        //        "Path": "/Sys/Account", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 8, "ParentId": 2, "GroupLv": 10, "Name": "群組管理",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 1
        //    },
        //    {
        //        "Id": 9, "ParentId": 5, "GroupLv": 10, "Name": "測試單元",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 10, "ParentId": 8, "GroupLv": 10, "Name": "測試2",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 11, "ParentId": 1, "GroupLv": 10, "Name": "平台測試1",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 12, "ParentId": 1, "GroupLv": 10, "Name": "平台測試2",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 13, "ParentId": 11, "GroupLv": 10, "Name": "平台測試1_1",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    },
        //    {
        //        "Id": 14, "ParentId": 11, "GroupLv": 10, "Name": "平台測試1_2",
        //        "Path": "", "Type": 0, "beEnable": true, "Sort": 0
        //    }
        //],

        addNewMenu: new SysMenuItem(),
        selectMenu: new SysMenuItem()
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
            NProgress.start()

            eki.req.SysMenu.post()
                .then(result => {
                    NProgress.done()
                    var resp = eki.resp(result)
                    //console.log(`sys menu->${resp.jsonString()}`)
                    vue.data.sysMenu = resp.info

                }).catch(ex => {
                    NProgress.done()
                })

        },

        menuList(pID) {
            //console.log(`menu list pID->${pID}`)
            return _.orderBy(vue.data.sysMenu.filter(m => m.ParentId == pID), "Sort", 'asc')
        },

        addMenu(parentMenu) {
            //pID=menu parentID          

            //console.log(`addMenu pID->${pID}`)

            //let parentMenu = set.data.sysMenu.first(m => m.Id == pID)

            vue.data.addNewMenu = new SysMenuItem()
            vue.data.addNewMenu.Parent = parentMenu ? parentMenu : new SysMenuItem()

            console.log(`parent menu->${vue.data.addNewMenu.Parent.Name}`)

            eki.popup('#add_menu_view')
                .open()
        },
        editMenu(menu) {
            //let menu = set.data.sysMenu.first(m => m.Id == mID)
            //console.log(`select menu->${menu.jsonString()}`)

            let parentMenu = vue.data.sysMenu.first(m => m.Id == menu.ParentId)

            vue.data.selectMenu = new SysMenuItem(menu)
            vue.data.selectMenu.Parent = parentMenu ? parentMenu : new SysMenuItem()

            eki.popup('#edit_menu_view')
                .open()
        },
        onEditMenu() {
            //console.log(`edit select menu->${set.data.selectMenu.jsonString()}`)

            NProgress.start()
            let param = vue.data.selectMenu.toReq()
            eki.req.EditSysMenu.post(param)
                .then(result => {
                    NProgress.done();
                    var resp = eki.resp(result);

                    vue.data.sysMenu = resp.info;
                    $.colorbox.close();

                }).catch(ex => {
                    NProgress.done()
                })

        },
        onDeleteMenu(menu) {
            //console.log(`delete menu id->${mID}`)
            //var menu = set.data.sysMenu.first(m => m.Id == mID)

            var yes = confirm(`確定要刪除 "${menu.Name}" 單元?`)

            if (yes) {
                console.log(`刪除單元`)
                NProgress.start()
                var param = { id: menu.Id }
                eki.req.DelSysMenu.post(param)
                    .then(result => {
                        NProgress.done()
                        let resp = eki.resp(result)
                        vue.data.sysMenu = resp.info
                    }).catch(ex => {
                        NProgress.done()
                    })
            } 

        },

        onAddNewMenu() {

            NProgress.start()

            let param = vue.data.addNewMenu.toReq();

            eki.req.AddSysMenu.post(param)
                .then(result => {
                    NProgress.done();

                    let resp = eki.resp(result)

                    vue.data.sysMenu = resp.info

                }).catch(ex => {
                    NProgress.done();
                    alert(ex.message)
                })

            $.colorbox.close();

            //測試用
            //var lastMenu = _.orderBy(set.data.sysMenu, 'Id', 'desc').first()

            //console.log(`lastMenu->${lastMenu.jsonString()}`)

            //console.log(`add new menu->${set.data.addNewMenu.jsonString()}`)
            //let newMenu = {
            //    Id: lastMenu.Id + 1,
            //    ParentId: set.data.addNewMenu.Parent.Id ? set.data.addNewMenu.Parent.Id : 0,
            //    Name: set.data.addNewMenu.Name,
            //    GroupLv: set.data.addNewMenu.GroupLv,
            //    Type: set.data.addNewMenu.Type,
            //    beEnable: true,
            //    Sort: set.data.addNewMenu.Sort
            //}
            //set.data.sysMenu.push(newMenu)
        }
    },
    filters: {
        menuDel: function (mID) {
            return vue.data.sysMenu.filter(m => m.ParentId == mID).length>0
        }
    },
    computed: {

    },
    mounted() {
        eki.req.SysGroup.post()
            .then(result => {
                let resp = eki.resp(result)
                vue.data.sysGroup = resp.info;
                //set.data.sysGroup.push({ Title: '無', Describe: '無', Lv: -1 })
                //console.log(`menu group->${set.data.sysGroup.jsonString()}`)
            }).catch(ex => {

            })
    },
    created() {
        console.log('SysMenu create')
    }
}

export { vue }


