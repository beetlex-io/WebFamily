<div>
    <div class="editor-bar" style="padding-left:20px;">
        <el-button size="mini" title="添加用户" @click="selectID='';dialogVisible=true;">
            <i class="fa-solid fa-file-circle-plus"></i>
        </el-button>
        <el-divider direction="vertical"></el-divider>
        <el-input size="mini" placeholder="请输入查询名称" v-model="searchName" style="width:200px">
        </el-input>
        <el-select size="mini" v-model="department" filterable style="margin-left:5px;">
            <el-option label="无" value="">
            </el-option>
            <el-option v-for="(item,index) in DepartmentID_options" :label="item.label" :value="item.value" key="item.value">

            </el-option>
        </el-select>
        <el-button size="mini" @click="page=0;onList()" style="margin-left:5px;"><i class="fa-solid fa-magnifying-glass"></i></el-button>
        <el-button size="mini" @click="onRefresh" style="margin-left:5px;">
            <i class="fa-solid fa-arrow-rotate-right"></i>
        </el-button>
    </div>
    <el-table size="mini" :data="items">
        <el-table-column label="员工编号" width="100">
            <template slot-scope="item">
                <el-link @click="onSelect(item.row)">{{item.row.WorkNumber}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="姓名" width="150">
            <template slot-scope="item">
                <el-link @click="onSelect(item.row)">{{item.row.Name}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="职位" width="150">
            <template slot-scope="item">
                <label>{{item.row.JobPostion}}</label>
            </template>
        </el-table-column>
        <el-table-column label="所属部门" width="150">
            <template slot-scope="item">
                <label>{{item.row.DepartmentID}}</label>
            </template>
        </el-table-column>
        <el-table-column label="上级负责人" width="150">
            <template slot-scope="item">
                <label>{{item.row.SuperiorID}}</label>
            </template>
        </el-table-column>
        <el-table-column label="手机">
            <template slot-scope="item">
                <label>{{item.row.MobilePhone}}</label>
            </template>
        </el-table-column>
        <el-table-column label="用效" width="80">
            <template slot-scope="item">
                <el-switch size="mini" v-model="item.row.Enabled" @change="onEnabled(item.row)"></el-switch>
            </template>
        </el-table-column>
        <el-table-column label="" width="120" align="right">
            <template slot-scope="item">
                <el-button @click="selectID=item.row.ID;permissionDialogVisible=true" title="查看权限" size="mini">
                    <i class="fa-solid fa-sliders"></i>
                </el-button>
                <el-button @click="onDelete(item.row)" v-if="!item.row.SystemData" size="mini">
                    <i class="fa-solid fa-trash-can"></i>
                </el-button>
            </template>
        </el-table-column>
        <div slot="append" style="text-align:right;padding:5px;">
            <el-pagination background layout="prev, pager, next" :page-size="pageSize"
                           :total="count" @current-change="onPageChange">
            </el-pagination>
        </div>
    </el-table>
    <el-dialog title="用户权限" :visible.sync="permissionDialogVisible" @opened="onOpenPermission" width="540px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-user-permission-editor @close="permissionDialogVisible=false" ref="permissionEditor"></baseinfo-user-permission-editor>
    </el-dialog>

    <el-dialog title="编辑用户" :visible.sync="dialogVisible" @opened="onOpen" width="700px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-user-editor ref="editor" @close="onClose($event)"></baseinfo-user-editor>
    </el-dialog>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                items: [],
                pageSize: 15,
                page: 0,
                count: 0,
                selectID: '',
                searchName: '',
                department: '',
                dialogVisible: false,
                DepartmentID_options: [],
                permissionDialogVisible: false,
            };
        },
        methods: {
            onOpenPermission() {
                this.$refs.permissionEditor.onGet(this.selectID);
            },
            onPageChange(e) {
                this.page = e - 1;
                this.onList();
            },
            onEnabled(e) {
                this.$get('/baseinfo/users/Enabled', { id: e.ID, enabled: e.Enabled }).then(r => {

                });
            },
            onDelete(e) {
                this.$confirmMsg('是否要删除用户信息?', () => {
                    this.$get('/baseinfo/users/Delete', { id: e.ID }).then(r => {
                        this.onList();
                    });
                });
            },
            onSelect(e) {
                this.selectID = e.ID;
                this.dialogVisible = true;
            },
            onClose(e) {
                this.dialogVisible = false;
                if (e) {
                    this.onList();
                    this.onListDepartmentOpetions();
                }
            },
            onRefresh() {
                this.onListDepartmentOpetions();
                this.onList();
            },
            onOpen() {
                this.$refs.editor.onGet(this.selectID);
            },
            onList() {
                this.$get('/baseinfo/users/list',
                    {
                        matchName: this.searchName, department: this.department, page: this.page, size: this.pageSize
                    }).then(r => {
                        this.items = r.items;
                        r.items.forEach(v => {
                            if (v.Enabled)
                                v.Enabled = true;
                        });
                        this.count = r.count;
                    });
            },
            onListDepartmentOpetions() {
                this.$get("/baseinfo/department/ListSelectOptions").then(r => {
                    this.DepartmentID_options = r;
                });
            },

        },
        mounted() {
            this.onListDepartmentOpetions();
            this.onList();
        }
    }
</script>
