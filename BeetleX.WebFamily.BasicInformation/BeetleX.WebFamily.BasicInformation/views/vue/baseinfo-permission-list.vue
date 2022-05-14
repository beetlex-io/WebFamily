<div>
    <div class="editor-bar" style="padding-left:20px;">
        <el-button size="mini" title="添加权限" @click="selectID='';dialogVisible=true;"><i class="fa-solid fa-file-circle-plus"></i></el-button>
        <el-divider direction="vertical"></el-divider>
        <el-select size="mini" v-model="searchCategory" filterable>
            <el-option label="无" value="">
            </el-option>
            <el-option v-for="(item,index) in categoriesOptions" :label="item" :value="item" key="item">

            </el-option>
        </el-select>
        <el-button size="mini" @click="onList" style="margin-left:5px;">
            <i class="fa-solid fa-magnifying-glass"></i>
        </el-button>
    </div>
    <el-table size="mini" :data="items">
        <el-table-column label="分类" width="100">
            <template slot-scope="item">
                <label>{{item.row.Category}}</label>
            </template>
        </el-table-column>
        <el-table-column label="功能名称" width="150">
            <template slot-scope="item">
                <el-link @click="onSelect(item.row)">{{item.row.Name}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="权限码" width="150">
            <template slot-scope="item">
                <label>{{item.row.Code}}</label>
            </template>
        </el-table-column>

        <el-table-column label="描述">
            <template slot-scope="item">
                <label>{{item.row.Note}}</label>
            </template>
        </el-table-column>
        <el-table-column label="" width="120" align="right">
            <template slot-scope="item">
                <el-button @click="onDelete(item.row)" v-if="!item.row.SystemData" size="mini"><i class="fa-solid fa-trash-can"></i></el-button>
            </template>
        </el-table-column>
    </el-table>
    <el-dialog title="编辑权限"
               :visible.sync="dialogVisible" @opened="onOpen"
               width="500px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-permission-editor ref="editor" @close="onClose($event)"></baseinfo-permission-editor>
    </el-dialog>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                items: [],
                selectID: '',
                searchCategory: '',
                categoriesOptions: [],
                dialogVisible: false,
            };
        },
        methods: {
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
            onDelete(e) {
                this.$confirmMsg('是否删除权限项?', () => {
                    this.$get('/baseinfo/permissions/Delete', { id: e.ID }).then(r => {
                        this.onList();
                    });
                });
            },
            onOpen() {
                this.$refs.editor.onGet(this.selectID);
            },
            onPageChange(e) {
                this.page = e - 1;
            },
            onList() {
                this.$get('/baseinfo/permissions/List', { category: this.searchCategory }).then(r => {
                    this.items = r;
                });
                this.onListCategoriesOptions();
            },
            onListCategoriesOptions() {
                this.$get('/baseinfo/permissions/ListCategories').then(r => {
                    this.categoriesOptions = r;
                });
            }
        },
        mounted() {
            this.onList();
        }
    }
</script>
