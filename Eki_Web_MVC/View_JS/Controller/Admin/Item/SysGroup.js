


var vue = {

    el: '#sysGroupDiv',

    data: {
        show: false



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


        }

    },
    filters: {

    },
    computed: {

    },
    mounted() {

    },
    created() {
        console.log('SysGroup created')
    }
}

export { vue }


