
let event = {
    //通知外部這step完成
    onFinish: function (payMethod) {

    }
}

var vue = new VueController({

    el: '#charge_step3',

    data: {
        show: false,
        userPayMethod: PayMethod.Credit,

        selectStyle: {
            border: '4px solid',
            borderColor: 'rgb(153 203 81)'
        },
        unSelectStyle: {
            border: '2px solid',
            borderColor: 'rgb(217 217 217)'
        }
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
        onSelectPayMethod(m) {
            vue.data.userPayMethod = m;
        },
        onNext() {

            vue.data.show = false

            event.onFinish(vue.data.userPayMethod)

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

export { vue, event }


