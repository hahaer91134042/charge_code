/*
 * 這是用來跟C#介接轉資料送到VUE裡面用的
 須設定 v-on:init 來指定初始資料要傳遞給誰
 exp:<view-data  data="" v-on:init="initData"></view-data>
 */
export default Vue.component('view-data', {
    props: ['data'],
    watch: {

    },
    template: ``,//這邊會跳警告 但不用管 依樣可以用

    created() {
        //console.log('view data create');
        var items = JSON.parse(base64.decode(this.data))
        //console.log(`view data get->${JSON.stringify(items)}`)
        this.$emit('init', items)
    }

})