
/*
     <g-map v-on:map-ready="initMap" />
 */
export default Vue.component('g-map', {
    props: [],
    data: function () {
        return {

        }
    },
    watch: {

    },
    template: ``,
    mounted() {
        //console.log(`gmap ready`)
        $script(eki.config.mapUrl, ()=>{
            this.$emit('map-ready')
        })
    },
    created() {
        //console.log(`gmap create`)
        //console.log($script)
        //var script = document.createElement('script');
        //script.type = 'text/javascript';
        //script.src = 'https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyCMknx65zH4tAOQ5A721sJO75ph6R9OvsQ';
        //document.body.appendChild(script);
        //this.$emit('init')
    }

})