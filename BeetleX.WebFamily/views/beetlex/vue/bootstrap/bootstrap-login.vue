<div style="padding-top:100px; width:450px;margin:auto;">

    <div class="card">
        <h5 class="card-header">{{$getTitle()}}</h5>
        <div class="card-body" style="padding-top:50px;">
            <form id="login-form" class="row g-3 needs-validation">
                <div class="mb-3 row">
                    <label for="loginName" class="col-sm-3 col-form-label">用户名</label>
                    <div class="col-sm-9">
                        <input type="text" class="form-control form-control-sm" v-model="record.Name" id="logiName" placeholder="admin" required>
                        <div class="invalid-feedback">
                            输入登陆用户名!
                        </div>
                    </div>
                </div>
                <div class="mb-3 row">
                    <label for="loginPwd" class="col-sm-3 col-form-label">密码</label>
                    <div class="col-sm-9">
                        <input type="password" class="form-control form-control-sm" v-model="record.Password" id="loginPwd" placeholder="123456" required>
                        <div class="invalid-feedback">
                            输入登陆密码!
                        </div>
                    </div>
                </div>
                <div class="mb-3 row">
                    <label for="loginPwd" class="col-sm-3 col-form-label"></label>
                    <div class="col-sm-9" style="text-align:right;">
                        <button type="button" @click="onLogin" class="btn btn-sm btn-secondary">登陆</button>
                    </div>
                </div>
            </form>
         
        </div>
        <div class="card-footer text-muted" style="text-align:right;">
            Web SPA plug-in for <a href="http://beetlex.io" target="_blank">BeetleX</a>
        </div>
    </div> 
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