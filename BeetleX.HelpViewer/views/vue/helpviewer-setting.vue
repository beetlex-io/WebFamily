<div class="setting-panel">
    <el-tabs v-model="activeName" type="card">
        <el-tab-pane label="用户管理" name="first">
            <el-table size="mini" :data="users">
                <el-table-column label="用户名">
                    <template slot-scope="item">
                        <label>{{item.row.Name}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="类型" width="100">
                    <template slot-scope="item">
                        <label>{{item.row.Type}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="创建时间" width="200">
                    <template slot-scope="item">
                        <label>{{item.row.CreateTime}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="可用" width="100">
                    <template slot-scope="item">
                        <el-switch size="mini" v-model="item.row.Enabled"></el-switch>
                    </template>
                </el-table-column>
                <el-table-column label="管理员" width="100">
                    <template slot-scope="item">
                        <el-switch size="mini" v-model="item.row.IsAdmin" @change="onChangeAdmin(item.row)"></el-switch>
                    </template>
                </el-table-column>
                <el-table-column label="" width="100">
                    <template slot-scope="item">
                        <el-button size="mini" @click="changePWDUser=item.row;dialogVisible=true"><i class="fa-solid fa-key"></i></el-button>
                    </template>
                </el-table-column>
                <div slot="append" style="text-align:right;padding:5px;">
                    <el-pagination background layout="prev, pager, next" :page-size="pageSize" :total="count" @current-change="onPageChange">
                    </el-pagination>
                </div>
            </el-table>

        </el-tab-pane>
        <el-tab-pane label="配置管理" name="second">
            <el-form :inline="true" size="mini" :model="folderRecord" label-width="100px" ref="dataform">
                <el-form-item label="名称" prop="Name" :rules="Name_rules">
                    <el-input size="mini" v-model="folderRecord.Name"></el-input>
                </el-form-item>
                <el-form-item label="描述" prop="Note">
                    <el-input size="mini" v-model="folderRecord.Note" style="width:250px;"></el-input>
                </el-form-item>
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">添加</el-button>
            </el-form>
            <el-table size="mini" :data="folders">
                <el-table-column label="名称" width="200">
                    <template slot-scope="item">
                        <label>{{item.row.Name}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="描述">
                    <template slot-scope="item">
                        <label>{{item.row.Note}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="" width="100">
                    <template slot-scope="item">
                        <el-button size="mini" @click="onFolderDelete(item.row)">删除</el-button>
                    </template>
                </el-table-column>
            </el-table>
        </el-tab-pane>
    </el-tabs>
    <el-dialog title="修改密码" :visible.sync="dialogVisible" width="500px" :close-on-click-modal="false" :append-to-body="true">
        <webfamily-changepwd @close="onChangePwd($event)"></webfamily-changepwd>
    </el-dialog>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                users: [],
                pageSize: 20,
                count: 0,
                page: 0,
                dialogVisible: false,
                folders: [],
                Name_rules: [{ required: true, message: '值不能为空！', trigger: 'blur' },],
                folderRecord: {
                    Name: null,
                    Note: null,
                },
                activeName: 'first',
                changePWDUser: {},
            };
        },
        methods: {
            onChangeAdmin(e) {
                this.$get('/admin/UserAdminEnabled', { id: e.ID, enabled: e.IsAdmin }).then(r => {

                });
            },
            onChangePwd(e) {
                this.dialogVisible = false;
                if (e) {

                    this.$post('/admin/UserChangePwd', { id: this.changePWDUser.ID, password: e }).then(r => {
                        this.dialogVisible = false;
                    });

                }
            },

            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$confirmMsg('是否保存数据?', () => {
                            this.$post('/admin/FolderAdd', this.folderRecord).then(r => {
                                this.$clearObject(this.folderRecord);
                                this.onListFolers();
                            });
                        });
                    }
                });
            },
            onFolderDelete(e) {
                this.$confirmMsg('是否删除目录?', () => {
                    this.$post('/admin/FolderDelete', { id: e.ID }).then(r => {
                        this.onListFolers();
                    });
                });
            },
            onPageChange(e) {
                this.page = e - 1;
                this.onListUsers();
            },
            onListFolers() {
                this.$get("/admin/FolderList").then(r => {
                    this.folders = r;
                });
            },
            onListUsers() {
                this.$get('/admin/ListUsers', { page: this.page, size: this.pageSize }).then(r => {
                    this.users = r.items;
                    this.count = r.count;
                });
            }
        },
        mounted() {
            this.onListUsers();
            this.onListFolers();
        }
    }
</script>
<style>
    .setting-panel .el-tabs__item:hover {
        color: #409EFF;
        cursor: pointer
    }
</style>