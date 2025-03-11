
/*
 這是vue instance的模板
 */ 

var vue = {

    el: '#',

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

    }
}

export { vue }


