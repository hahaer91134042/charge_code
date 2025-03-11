/*
 <progress-view :loading="isLoading" />
 */

export default Vue.component('progress-view', {
    props: ['loading'],
    //data: function() {
    //    return {
    //        load: this.loading
    //    }
    //},
    watch: {
        loading: function (now, old) {
            //console.log('progress is loading->'+now)
            if (now)
                this.show()
            else
                this.close()
        }
    },
    template: `<div style="display:none;" > <img id="loading_progress"  src='../../images/loading461x461.gif' alt='loading...' style='width:250px;height:250px;' /> </div>`,
    methods: {
        show() {
            let setting = {
                inline: true,
                open: true,
                href: '#loading_progress',
                overlayClose: false, escKey: false, closeButton: false, trapFocus: false
            }
            $.colorbox(setting)
        },
        close() {
            $.colorbox.close();
        }
    }

})

