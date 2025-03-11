//import * as Enum from "../Model/enum.js";
import * as Enum from "../model/enum.js";

/*
 * data=[{"Current":2,"Charge":7}]
 * 
<socket-label v-bind:data-list="socketData"></socket-label>
 */

export default Vue.component('socket-label', {
    props: ["dataList"],
    data: function () {
        return {

        }
    },
    watch: {

    },
    methods: {
        socketName(value) {
            if (value < 0)
                return '無使用'
            let name = Enum.ChargeSocket.find[value].name
            //console.log(`socket value->${value} get name->${name}`)
            return name
        },
        currentName(value) {
            if (value < 0)
                return '無使用'
            let name = Enum.Current.find[value].name
            //console.log(`current value->${value} get name->${name}`)
            return name
        }
    },
    template: `<div v-if="dataList.length < 1">
                            <span class="badge badge-success" style="font-size:medium">一般車位</span>
                        </div>
                        <div v-else  v-for="item in dataList">
                            <span class="badge"
                                  v-bind:class="{'badge-primary':item.Current==2,'badge-danger':item.Current==1}"
                                  style="font-size:medium">{{currentName(item.Current)}}</span> |
                            <span style="color:black;font-size:medium;vertical-align:middle;">{{socketName(item.Charge)}}</span>
                        </div>`,
    ready() {
        //console.log(` socket label list->${JSON.stringify(socketList)}`)

    },
    created() {

    }

})