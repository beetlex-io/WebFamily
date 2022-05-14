<div>
    <div class="editor-bar" style="padding-left:20px;">
        <el-button size="mini" title="添加角色" @click="selectID='';dialogVisible=true;">
            <i class="fa-solid fa-file-circle-plus"></i>
        </el-button>
        <el-divider direction="vertical"></el-divider>
        <el-input size="mini" placeholder="请输入查询名称" v-model="searchName" style="width:200px">
        </el-input>
        <el-button size="mini" @click="page=0;onList()" style="margin-left:5px;"><i class="fa-solid fa-magnifying-glass"></i></el-button>
    </div>
    <el-table size="mini" :data="items">

        <el-table-column label="角色名称" width="150">
            <template slot-scope="item">
                <el-link @click="onSelect(item.row)">{{item.row.Name}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="描述">
            <template slot-scope="item">
                <label>{{item.row.Note}}</label>
            </template>

        </el-table-column>
        <el-table-column label="" width="120" align="right">
            <template slot-scope="item">
                <el-button @click="onPermission(item.row)" title="权限配置" size="mini">
                    <i class="fa-solid fa-sliders"></i>
                </el-button>
                <el-button @click="onDelete(item.row)" v-if="!item.row.SystemData" size="mini">
                    <i class="fa-solid fa-trash-can"></i>
                </el-button>
            </template>
        </el-table-column>
        <div slot="append" style="text-align:right;padding:5px;">
            <el-pagination background layout="prev, pager, next" :page-size="pageSize" :total="count" @current-change="onPageChange">
            </el-pagination>
        </div>
    </el-table>
    <el-dialog title="角色权限设置" :visible.sync="dialogPermissionVisible" @opened="onOpenPermission" width="520px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-roles-permission-editor ref="editPermission" @close="dialogPermissionVisible=false;">

        </baseinfo-roles-permission-editor>
    </el-dialog>
    <el-dialog title="编辑信息" :visible.sync="dialogVisible" @opened="onOpen" width="500px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-roles-editor ref="editor" @close="onClose($event)"></baseinfo-roles-editor>
    </el-dialog>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                items: [],
                pageSize: 15,
                selectID: '',
                count: 0,
                page: 0,
                dialogVisible: false,
                searchName: '',
                rolePermission: '',
                dialogPermissionVisible: false,
            };
        },
        methods: {
            onPermission(e) {
                this.rolePermission = e.ID;
                this.dialogPermissionVisible = true;
            },
            onSelect(e) {
                this.selectID = e.ID;
                this.dialogVisible = true;
            },
            onClose(e) {
                if (e) {
                    this.onList();
                }
                this.dialogVisible = false;
            },
            onOpenPermission() {
                this.$refs.editPermission.onGet(this.rolePermission);
            },
            onOpen() {
                this.$refs.editor.onGet(this.selectID);
            },
            onPageChange(e) {
                this.page = e - 1;
                this.onList();
            },
            onList() {
                this.$get('/baseinfo/roles/list', { matchName: this.searchName, page: this.page, size: this.pageSize }).then(r => {
                    this.items = r.items;
                    this.count = r.count;
                });
            },
            onDelete(e) {
                this.$confirmMsg('是否删除角色?', () => {
                    this.$get('/baseinfo/roles/Delete', { id: e.ID }).then(r => {
                        this.page = 0;
                        this.onList();
                    });
                });
            }
        },
        mounted() {
            this.onList();
        }
    }
</script>
