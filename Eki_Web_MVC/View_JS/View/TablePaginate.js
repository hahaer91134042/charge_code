/*
   <table-paginate :raw-data="dataList" v-on:change-page="onChangePage"></table-paginate>
 */

export default Vue.component('table-paginate', {
    props: ['rawData'],
    template:`<div class="pagination" v-show="rawData">
                    <ul>
                        <li v-bind:class="{'disabled': (currPage -10 < 1)}" v-on:click="setPage(currPage-10)"><a href="#">上十頁</a></li>
                        <li v-bind:class="{'disabled': (currPage === 1)}" v-on:click="setPage(currPage-1)"><a href="#">上一頁</a></li>
                        <li v-for="index in totalPage"  v-bind:class="{'active': (currPage === (index))}" v-on:click="setPage(index)">
                            <a href="#">{{ showTextPage(index) }}</a>
                        </li>
                        <li v-bind:class="{'disabled': (currPage === totalPage)}" v-on:click="setPage(currPage+1)"><a href="#">下一頁</a></li>
                        <li v-bind:class="{'disabled': (currPage + 10 > totalPage)}" v-on:click="setPage(currPage+10)"><a href="#">下十頁</a></li>
                        <li class="form-control">共{{ totalNum }}筆</li>                        
                        <li class="form-control">
                            每頁
                            <select v-model="countOfPage">
                                <option value="3">3</option>
                                <option value="5">5</option>
                                <option value="10">10</option>
                                <option value="20">20</option>
                            </select>
                            筆
                        </li>
                        <li class="form-control">
                            跳轉至
                            <input type="text"  maxlength="6" v-model="specificPage" size="1" v-on:keyup.enter="setPage(specificPage)" />
                            頁
                        </li>
                    </ul>
                </div>`,
    data: function () {
        return {
            //分頁用變數初值
            countOfPage: 5,
            currPage: 1,
            totalNum: 0,
            specificPage: ''
        }
    },
    watch: {
        countOfPage: function (n, o) {
            this.$emit('change-page', this.filterData)
        },
        currPage: function (newVal, oldVal) {
            this.$emit('change-page', this.filterData)
        },
        rawData: function (newData, oldData) {
            //console.log(`Paginate rawData change`)
            this.totalNum = newData.length;
            this.$emit('change-page', this.filterData)
        }
        //refreashView: function (newVal, oldVal) {
        //    console.log(`Paginate refreashView change->${newVal}`)
        //    if (newVal) {
        //        this.$emit('change-page', this.filterData)
        //    }
        //}
    },
    methods: {
        showTextPage(index) {
            //console.log(`showTextPage index->${index}`)
            if (index === 1 || index === this.totalPage) {
                //console.log(vm.totalPage);
                //始終顯示第一頁與最後一頁
                return index;
            } else if (index < this.currPage + 2 && index > this.currPage - 2) {
                //始終顯示當前頁前一頁與後一頁
                return index;
            } else if (index === this.currPage + 2 || index === this.currPage - 2) {
                //當前頁的前前頁與後後頁顯示'...'
                return '...';
            } else {
                //其他不顯示
                return false;
            }
        },
        setPage(idx) {
            let index = parseInt(idx);
            if (index <= 0 || index > this.totalPage) {
                return;
            }
            this.currPage = index;
            this.specificPage = '';
        }
    },
    computed: {
        //每頁筆數過濾
        filterData() {
            this.specificPage = '';
            let start = parseInt(this.pageStart);
            let countPage = parseInt(this.countOfPage);
            let end = start + countPage;
            /* console.log('List data->' + vm.ListDatas)*/
            return this.rawData.slice(start, end);
        },
        //依currPage變動重新計算pageStart
        pageStart() {
            return (this.currPage - 1) * this.countOfPage;
        },
        //依過濾結果重新計算totalPage
        totalPage() {
            let total = Math.ceil(this.totalNum / this.countOfPage);
            //console.log(`totalPage->${total}`)
            return total;
        }
    },

    created() {
        //console.log('paginate created')
        this.totalNum = this.rawData.length;
        this.$emit('change-page', this.filterData)
    }
})