
/*
 這是vue instance的模板
 */ 

var vue = new VueController({

    el: '#PageTop',

    data: {
        sidebar: false,
        title: "電表"



    },
    watch: {


    },
    methods: {


    },
    filters: {

    },
    computed: {
        meterCount() {
            return vue.main.data.user.Meter.length
        },
        name() {
            return vue.main.data.user.Name
        },
        notifyCount() {
            return 0;
        }
    },
    mounted() {

    },
    created() {
        //console.log(`PageTop created`)
    }
})

export { vue }


