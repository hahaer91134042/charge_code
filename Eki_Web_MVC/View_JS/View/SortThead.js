/*
 * tableColumns: [
       { title: '開始時間', key: 'Start', class: 'col-xs-2 text-capitalize th_click', sort: true, default: true },
    ]
  <thead is="sort-thead" :columns="tableColumns" :data-list="viewData" v-on:filter-data="onSortData" ></thead>
 */
export default Vue.component('sort-thead', {
    props:['columns','dataList'],
    template: `<thead>
                        <tr class="table table-primary" >
                            <template v-for="info in columns">
                                <th v-if="info.sort==true"  :class="info.class"  style="font-weight:bold;" v-on:click="sortBy(info.key),reverse = !reverse">
                                    {{info.title}}
                                    <span class="sortorder" v-if="sortTitle === info.key" v-bind:class="{'reverse': reverse}"></span>
                                </th>
                                <th v-else :class="info.class"  style="font-weight:bold;">
                                    {{info.title}}
                                </th>
                            </template>
                        </tr>
                    </thead>`,
    data: function () {
        return {
            reverse: true,
            sortTitle: ''
        }
    },
    watch: {

    },
    methods: {
        sortBy(columnName) {

            //var length = columnName.length;
            //console.log(`sortBy->${columnName} length->${length}`)

            if (columnName.length < 1)
                return;

            let orderBy = this.reverse ? 'desc' : 'asc';

            //console.log(`reverse->${this.reverse} orderBy->${orderBy}`)
            this.sortTitle = columnName;

            this.dataList = _.orderBy(this.dataList, columnName, orderBy)

            this.$emit('filter-data', this.dataList)

        }
    },

    created() {
        //console.log(`columns->${JSON.stringify(this.columns)}`)
        this.sortTitle = this.columns.find(e => e.default).key
    }
})