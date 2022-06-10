<div>

    <helpviewer-folder-panel @select="onSelectFolder($event)">

    </helpviewer-folder-panel>

    <helpviewer-header @add="onAdd">

    </helpviewer-header>
    <div style=" position: absolute; top: 60px; left: 280px; right: 0px; bottom: 70px; overflow-y: auto; padding-right: 10px;">
        <el-table :show-header="false" size="mini" :data="items">
            <el-table-column label="标题">
                <template slot-scope="item">
                    <el-link @click="selectDocument=item.row;documentDisplay=true;">{{item.row.Title}}</el-link>
                </template>
            </el-table-column>
            <el-table-column label="标签" width="200">
                <template slot-scope="item">
                    <label>{{item.row.FolderName}}</label>
                </template>
            </el-table-column>
            <el-table-column label="标签" width="100">
                <template slot-scope="item">
                    <label>{{item.row.Tag}}</label>
                </template>
            </el-table-column>
            <el-table-column label="创建时间" width="150">
                <template slot-scope="item">
                    <label>{{item.row.CreateTime}}</label>
                </template>
            </el-table-column>

            <el-table-column label="排序值" v-if="user.role.toLowerCase()=='admin'" width="120">
                <template slot-scope="item">
                    <el-input-number size="mini" style="width:100px;"
                                     v-model="item.row.OrderValue" @change="handleChange(item.row)"
                                     :min="0" :max="1000"></el-input-number>
                </template>
            </el-table-column>

            <el-table-column v-if="user.role.toLowerCase()=='admin'" width="120">
                <template slot-scope="item">
                    <el-button size="mini" circle @click="onEdit(item.row)"><i class="fa-solid fa-pen-to-square"></i></el-button>
                    <el-button size="mini" circle @click="onDelete(item.row)"><i class="fa-solid fa-trash-can"></i></el-button>
                </template>
            </el-table-column>

        </el-table>
    </div>
    <div style="text-align: right; padding: 5px; position: absolute; left: 280px; right: 0px; bottom: 30px">
        <el-pagination background layout="prev, pager, next" :page-size="pageSize" :total="count" @current-change="onPageChange">
        </el-pagination>
    </div>
    <el-dialog title="编辑" class="document-view-panel" :visible.sync="dialogVisible" width="1024px" :close-on-click-modal="false" :fullscreen="true">
        <helpviewer-doc-editor @close="onModify($event)" :token="token"></helpviewer-doc-editor>
    </el-dialog>
    <div class="document-view-panel">
        <el-dialog :title="selectDocument.Title" :visible.sync="documentDisplay" width="1024px" :close-on-click-modal="false" :fullscreen="true">
            <helpviewer-doc-view :token="selectDocument"></helpviewer-doc-view>
        </el-dialog>
    </div>
    <div style="position: absolute; bottom: 0px; left: 300px; right: 0px; text-align: center; padding: 10px;">
        Copyright © 2019-2022   <a href="https://beetlex-io.com" target="_blank">beetlex-io.com</a>
    </div>
</div>
<script>
    export default {
        data() {
            return {
                contentModel: 'helpviewer-setting',
                token: null,
                dialogVisible: false,
                items: [],
                pageSize: 20,
                count: 0,
                page: 0,
                selectDocument: {},
                documentDisplay: false,
                user: { name: '', role: '' },
                selectFolder: '',
            };
        },
        methods: {
            handleChange(e) {
                this.$get('/admin/DocumentChangeOrderValue', { id: e.ID, value: e.OrderValue }).then(r => {
                    this.onList();
                });
            },
            onSelectFolder(e) {
                this.page = 0;
                this.selectFolder = e;
                this.onList();
            },
            onDelete(e) {
                this.$confirmMsg('是否删除文档?', () => {
                    this.$get("/admin/DocumentDelete", { id: e.ID }).then(r => {
                        this.onList();
                    });
                });
            },
            onEdit(e) {
                this.token = e;
                this.dialogVisible = true;
            },
            onPageChange(e) {
                this.page = e - 1;
                this.onList();
            },
            onList() {
                this.$get('/admin/DocumentList', { folder:this.selectFolder, page: this.page, size: this.pageSize }).then(r => {
                    this.items = r.items;
                    this.count = r.count;
                });
            },
            onModify(e) {
                this.dialogVisible = false;
                if (e) {
                    this.onList();
                }
            },
            onAdd() {
                this.token = null;
                this.dialogVisible = true;
            }
        },
        mounted() {
            this.user = this.$getUser();
            console.log("user", this.user);
            this.onList();

        }
    }
</script>
<style>
    .document-view-panel .el-dialog__body {
        position: absolute;
        top: 40px;
        left: 0px;
        right: 0px;
        bottom: 10px;
        overflow-y: auto;
        padding: 0px;
    }

    .document-view-panel .el-dialog__header {
        font-weight: bold;
    }

    .document-view-panel .el-dialog {
        border-radius: 6px;
        max-width: 1280px;
        margin: auto;
    }

    .content-model {
        position: absolute;
        left: 300px;
        top: 40px;
        right: 0px;
        bottom: 0px;
        padding-right: 10px;
        overflow-y: auto;
    }

        .content-model .el-tabs__item:hover {
            color: #409EFF;
            cursor: pointer
        }
</style>