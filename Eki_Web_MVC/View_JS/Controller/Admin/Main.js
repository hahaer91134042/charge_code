
import * as sysAcc from './Item/SysAcc.js';
import * as sysMenu from "./Item/SysMenu.js";
import * as sysGroup from "./Item/SysGroup.js";
import * as cpsCommunity from "./item/CpsCommunity.js";



class MenuItem {
    //這邊model=viewmodel
    constructor(name, m, def = false) {
        this.name = name;
        this.model = m;
        this.model.data.title = name
        this.model.data.show = def
    }
}

class MenuControl {
    //依照需要新增跟修改
    leftMenu = [
        new MenuItem('帳號管理', sysAcc.vue),
        new MenuItem('單元管理', sysMenu.vue),
        new MenuItem('群組管理', sysGroup.vue),
        new MenuItem('停車場列表', cpsCommunity.vue, true)
    ]

    constructor() {
    }

    //addMenu(menu) {
    //    if (typeof menu != typeof MenuItem)
    //        throw new Exception("menu type error")

    //    this.leftMenu.push(menu)
    //}

    filterItemInView(menuList) {
        //var curMenu = menuList.map(name => this.leftMenu.first(m => m.name == name))
        var curMenu = this.leftMenu.filter(m => menuList.some(name => m.name == name))
        /*this.leftMenu = curMenu*/
        return curMenu
    }

    showMenu(key) {
        this.leftMenu.forEach(item => {
            if (item.name == key) {
                item.model.data.show = true
            } else {
                item.model.data.show = false
            }
        })
    }
}




let menuControl = new MenuControl();


function InitMenu(menuLink) {


    //生成初始化的menu data
    //menu.each(function (i, item) {
    //    var name = $(item).text()
    //    console.log(`menu index->${i}  element->${name}`)


    //    menuControl.addMenu(new MenuItem())

    //})

    menuLink.click(function () {

        let option = $(this).text()
        //console.log(`menu click ->${option}`)        

        //更新目前選擇到的menu
        //mainVue.data.leftMenu = mainVue.data.leftMenu.map(item => {
        //    if (item.name == option)
        //        return new MenuItem(item.name, true)
        //    else
        //        return new MenuItem(item.name, false)
        //})

        $('#path').html(`導覽列:<span>${option}</span>`)

        //之後再去掉
        switch (option) {
            case '帳號管理':
            case '單元管理':
            case '群組管理':
            case '停車場列表':
                menuControl.showMenu(option)
                //mainVue.methods.onMenuSelect(option)
                break;
            default:
                alert('該選項目前無法使用!')
        }

        //關閉側邊攔
        $('.sidenav').width(0)

    })

    console.log(`menu  size->${menuLink.length}`)

}

//這是共用功能
let main = {
    //主要是初始化一些通用設定
    init() {
        //$('.toast').toast({
        //    animation: true,
        //    autohide: true,
        //    delay:5000
        //})
    },

    showToast(option) {
        new EkiToast(option).show()
    }
}


$(function () {

    main.init()

    NProgress.configure({
        parent: '#MainDiv'
    });

    let menuLink = $('.menu-link')

    InitMenu(menuLink)



    let menuOptions = []

    menuLink.each(function (i, item) {
        menuOptions.push($(item).text())
    })

    console.log(`Main  menuOptions->${menuOptions.jsonString()}`)

    //init vue control
    menuControl
        //.leftMenu
        .filterItemInView(menuOptions)
        .forEach(m => {
            m.model.main = main;
            eki.vue(m.model)
        })



})