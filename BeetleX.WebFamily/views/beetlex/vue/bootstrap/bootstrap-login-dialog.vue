<div class="modal fade" id="login-modal" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false" aria-hidden="true">
    <div class="modal-dialog modal-fullscreen login-modal">
        <div class="modal-content login-modal">
           
            <div class="modal-body">
                <component :is="loginModel"></component>
            </div>
        </div>
    </div>
</div>


<script>
    export default {
        props: ['login'],
        data() {
            return { loginModel:this.login };
        },
        watch: {
            login(val) {
                this.loginModel = val;
            }
        }
    }
</script>