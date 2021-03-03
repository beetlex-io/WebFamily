<div style="display:contents;">
    <el-button icon="el-icon-upload2" circle @click="onOpen" :size="size" style="border-style:none;"></el-button>
    <div v-if="opened==true" style="position:fixed;top:0px;bottom:0px;left:0px;right:0px;background-color:rgba(128, 128, 128, 0.47);z-index:900">
        <el-card class="box-card" style="position:absolute;height:420px; width:500px;margin:auto;margin-top:20vh;margin-bottom:70vh;top:0px;bottom:0px;left:0px;right:0px;">
            <div slot="header" class="clearfix">
                <span style="font-size:11pt;">{{uploadTitle}}</span>
            </div>
            <div>
                <div style="text-align:center;height:300px">
                    <el-upload :action="uploadUrl"
                               ref="upload"
                               :auto-upload="false"
                               :show-file-list="false"
                               :on-success="onUploadSuccess"
                               :on-error="onUploadError"
                               :on-progress="onUploadProcess"
                               :on-change="handleChange"
                               :before-upload="onBeforeChange" style="">

                        <i class="el-icon-plus" style="height:32px;width:32px;text-align:center;line-height:32px;border: 1px dashed #d9d9d9"></i>
                        <div slot="tip" class="el-upload__tip" style="display:block;">
                            <span style="display:block;">(大小不超过{{maxSize?maxSize:500}}kb)</span>
                            <img v-if="uploadImgUrl" :src="uploadImgUrl" style="max-height:150px;">
                            <i v-if="uploading==true" class="el-icon-loading"></i>
                            <p v-if="uploadFileName" size="small" effect="plain" style="margin-left:10px;">
                                {{uploadFileName}}

                            </p>

                            <p v-if="uploadError" size="small" type="danger" style="margin-left:10px;color:#ff6a00">上传错误:{{uploadError}}</p>

                        </div>

                    </el-upload>
                </div>

                <div style="text-align:right;">
                    <el-button :size="size" @click="opened=false">取消</el-button>
                    <el-button :size="size" type="primary"  @click="onConfirm">确定</el-button>
                </div>
            </div>
        </el-card>
    </div>
</div>
<script>
    {
        data(){
            return {
                opened: false,
                model: this.value,
                uploadUrl: this.url,
                maxSize: this.fileSize,
                uploading: false,
                uploadImgUrl: null,
                uploadError: null,
                uploadFileName: null,
                uploadTitle: this.title ? this.title:'上传图片'
            }
        },
        methods: {
            onConfirm(){
                if (!this.uploadError)
                    this.$refs.upload.submit();
            },
            onOpen(){
                this.opened = true;
                this.uploading = false;
                this.uploadImgUrl = null;
                this.uploadError = null;
                this.uploadFileName = null;
            },
            handleChange(file, fileList) {
                this.uploadError = null;
                this.uploadFileName = null;
                var reader = new FileReader();
                reader.onloadend = () => {
                    this.uploadImgUrl = reader.result;
                }
                this.uploadImgUrl = reader.readAsDataURL(file.raw);
                this.uploadFileName = file.name;
                var maxSize = this.maxSize ? this.maxSize : 500;
                maxSize = maxSize * 1024;
                if (file.raw.size > maxSize) {
                    this.uploadError = '上传文件不能超过' + maxSize / 1024 + 'kb!';
                    return false;
                }
                if (file.raw.type.indexOf('image') == -1) {
                    this.uploadError = '选择的文件不是图片!';
                    return;
                }

            },
            onUploadSuccess(response, file, fileList){
                console.log(response);
                this.uploading = false;
                this.model = response;
                this.$emit('change', this.model);
                this.opened = false;
            },
            onUploadError(err, file, fileList){
                this.uploadError = err.message;
                this.uploading = false;
            },
            onBeforeChange(file){
                this.uploadError = null;


                return true;
            },

            onUploadProcess(event, file, fileList){
                this.uploading = true;
            },
        },
        watch: {
            url(val){
                this.uploadUrl = val;
            },
            fileSize(val){
                this.maxSize = val;
            },
            title(val){
                this.uploadTitle = val;
            }
        },
        model: {
            prop: 'value',
                event: 'change',
        },
        props: ['value', 'url', 'fileSize', 'size','title']
    }
</script>