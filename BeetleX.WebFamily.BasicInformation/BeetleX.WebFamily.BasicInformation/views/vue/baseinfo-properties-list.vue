<div>
    <el-row :gutter="20">
        <el-col>
            <div class="editor-bar">
                <el-form :inline="true" size="mini" :model="record" label-width="80px" ref="dataform">

                    <el-form-item label="名称" prop="Name" :rules="Name_rules"><el-input size="mini" v-model="record.Name"></el-input></el-form-item>
                    <el-form-item label="值" prop="Value" :rules="Value_rules"><el-input size="mini" v-model="record.Value"></el-input></el-form-item>
                    <el-form-item label="分类" prop="Category" :rules="Category_rules">
                        <el-input size="mini" v-model="record.Category" class="input-with-select" style="width:150px;">
                            <el-select v-model="record.Category" slot="prepend">
                                <el-option v-for="(item,index) in categories" :label="item" :value="item"></el-option>
                            </el-select>
                        </el-input>
                    </el-form-item>
                    <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">添加</el-button>
                </el-form>
            </div>
            <el-table size="mini" :data="items">

                <el-table-column label="属性分类" width="240">
                    <template slot="header" slot-scope="scope">
                        <span style="margin-right:10px;">分类</span>
                        <el-select style="width:160px;" size="mini" v-model="selectCategory" @change="onSelectCategory" filterable placeholder="请选择">

                            <el-option key=""
                                       label="无"
                                       value="">
                            </el-option>
                            <el-option v-for="item in categories"
                                       :key="item"
                                       :label="item"
                                       :value="item">
                            </el-option>

                        </el-select>
                    </template>
                    <template slot-scope="item">
                        <label>{{item.row.Category}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="名称" width="160">
                    <template slot-scope="item">
                        <label>{{item.row.Name}}</label>
                    </template>
                </el-table-column>
                <el-table-column label="值">
                    <template slot-scope="item">
                        <el-input size="mini" v-model="item.row.Value" @change="item.row.isEdit=true"></el-input>
                    </template>
                </el-table-column>
                <el-table-column label="OrderValue" width="140">
                    <template slot-scope="item">
                        <el-input-number size="mini" style="width:120px;" @change="item.row.isEdit=true" v-model="item.row.OrderValue"></el-input-number>
                    </template>
                </el-table-column>
                <el-table-column label="" width="120" align="right">
                    <template slot-scope="item">
                        <el-button v-if="item.row.isEdit" size="mini" icon="fa-solid fa-floppy-disk" @click="onSave(item.row)"></el-button>
                        <el-button @click="onDelete(item.row)" v-if="!item.row.SystemData" size="mini"><i class="fa-solid fa-trash-can"></i></el-button>
                    </template>
                </el-table-column>
                <div slot="append" style="text-align:right;padding:5px;">
                    <el-pagination background layout="prev, pager, next" :page-size="pageSize" :total="count" @current-change="onPageChange">
                    </el-pagination>
                </div>
            </el-table>
        </el-col>
    </el-row>

</div>
<script>
    export default {
        props: [],
        data() {
            return {
                items: [],
                categories: [],
                selectCategory: '',
                pageSize: 15,
                page: 0,
                count: 0,
                Category_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                Category_options: [],
                Name_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                Value_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                record: {
                    ID: null,
                    Category: null,
                    Name: null,
                    Value: null,
                }
            };
        },
        methods: {
            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$post('/baseinfo/properties/Modify', this.record).then(r => {
                            this.resetForm();
                            this.onList();
                            this.onListCategories();
                        });
                    }
                });
            },
            onSave(e) {
                e.isEdit = false;
                this.$confirmMsg('是否保存属性？', () => {
                    this.$post('/baseinfo/properties/Modify', e).then(r => {

                    });
                });
            },
            onDelete(e) {
                this.$confirmMsg('是否要删除属性？', () => {
                    this.$get("/baseinfo/properties/delete", { id: e.ID }).then(r => {
                        this.page = 0;
                        this.onList();
                    });
                });
            },
            resetForm() {
                this.$refs['dataform'].resetFields();
            },
            onListCategories() {
                this.$get('/baseinfo/properties/ListCategories').then(r => {
                    this.categories = r;
                });
            },
            onList() {
                this.$get("/baseinfo/properties/List", { category: this.selectCategory, page: this.page, size: this.pageSize })
                    .then(r => {
                        r.items.forEach(v => {
                            v.isEdit = false;
                        });
                        this.items = r.items;
                        this.count = r.count;
                    });
            },
            onSelectCategory() {
                this.page = 0;
                this.onList();
            },
            onPageChange(e) {
                this.page = e - 1;
                this.onList();
            }
        },
        mounted() {
            this.onListCategories();
            this.onList();
        }
    }
</script>
<style>
    .editor-bar {
        border-bottom-style: solid;
        border-bottom-width: 1px;
        border-bottom-color: #ebebeb;
        padding: 10px;
        padding-left: 0px;
        padding-right: 0px;
        margin-bottom: 20px;
        background-color: #fbfbfb
    }

    .categories-list {
        width: 200px;
    }

        .categories-list ul {
            list-style: none;
            margin: 10px;
            padding: 0px;
        }

        .categories-list a {
            width: 160px;
            display: inline-block;
            padding: 4px;
            border-radius: 4px;
        }

            .categories-list a:hover {
                background-color: #ebebeb;
            }
</style>
