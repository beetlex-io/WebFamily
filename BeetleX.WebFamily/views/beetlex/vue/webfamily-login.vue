<div style="width:450px;margin:auto;margin-top:100px">

    <el-card class="box-card">
        <div slot="header" class="clearfix">
            <h2 style="margin:0px;">{{$getTitle()}}</h2>

        </div>
        <el-form size="mini" :model="record" label-width="80px" ref="dataform">
            <el-row>
                <el-col :span="24">
                    <el-form-item label="用户名" prop="Name" :rules="Name_rules"><el-input size="mini" v-model="record.Name" placeholder="admin"></el-input></el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="24">
                    <el-form-item label="密码" prop="Password" :rules="Password_rules"><el-input size="mini" v-model="record.Password" show-password placeholder="123456"></el-input></el-form-item>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="24" style="text-align:right">
                    <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">登陆</el-button>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span="24" style="text-align:right;padding-top:20px;">
                    Web SPA plug-in for <a href="http://beetlex.io" target="_blank">BeetleX</a>
                </el-col>
            </el-row>
        </el-form>
    </el-card>
</div>
<script>
    {
        props: [],
            data(){
            return {
                Name_rules: [{ required: true, message: '用户名不能为空！', trigger: 'blur' },],
                Password_rules: [{ required: true, message: '密码不能为空！', trigger: 'blur' },],
                record: {
                    Name: null,
                    Password: null,
                }
            };
        },
        methods: {

            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$post('/website/login', this.record).then(r => {
                            this.$userLogin(this.record.Name);
                        });
                    }
                });
            },
            resetForm() {
                this.$refs['dataform'].resetFields();
            }
        },
        mounted(){
        }
    }
</script>
