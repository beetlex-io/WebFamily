<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="24">
                <el-form-item label="部门名称" prop="Name" :rules="Name_rules"><el-input size="mini" v-model="record.Name"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="负责人" prop="">
                    <el-select size="mini" multiple filterable v-model="Managers" style="width:340px;">
                        <el-option style="width:400px;" v-for="(item,index) in ManagerID_options" :label="item.label" :value="item.value" key="item.value">
                            <span style="float: left;width:100px;">{{item.label }}</span>
                            <span style="float: left;padding-left:20px;" v-if="item.JobPostion"><b>职位:</b> {{item.JobPostion }}</span>
                            <span style="float: right; width: 120px;" v-if="item.Department"><b>部门:</b> {{ item.Department }}</span>

                        </el-option>
                    </el-select>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="上级部门" prop="SuperiorID">
                    <el-select size="mini" v-model="record.SuperiorID" filterable placeholder="请选择">
                        <el-option label="无" value="">
                        </el-option>
                        <template v-for="(item,index) in SuperiorID_options">
                            <el-option v-if="item.value!=record.ID" :label="item.label" :value="item.value" key="item.label">
                            </el-option>
                        </template>

                    </el-select>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="联系电话" prop="TelePhone"><el-input size="mini" v-model="record.TelePhone"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="备注" prop="Note"><el-input size="mini" v-model="record.Note" type="textarea"></el-input></el-form-item>
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
        props: ['token'],
        data() {
            return {
                Name_rules: [{ required: true, message: '必填!', trigger: 'blur' },],
                ManagerID_options: [],
                SuperiorID_options: [],
                Managers: [],
                record: {
                    ID: null,
                    Name: null,
                    ManagerID: null,
                    SuperiorID: null,
                    Note: null,
                    TelePhone: null,
                }
            };
        },
        watch: {
            token(val) {

            }
        },
        methods: {
            onGet(val) {
                if (val) {
                    this.$get('/baseinfo/department/Get', { id: val }).then(r => {
                        this.record = r;
                        if (this.record.ManagerID)
                            this.Managers = this.record.ManagerID.split(',');
                        else {
                            this.Managers = [];
                        }
                    });
                } else {
                    this.$clearObject(this.record);
                    this.Managers = [];
                }
                this.onListSelectOptions();
                this.onListUsers();
            },
            onListUsers() {
                this.$get("/baseinfo/users/ListSelectOptions").then(r => {
                    this.ManagerID_options = r;
                });
            },
            onListSelectOptions() {
                this.$get("/baseinfo/department/ListSelectOptions").then(r => {
                    this.SuperiorID_options = r;
                });
            },
            submitForm() {
                this.$refs['dataform'].validate((valid) => {
                    if (valid) {
                        this.$confirmMsg('是否保存部门信息?', () => {
                            this.record.ManagerID = this.Managers.join();
                            this.$post('/baseinfo/department/Modify', this.record).then(r => {
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
