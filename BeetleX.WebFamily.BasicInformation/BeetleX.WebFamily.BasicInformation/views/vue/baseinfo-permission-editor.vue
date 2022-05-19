<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="24">
                <el-form-item label="权限分类" prop="Category">
                    <el-input size="mini" v-model="record.Category" class="input-with-select">
                        <el-select v-model="record.Category" slot="prepend">
                            <el-option v-for="(item,index) in Category_options" :label="item" :value="item"></el-option>
                        </el-select>
                    </el-input>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="功能名称" prop="Name" :rules="Name_rules"><el-input size="mini" v-model="record.Name"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="权限码" prop="Code" :rules="Code_rules"><el-input size="mini" v-model="record.Code"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="描述" prop="Note"><el-input size="mini" v-model="record.Note" type="textarea"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col span="24" style="text-align:right">
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="$emit('close',false)">取消</el-button>
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">确定</el-button>
            </el-col>
        </el-row>
    </el-form>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                Category_options: [],
                Name_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                Code_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                record: {
                    ID: null,
                    Category: null,
                    Name: null,
                    Code: null,
                    Note: null,
                }
            };
        },
        methods: {
            onGet(id) {
                if (id) {
                    this.$get("/baseinfo/permissions/get", { id: id }).then(r => {
                        this.record = r;
                    });
                }
                else {
                    this.$clearObject(this.record);
                }
                this.onListCategoriesOptions();
            },
            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$confirmMsg('是否保存权限项?', () => {
                            this.$post('/baseinfo/permissions/Modify', this.record).then(r => {
                                this.$emit('close', true);
                            });
                        });
                    }
                });
            },
            onListCategoriesOptions() {
                this.$get('/baseinfo/permissions/ListCategories').then(r => {
                    this.Category_options = r;
                });
            },
            resetForm() {
                this.$refs['dataform'].resetFields();
            }
        },
        mounted() {
        }
    }
</script>
