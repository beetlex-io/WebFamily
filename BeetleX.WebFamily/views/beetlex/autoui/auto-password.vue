<div>
    <el-button v-if="!hide" :size="size" @click="open" plain>设置</el-button>

    <el-dialog title="设置密码" :visible.sync="opened" @opened="onOpen" width="450px">
        <div>
            <auto-form ref="form" v-model="model" :info="editor" :size="size" @command="onCommand">

            </auto-form>
        </div>

        <div style="text-align:right;padding-right:20px;">
            <el-button :size="size" @click="opened=false">取消</el-button>
            <el-button :size="size" type="primary" @click="onConfirm">确定</el-button>
        </div>
    </el-dialog>
</div>
<script>
    {
        data(){
            return {
                model: {},
                opened: false,
                editor: null,
                token: null,
            }
        },
        methods: {
            onOpen(){
                this.model = {};
                if (!this.editor) {
                    var editor = new autoData();
                    var npwd = editor.addPassword("password", "新密码");
                    npwd.eof = true;
                    if (this.type != 'none') {
                        npwd.require("请输入新密码!").range("密码长度必须6个字符以!", null, null, 6);
                    }
                    var cpwd = editor.addPassword("cpassword", "确认新密码", true);
                    if (this.type != 'none') {
                        cpwd.require('请输入确认密码!');
                    }
                    cpwd.validator((rule, value, callback) => {
                        var password = this.$refs.form.getField('password');
                        var cpassword = this.$refs.form.getField('cpassword');
                        if (password.value != cpassword.value) {
                            if (password.value || cpassword.value) {
                                callback(new Error('确认密码不正确!'));
                                return;
                            }
                        }
                        callback();
                    });

                
                    if (this.type == 'high') {
                        npwd.validator((rule, value, callback) => {
                            var password = this.$refs.form.getField('password');
                            var reg = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,16}$/;
                            if (reg.test(password.value)) {
                                callback();
                            }
                            else {
                                callback(new Error('必须包含大小写字母,数字和特殊符号,8~16位!'));
                            }
                        })
                    }
                    this.editor = editor;
                }
            },
            open(){
                this.model = {};
                this.opened = true;
            },
            onConfirm(){
                if (this.$refs.form.success()) {
                    this.$emit('change', this.model.password);
                    this.$emit('completed', { value: this.model.password, token: this.token });
                    this.opened = false;
                }
            },
            onCommand(e){

            },
        },
        mounted(){

        },
        model: {
            prop: 'value',
                event: 'change',
        },
        props: ['value', 'type', 'size', 'hide']
    }
</script>