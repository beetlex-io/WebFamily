<div>
    <div class="editor-bar" style="padding-left:20px;">
        <el-button size="mini" title="添加部门" @click="selectID='';dialogVisible=true;"><i class="fa-solid fa-file-circle-plus"></i></el-button>
        <el-input size="mini" placeholder="请输入查询名称" v-model="searchName" style="width:200px;margin-left:5px;">
            <el-button slot="append" @click="page=0;onList()"><i class="fa-solid fa-magnifying-glass"></i></el-button>
        </el-input>
    </div>
    <el-table size="mini" :data="items">

        <el-table-column label="部门名称" width="160">
            <template slot-scope="item">
                <el-link @click="onSelect(item.row.ID)">{{item.row.Name}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="负责人" width="160">
            <template slot-scope="item">
                <label>{{item.row.Manager}}</label>
            </template>
        </el-table-column>
        <el-table-column label="联系电话" width="160">
            <template slot-scope="item">
                <label>{{item.row.TelePhone}}</label>
            </template>
        </el-table-column>
        <el-table-column label="上级部门" width="160">
            <template slot-scope="item">
                <label>{{item.row.Superior}}</label>
            </template>
        </el-table-column>
        <el-table-column label="备注">
            <template slot-scope="item">
                <label>{{item.row.Note}}</label>
            </template>
        </el-table-column>
        <el-table-column label="" width="80" align="right">
            <template slot-scope="item">
                <el-button @click="onDelete(item.row)" v-if="!item.row.SystemData" size="mini">
                    <i class="fa-solid fa-trash-can"></i>
                </el-button>
            </template>
        </el-table-column>
        <div slot="append" style="text-align:right;padding:5px;">
            <el-pagination background layout="prev, pager, next" :page-size="size" :total="count" @current-change="onPageChange">
            </el-pagination>
        </div>
    </el-table>
    <el-dialog title="编辑部门"
               :visible.sync="dialogVisible" @opened="onOpen"
               width="500px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-department-editor ref="editor" @close="onClose($event)" :token="selectID"></baseinfo-department-editor>
    </el-dialog>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                items: [],
                count: 0,
                size: 15,
                page: 0,
                selectID: '',
                dialogVisible: false,
                searchName: '',
            };
        },
        methods: {
            onSelect(e) {

                this.selectID = e;
                this.dialogVisible = true;
            },
            onOpen() {
                this.$refs.editor.onGet(this.selectID);
            },
            onClose(e) {
                this.dialogVisible = false;
                if (e) {
                    this.onList();
                }
            },
            onPageChange(e) {
                this.page = e - 1;
                this.onList();
            },
            onDelete(e) {
                this.$confirmMsg('是否要删除部门?', () => {
                    this.$get("/baseinfo/department/Delete", { id: e.ID }).then(r => {
                        this.page = 0;
                        this.onList();
                    });
                });
            },
            onList() {
                this.$get('/baseinfo/department/list', { matchName: this.searchName, page: this.page, size: this.size }).then(r => {
                    this.items = r.items;
                    this.count = r.count;
                });
            }
        },
        mounted() {
            this.onList();
        }
    }
</script>
