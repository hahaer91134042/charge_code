/*
 需實作屬性 v-on:close-img 來設定關閉dialog的時候把url變成空格
 */
export default Vue.component('img-dialog', {
    props: ['showimg','width','height'],
    data: function () {
        return {
            imgStyle: {
                width:'90%',
                height: '50%'
                //目前沒用以後再想想該怎使用
                //width:90%;height:80%;

                //return `width:${this.width}%;height:${this.height}%;`
            }
        }
    },
    computed: {

    },
    watch: {
        showimg: function (url, old) {
            if (url.length > 0)
                this.open()
            else
                this.cancel()
        }
    },
    template: `<div style="display:none;">
        <div id="dialog_large_image">
            <button class="close mr-2 mt-2" v-on:click="$emit('close-img')">×</button>
            <div style="text-align:center">
                <img id="imgLargeImage" v-bind:src='showimg' style="width:90%;height:80%;">
            </div>            
            <div class="text-center" style="margin:10px">
                <input type="button" class="btn btn-outline-primary" value="關閉" v-on:click="$emit('close-img')" />
            </div>
        </div>
    </div>`,
    methods: {
        open() {
            $.colorbox({
                inline: true,
                width: "80%", height: "80%",
                open: true,
                href: "#dialog_large_image",
                overlayClose: false, escKey: false, closeButton: false, trapFocus: false
            });
        },
        cancel() {
            $.colorbox.close();
        }
    },
    created() {
        console.log(`img dialog created w->${this.width} h->${this.height}  style->${JSON.stringify(this.imgStyle)}`)
    }
})