import * as sortThead from "../../../view/sortthead.js";
import * as tablePaginate from "../../../view/tablepaginate.js";


class SysUser {
    id=0
    acc=''
    name = ''
    pwd = ''
    phone = ''
    email = ''
    lv=-1
    beEnable=true

    constructor(user) {
        if (user) {
            this.id = user.Id
            this.acc = user.Account
            this.name = user.Name
            this.pwd = user.Pwd
            this.phone = user.Phone
            this.email = user.Email
            this.lv = user.Lv
            this.beEnable = user.beEnable
        }
    }
}

var vue = {

    el: '#sysAccDiv',

    data: {
        title:'',
        show: false,
        sysGroup:[],
        sysUser:[],
        //測試用
        //sysUser: [
        //    { Id: 1, Account: 'Test1', Name: "1111" },
        //    { Id: 2, Account: 'Test2', Name: "1112" },
        //    { Id: 3, Account: 'Test3', Name: "1113" },
        //    { Id: 4, Account: 'Test4', Name: "1114" },
        //    { Id: 5, Account: 'Test5', Name: "1115" },
        //    { Id: 6, Account: 'Test6', Name: "1116" },
        //    { Id: 7, Account: 'Test7', Name: "1117" }
        //],
        filterData:[],

        tableColumns: [
            { title: '編號', key: 'Id', class: 'col-1 text-capitalize th_click', sort: true, default: true },
            { title: '帳號', key: 'Account', class: 'col-md-2 text-capitalize th_click', sort: true },
            { title: '名稱', key: 'Name', class: 'col-md-2 text-capitalize th_click', sort: true },
            { title: '權限等級', key: 'Lv', class: 'col-md-2 text-capitalize th_click', sort: true },
            { title: '建立時間', key: 'cDate', class: 'col-lg-2 text-capitalize th_click', sort: true },
            { title: '操作', class:'col-1 text-capitalize',sort:false}
        ],

        newUser: new SysUser(),
        editUser: new SysUser()
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
            console.log(`sysUser length->${vue.data.sysUser.length}`)

            NProgress.start()
            eki.req.GetSysUser.post()
                .then(result => {
                    NProgress.done()
                    let response = eki.resp(result)
                    //console.log(`GetSysUser->${response.jsonString()}`)
                    vue.data.sysUser = response.info

                }).catch(ex => {
                    NProgress.done()
                })
        },
        onUserSort(data) {
            vue.data.sysUser = data;
        },
        onChangePage(data) {
            vue.data.filterData = data;
        },
        onEditUser(user) {
            //console.log(`edit user->${JSON.stringify(user)}`);
            vue.data.editUser = new SysUser(user);

            eki.popup("#edit_acc_view").open()

            //var colorboxSetting = {
            //    inline: true, width: "50%",
            //    height: "80%", top: 0, open: true,
            //    href: "#edit_acc_view", overlayClose: false,
            //    escKey: false, closeButton: false, trapFocus: false
            //};
            //$.colorbox(colorboxSetting);

        },
        onAddNewAcc() {

            vue.data.newUser = new SysUser();

            eki.popup("#add_acc_view").open()

            //var colorboxSetting= {
            //    inline: true, width: "50%",
            //    height: "80%", top: 0, open: true,
            //    href: "#add_acc_view", overlayClose: false,
            //    escKey: false, closeButton: false, trapFocus: false
            //};
            //$.colorbox(colorboxSetting);
        },
        addNewUser() {

            NProgress.start()

            eki.req.AddSysUser.post(vue.data.newUser)
                .then(result => {
                    NProgress.done()
                    var res = eki.resp(result)

                    //console.log(`Add user result->${res.jsonString()}`)
                    if (res.success) {
                        vue.data.sysUser = res.info;
                        $.colorbox.close()
                        vue.data.newUser = new SysUser()
                    } else {
                        alert(res.message)
                    }

                }).catch(ex => {
                    NProgress.done()
                    alert(ex.message)
                    $.colorbox.close()
                })

        },
        startEditUser() {

            NProgress.start()
            eki.req.EditSysUser.post(vue.data.editUser)
                .then(result => {
                    NProgress.done();
                    let resp = eki.resp(result)
                    //console.log(`edit user resp->${resp.jsonString()}`)
                    if (resp.success) {
                        vue.data.sysUser = resp.info;
                        $.colorbox.close();
                    } else {
                        alert(resp.message)
                    }

                }).catch(ex => {
                    NProgress.done();
                    alert(ex.message)
                })

        }
    },
    computed: {
        checkEditEmail() {
            //console.log(`check mail->${set.data.editUser.email}  valid->${set.data.editUser.email.isEmail()}`)
            return vue.data.editUser.email.isEmail()
        },
        checkAddEmail() {
            return vue.data.newUser.email.isEmail()
        }
    },

    mounted() {

        eki.pwd.initView($('#sysAccountPwdGroup'))

        eki.req.SysGroup.post()
            .then(result => {
                let resp = eki.resp(result)
                vue.data.sysGroup = resp.info;
                //set.data.sysGroup.push({ Title: '無', Describe: '無', Lv: -1 })
                //console.log(`Sys group->${set.data.sysGroup.jsonString()}`)
            }).catch(ex => {

            })
    },
    created() {
        console.log('SysAcc vue create')        

    }
}

function setPwdEnent() {
    $('#see_pwd').click(function () {
        console.log('eye click')
        var input = $('#edit_loginPwd');
        //console.log(`input type->${input.attr('type')}`)

        if (input.attr('type') === 'password') {
            input.attr('type', 'text')
            $('#icon_eye').removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            input.attr('type', 'password')
            $('#icon_eye').removeClass('fa-eye-slash').addClass('fa-eye');
        }
    })
}


export { vue }


