<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="12">
                <el-form-item label="员工编号" prop="WorkNumber" :rules="WorkNumber_rules"><el-input size="mini" v-model="record.WorkNumber"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="姓名" prop="Name" :rules="Name_rules"><el-input size="mini" v-model="record.Name"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="邮件地址" prop="EMail" :rules="EMail_rules"><el-input size="mini" v-model="record.EMail"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="职位" prop="JobPostion" filterable>
                    <el-select size="mini" v-model="record.JobPostion" style="width: 210px;">
                        <el-option v-for="(item,index) in JobPostion_options" :label="item.label" :value="item.value" key="item.value"></el-option>
                    </el-select>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="手机" prop="MobilePhone" :rules="MobilePhone_rules"><el-input size="mini" v-model="record.MobilePhone"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="联系电话" prop="TelePhone">
                    <el-input size="mini" v-model="record.TelePhone"></el-input>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="所属部门" prop="DepartmentID">
                    <el-select size="mini" v-model="record.DepartmentID" filterable style="width: 210px;">
                        <el-option label="无" value="">
                        </el-option>
                        <el-option v-for="(item,index) in DepartmentID_options" :label="item.label" :value="item.value" key="item.value">

                        </el-option>
                    </el-select>
                </el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="上级负责人" prop="SuperiorID">
                    <el-select size="mini" v-model="record.SuperiorID" filterable style="width: 210px;">
                        <el-option label="无" value="">
                        </el-option>
                        <el-option style="width:400px;" v-for="(item,index) in SuperiorID_options" v-if="record.ID!=item.value" :label="item.label" :value="item.value" key="item.value">
                            <span style="float: left;width:100px;">{{item.label }}</span>
                            <span style="float: left;padding-left:20px;" v-if="item.JobPostion"><b>职位:</b> {{item.JobPostion }}</span>
                            <span style="float: right; width: 120px;" v-if="item.Department"><b>部门:</b> {{ item.Department }}</span>
                        </el-option>
                    </el-select>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="入职日期" prop="EntryDay">
                    <el-date-picker size="mini" v-model="record.EntryDay" align="left" type="date" style="width: 210px;"></el-date-picker>
                </el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="出生日期" prop="BirthDay">
                    <el-date-picker size="mini" v-model="record.BirthDay" align="left" type="date" style="width: 210px;"></el-date-picker>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="性别" prop="Sex" style="width: 210px;">
                    <el-select size="mini" v-model="record.Sex"><el-option v-for="(item,index) in Sex_options" :label="item.label" :value="item.value" key="item.value"></el-option> </el-select>
                </el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="头像地址" prop="Icon">
                    <el-input size="mini" v-model="record.Icon"></el-input>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="联系地址" prop="Address">
                    <el-input size="mini" v-model="record.Address"></el-input>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="微信ID" prop="WeiXunID">
                    <el-input size="mini" v-model="record.WeiXunID"></el-input>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="角色" prop="role">
                    <el-select size="mini" v-model="roles" filterable multiple style="width:540px;">
                        <el-option v-for="(item,index) in role_options" :label="item.label" :value="item.value" key="item.value">
                            <span style="float: left;width:100px;">{{item.label }}</span>
                            <span style="float: inherit;padding-left:20px;">{{item.Note}}</span>
                        </el-option>
                    </el-select>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="">
                    <el-button size="mini" @click="passwordDialogVisible=true;" icon="fa-solid fa-key">设置密码</el-button>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="系统管理员" prop="IsAdmin">
                    <el-switch size="mini" :disabled="record.SystemData" v-model="record.IsAdmin" type="textarea"></el-switch>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="备注" prop="Note">
                    <el-input size="mini" v-model="record.Note" type="textarea"></el-input>
                </el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col span="24" style="text-align:right">
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="$emit('close',false)">取消</el-button>
                <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="submitForm">保存</el-button>
            </el-col>
        </el-row>
    </el-form>
    <el-dialog top="30vh" title="用户权限" :visible.sync="passwordDialogVisible" @opened="onOpenpassword" width="400px" :append-to-body="true" :close-on-click-modal="false">
        <baseinfo-user-changepassword @close="passwordDialogVisible=false" ref="passwordEditor"></baseinfo-user-changepassword>
    </el-dialog>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                WorkNumber_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                Name_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                EMail_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                JobPostion_options: [],
                MobilePhone_rules: [{ required: true, message: '必填！', trigger: 'blur' },],
                DepartmentID_options: [],
                SuperiorID_options: [],
                role_options: [],
                Sex_options: [{ value: '男', label: '男' }, { value: '女', label: '女' }],
                roles: [],
                passwordDialogVisible: false,
                record: {
                    ID: null,
                    WorkNumber: null,
                    Name: null,
                    EMail: null,
                    JobPostion: null,
                    MobilePhone: null,
                    TelePhone: null,
                    DepartmentID: null,
                    SuperiorID: null,
                    EntryDay: null,
                    BirthDay: null,
                    Sex: '男',
                    Icon: null,
                    Address: null,
                    WeiXunID: null,
                    Note: null,
                    Enabled: false,
                    Roles: '',
                    IsAdmin: false,
                }
            };
        },
        methods: {
            onOpenpassword() {
                this.$refs.passwordEditor.onGet(this.record.ID);
            },
            onGet(id) {
                if (id) {
                    this.$get('/baseinfo/users/get', { id: id }).then(r => {
                        this.record = r;
                        if (this.record.Roles)
                            this.roles = this.record.Roles.split(',');
                        else {
                            this.roles = [];
                        }
                    });
                }
                else {
                    this.$clearObject(this.record);
                    this.record.IsAdmin = false;
                    this.record.Enabled = false;
                    this.roles = [];
                }
                this.onListDepartmentOpetions();
                this.onListUserOptions();
                this.onListJobPostionOptions();
                this.onListRoleOptions();

            },
            onListRoleOptions() {
                this.$get('/baseinfo/roles/ListSelectOptions').then(r => {
                    this.role_options = r;
                });
            },
            onListJobPostionOptions() {
                this.$get('/baseinfo/properties/ListSelectOptions', { category: '职位' }).then(r => {
                    this.JobPostion_options = r;
                });
            },
            onListUserOptions() {
                this.$get("/baseinfo/users/ListSelectOptions").then(r => {
                    this.SuperiorID_options = r;
                });
            },
            onListDepartmentOpetions() {
                this.$get("/baseinfo/department/ListSelectOptions").then(r => {
                    this.DepartmentID_options = r;
                });
            },
            submitForm() {
                this.$refs['dataform'].validate((valid) => {

                    if (valid) {
                        this.record.Roles = this.roles.join();
                        this.$confirmMsg('是否保存用户信息?', () => {
                            this.$post('/baseinfo/users/Modify', this.record).then(r => {
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
