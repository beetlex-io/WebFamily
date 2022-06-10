<div class="viewer-header">

    <span style="padding-right:10px;float:left;"><i class="fa-regular fa-user" style="padding-right:5px;"></i>{{this.$getUserInfo().Name}}({{this.$getUserInfo().Type}})</span>
    <el-button size="mini" v-if="user.role.toLowerCase()=='admin'" @click="$emit('add')" circle><i class="fa-solid fa-file-circle-plus"></i></el-button>

    <el-button size="mini" v-if="user.role.toLowerCase()=='admin'" @click="dialogVisible=true" circle><i class="fa-solid fa-screwdriver-wrench"></i></el-button>
    <el-button size="mini" @click="$userSignout()" circle><i class="fa-solid fa-right-from-bracket"></i></el-button>
    <el-dialog title="管理" :visible.sync="dialogVisible" :close-on-click-modal="false" :fullscreen="true">
        <helpviewer-setting></helpviewer-setting>
    </el-dialog>
</div>
<script>
    export default {
        data() {
            return {
                dialogVisible: false,
                user: { name: '', role: '' }
            };
        },
        methods: {

        },
        mounted() {
            this.user = this.$getUser();
        }
    }
</script>
<style>
    .el-dialog__body {
        padding: 20px 20px;
        color: #606266;
        font-size: 14px;
        word-break: break-all;
    }

    .viewer-header {
        position: absolute;
        left: 280px;
        right: 0px;
        height: 30px;
        padding: 8px 20px;
        text-align: right;
        border-bottom-color: #bbbbbb;
        border-bottom-width: 1px;
        border-bottom-style: solid;
    }

        .viewer-header .el-dialog__body {
            position: absolute;
            top: 40px;
            left: 10px;
            right: 10px;
            bottom: 10px;
            overflow-y: auto;
            padding: 0px;
        }

        .viewer-header .el-dialog__header {
            font-weight: bold;
        }

        .viewer-header .el-dialog {
            border-radius: 6px;
            max-width: 1280px;
            margin: auto;
        }
</style>