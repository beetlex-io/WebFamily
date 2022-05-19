<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="24">
                <el-form-item label="名称" prop="Name" :rules="Name_rules"><el-input size="mini" v-model="record.Name"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="描述" prop="Note" :rules="Note_rules"><el-input size="mini" v-model="record.Note" type="textarea"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col span="24" style="text-align:right">
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="$emit('close',false)">取消</el-button>
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm()">确定</el-button>
            </el-col>
        </el-row>
    </el-form>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                Name_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                Note_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                record: {
                    Name: null,
                    Note: null,
                }
            };
        },
        methods: {
            onGet(id) {
                if (id) {
                    this.$get('/baseinfo/roles/get', { id: id }).then(r => {
                        this.record = r;
                    });
                }
                else {
                    this.$clearObject(this.record);
                }
            },

            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$confirmMsg('是否保存角色信息?', () => {
                            this.$post('/baseinfo/roles/modify', this.record).then(r => {
                                this.$emit('close', true);
                            });
                        });
                    }
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
