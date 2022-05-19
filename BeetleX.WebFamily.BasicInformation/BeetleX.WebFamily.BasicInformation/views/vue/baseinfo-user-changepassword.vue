<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="24">
                <el-form-item label="新密码" prop="Password" :rules="Password_rules"><el-input size="mini" v-model="record.Password" show-password></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="确认密码" prop="Password1" :rules="Password1_rules"><el-input size="mini" v-model="record.Password1" show-password></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col span="24" style="text-align:right">
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="$emit('close')">取消</el-button>
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
                Password_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                Password1_rules: [{ required: true, message: '必填！', trigger: 'blur' },
                {
                    validator: (rule, value, callback) => {
                        if (value != this.record.Password) {
                            callback(new Error('确认密码不一致!'));
                        }
                        else {
                            callback();
                        }
                    }, trigger: 'blur'
                }
                ],
                record: {
                    Password: null,
                    Password1: null,
                },
                userID: null,
            };
        },
        methods: {
            onGet(id) {
                this.userID = id;
                this.$clearObject(this.record);
            },
            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$confirmMsg('是否修改用户密码?', () => {
                            this.$get('/baseinfo/users/ChangePassword', { id: this.userID, password: this.record.Password }).then(r => {
                                this.$emit('close');
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
