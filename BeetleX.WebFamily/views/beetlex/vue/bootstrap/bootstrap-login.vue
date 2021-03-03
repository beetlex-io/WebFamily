<div style="padding-top:100px; width:500px;margin:auto;">
    <h4 style="text-align:center;margin-bottom:40px;">用户登陆</h4>
    <form id="login-form" class="row g-3 needs-validation">
        <div class="mb-3 row">
            <label for="loginName" class="col-sm-2 col-form-label">用户名</label>
            <div class="col-sm-10">
                <input type="text" class="form-control form-control-sm" v-model="record.Name" id="logiName" required>
                <div class="invalid-feedback">
                    输入登陆用户名!
                </div>
            </div>
        </div>
        <div class="mb-3 row">
            <label for="loginPwd" class="col-sm-2 col-form-label">密码</label>
            <div class="col-sm-10">
                <input type="password" class="form-control form-control-sm" v-model="record.Password" id="loginPwd" required>
                <div class="invalid-feedback">
                    输入登陆密码!
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button type="button" @click="onLogin" class="btn btn-sm btn-secondary">登陆</button>
        </div>
    </form>

</div>
<script>
    export default {
        data() {
            return {
                record: {
                    Name: null,
                    Password: null,
                },
            };
        },
        methods: {
            onLogin() {
                
                var form = document.getElementById('login-form');
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                
                }
                else {
                    this.$post('/website/login', this.record).then(r => {
                        this.$userLogin(this.record.Name);
                    });
                }
                form.classList.add('was-validated')
            }
        },
        mounted() {

        }
    }
</script>