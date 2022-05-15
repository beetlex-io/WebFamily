<div>
    <div style="height:600px;overflow-y:auto;">
        <ul class="role-permission-list">
            <li v-for="(item,i) in items">
                <div><el-link :icon="[item.extend?'el-icon-folder-opened':'el-icon-folder']" @click="item.extend=!item.extend">{{item.Category}}<span>({{item.Items.length}})</span></el-link></div>
                <el-table v-if="item.extend" size="mini" :show-header="i==0" :data="item.Items" style="margin-left:20px;width:550px;">
                    <el-table-column label="功能名称" width="150">
                        <template slot-scope="item">
                            <label>{{item.row.Name}}</label>
                        </template>
                    </el-table-column>
                    <el-table-column label="权限码" width="250">
                        <template slot-scope="item">
                            <label>{{item.row.Code}}</label>
                        </template>
                    </el-table-column>
                    <el-table-column label="是否可用" align="right" header-align="right">
                        <template slot-scope="item">
                            <el-switch size="mini" :disabled="true" v-model="item.row.Value"></el-switch>
                        </template>
                    </el-table-column>
                </el-table>
            </li>
        </ul>
    </div>
    <el-row>
        <el-col span="24" style="text-align:right;padding-top:10px;">
            <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="$emit('close')">确定</el-button>
        </el-col>
    </el-row>
</div>
<script>
    export default {
        data() {
            return {
                items: [],
                roleID: '',
            };
        },
        methods: {
            onGet(id) {
                this.roleID = id;
                this.$get('/baseinfo/users/GetPermission', { id: id }).then(r => {
                    r.forEach(v => {
                        v.extend = true;
                    });
                    this.items = r;
                });
            }
        },
        mounted() {

        }
    }
</script>
<style>
    .role-permission-list {
        list-style: none;
        padding: 0px;
        margin: 0px;
    }

        .role-permission-list li > span {
            padding: 5px;
            font-size: 10pt;
            font-weight: bold;
        }
</style>