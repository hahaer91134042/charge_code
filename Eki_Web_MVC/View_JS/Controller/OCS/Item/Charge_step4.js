

let event = {
    //通知外部這step完成
    onFinish: function (invo) {

    }
}

var vue = new VueController({

    el: '#charge_step4',

    data: {
        show: false,
        donateList: [],
        userInvoType: InvoType.ezPay,
        phoneVehicleNum: '',
        cerVehicleNum: '',
        userUBN: '',
        userName:'',
        userAddress:'',
        userDonateCode:'',


        searchDonate:'',
        selectStyle: {
            border: '4px solid',
            borderColor: 'rgb(153 203 81)'
        },
        unSelectStyle: {
            border: '2px solid',
            borderColor: 'rgb(217 217 217)'
        },
        errorStyle: {
            border: '4px solid',
            borderColor: 'red'
        }
    },
    watch: {
        show: function (n, o) {
            if (n)
                vue.methods.onSelect()
        },
        userInvoType: function (n, o) {
            //console.log(`userInvoType new value->${n}`)

            if (n != InvoType.Phone)
                vue.data.phoneVehicleNum = ""
            if (n != InvoType.Certificate)
                vue.data.cerVehicleNum = ""
            if (n != InvoType.Paper)
                vue.data.userUBN = ""

        },
        //userDonateCode: function (n, o) {
        //    $(`#donate_${n}`).css('background-color', 'rgb(127 190 38)')
        //    $(`#donate_${n} span:first`).css('color', 'white')

        //    $(`#donate_${o}`).css('background-color', 'white')
        //    $(`#donate_${o} span:first`).css('color', 'black')
        //}

    },
    methods: {
        //menu被選擇到了觸發
        onSelect() {


        },
        onSelectInvo(type) {
            //$('#invo_vehicle').collapse('hide')
            //$('#invo_ubn').collapse('hide')
            //$('#invo_donate').collapse('hide')

            switch (type) {
                case 0://載具
                    vue.data.userInvoType = InvoType.ezPay
                    $('#invo_vehicle').collapse('show')
                    $('#invo_ubn').collapse('hide')
                    $('#invo_donate').collapse('hide')
                    break;
                case 1://統編(紙本)
                    vue.data.userInvoType = InvoType.Paper
                    $('#invo_vehicle').collapse('hide')
                    $('#invo_ubn').collapse('show')
                    $('#invo_donate').collapse('hide')
                    break;
                case 2://捐贈
                    vue.data.userInvoType = InvoType.Donate
                    $('#invo_vehicle').collapse('hide')
                    $('#invo_ubn').collapse('hide')
                    $('#invo_donate').collapse('show')
                    break;
            }
        },
        onOpenDonate() {
            var config = {
                inline: true, width: "70%",
                height: "60%",
                //top: 0,
                open: true,
                overlayClose: true,
                escKey: false, closeButton: false, trapFocus: false
            }
            eki.popup('#donateDialog', config).open()
        },
        onSelectDonateCode(code) {

            vue.data.userDonateCode = code
            vue.data.searchDonate = ''
            vue.methods.closeColorBox()
        },
        onCheckOut() {
            let invo = {
                type: parseInt(vue.data.userInvoType),
                serial:''
            }

            switch (invo.type) {
                case InvoType.Paper:
                    invo.serial = vue.data.userUBN
                    invo.name = vue.data.userName
                    invo.address = vue.data.userAddress
                    if (!checkCompanyId(invo.serial)) {
                        vue.data.userUBN=""
                        alert('公司統編錯誤')
                        return
                    }
                    if (invo.name.length < 1) {
                        alert('請填入地址')
                        return
                    }
                    if (invo.address.length < 1) {
                        alert('請填入地址')
                        return
                    }
                    break;
                case InvoType.Phone:                    
                    invo.serial = `/${vue.data.phoneVehicleNum}`
                    if (!checkPhoneVehicle(invo.serial)) {
                        vue.data.phoneVehicleNum=""
                        alert(`手機載具錯誤`)
                        return
                    }
                    break;
                case InvoType.Certificate:
                    invo.serial = vue.data.cerVehicleNum
                    if (!checkCerVehicle(invo.serial)) {
                        vue.data.cerVehicleNum=""
                        alert('自然人憑證號碼錯誤')
                        return
                    }
                    break;
                case InvoType.Donate:
                    invo.serial = vue.data.userDonateCode
                    break;
                
            }

            event.onFinish(invo)
        }
    },
    filters: {

    },
    computed: {
        ubnSelect() {
            if (vue.data.userInvoType == InvoType.Paper)
                return true;
            return false;
        },
        donateSelect() {
            if (vue.data.userInvoType == InvoType.Donate)
                return true;
            return false;
        },

        filterDonate() {
            let key = vue.data.searchDonate
            if (key.length < 1)
                return vue.data.donateList
            return vue.data.donateList.filter(item => item[0].includes(key) || item[1].includes(key))
        }
    },
    mounted() {
        
    },
    created() {
        $.ajax({
            url: "/SettingFile/donate_code.csv",
            async: false,
            success: function (csvd) {
                let data = $.csv.toArrays(csvd);
                //console.log(`donate csv->${data.jsonString()}`)

                vue.data.donateList = data

            },
            dataType: "text",
            complete: function () {
                // call a function on complete 
            }
        });

        //var reader = new FileReader();
        //reader.readAsText(new File('~/Content/file/donate_code.csv'));
        //reader.onload = function (event) {
        //    var csv = event.target.result;
        //    var data = $.csv.toArrays(csv);
        //    console.log(`donate csv->${data.jsonString()}`)
        //}

    }
})

export { vue, event }


