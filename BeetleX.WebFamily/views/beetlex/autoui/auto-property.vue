

<el-form-item v-if="info.readonly==true" :label="info.label?info.label:info.name" :style="style">
    <el-input :value="model.value"
              :disabled="true" :style="{width:info.width+'px'}" :placeholder="info.placeholder">
    </el-input>
</el-form-item>

<el-form-item v-else-if="info.type=='label'" :label="info.label?info.label:info.name">
    <label style="width:100%">{{model.value}}</label>
</el-form-item>

<el-form-item v-else-if="info.type=='link'" :label="info.label?info.label:info.name">
    <el-link @click="onCommand">{{model.value}}</el-link>
</el-form-item>

<el-form-item v-else-if="info.type=='button'"  label-width="0" >
    <el-button style="float:right;padding-left:10px;padding-right:10px;" plain @click="onCommand">{{model.value?model.value:info.label}}</el-button>
</el-form-item>

<el-form-item v-else-if="info.type=='iconbutton'" label-width="0" >

    <el-button style="float:right;padding-left:10px;padding-right:10px;" v-if="info.label" type="info" :icon="info.icon" @click="onCommand"><span style="margin-left:4px;">{{info.label}}</span></el-button>
    <el-button v-else style="float:right;padding-left:10px;padding-right:10px;" type="info" :icon="info.icon" circle @click="onCommand"></el-button>
</el-form-item>

<el-form-item v-else-if="info.type=='number'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-input-number :placeholder="info.placeholder" v-model="model.value" :min="-99999999999" :max="99999999999" @change="updateValue" :style="{width:info.width}"></el-input-number>
</el-form-item>

<el-form-item v-else-if="info.type=='date'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-date-picker :placeholder="info.placeholder" v-model="model.value"
                    align="left"
                    type="date" @change="updateValue" :style="{width:info.width}">
    </el-date-picker>
</el-form-item>

<el-form-item v-else-if="info.type=='time'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-time-picker :placeholder="info.placeholder" v-model="model.value" :style="{width:info.width}" @change="updateValue">
    </el-time-picker>
</el-form-item>


<el-form-item v-else-if="info.type=='select'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-select :placeholder="info.placeholder" v-model="model.value" :style="{width:info.width}" @change="updateValue">
        <el-option v-if="item.data && item.nulloption==true">无</el-option>
        <el-option v-if="item.data" v-for="sitem in info.data" :label="sitem.label?sitem.label:sitem.value" :value="sitem.value" :key="sitem.value"></el-option>
    </el-select>
</el-form-item>

<el-form-item v-else-if="info.type=='radio'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-radio-group v-model="model.value" @change="updateValue">
        <el-radio v-if="info.data" v-for="sitem in info.data" :label="sitem.value">{{sitem.label?sitem.label:sitem.value}}</el-radio>
    </el-radio-group>
</el-form-item>

<el-form-item v-else-if="info.type=='checkbox'" :label="info.label?info.label:info.namel" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-checkbox-group v-model="model.value" @change="updateValue">
        <el-checkbox v-if="info.data" v-for="sitem in info.data" :label="sitem.value" :key="sitem.value">{{sitem.label?sitem.label:sitem.value}}</el-checkbox>
    </el-checkbox-group>
</el-form-item>

<el-form-item v-else-if="info.type=='switch'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-switch v-model="model.value" @change="updateValue"></el-switch>
</el-form-item>

<el-form-item v-else-if="info.type=='password'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-input :placeholder="info.placeholder" v-model="model.value" :style="{width:info.width}" show-password @change="updateValue"></el-input>
</el-form-item>

<el-form-item v-else-if="info.type=='remark'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-input :placeholder="info.placeholder" v-model="model.value" :style="{width:info.width}" type="textarea" @change="updateValue" :rows="info.row?info.row:3"></el-input>
</el-form-item>

<el-form-item v-else-if="info.type=='setpassword'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <auto-password v-model="model.value" :style="{width:info.width}" :type="info.pwdtype" size="mini" @completed="updateValue();onCommand()">

    </auto-password>
</el-form-item>

<el-form-item v-else-if="info.type=='rate'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-rate v-model="model.value" @change="updateValue"></el-rate>
</el-form-item>

<el-form-item v-else-if="info.type=='space'" label="">

</el-form-item>

<el-form-item v-else-if="info.type=='upload'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-upload class="upload-demo"
               :action="info.uploadurl"
               :show-file-list="false"
               :on-success="onUploadSuccess"
               :on-error="onUploadError"
               :on-progress="onUploadProcess"
               :before-upload="onBeforeChange">
        <el-button type="primary">点击上传</el-button>
        <div slot="tip" class="el-upload__tip" style="display:contents;">
            <span v-if="!uploadFileName">(大小不超过{{info.filesize?info.filesize:500}}kb)</span>
            <el-tag v-if="uploadFileName" size="small" effect="plain" style="margin-left:10px;">
                {{uploadFileName}}

            </el-tag>

            <el-tag v-if="uploadError" size="small" type="danger" style="margin-left:10px;">{{uploadError}}</el-tag>
            <i v-if="uploading==true" class="el-icon-loading"></i>
        </div>
    </el-upload>
</el-form-item>


<el-form-item v-else-if="info.type=='uploadimg'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <img :src="model.value?model.value:info.defaultimg" :style="{height:info.height?info.height+'px':'100%'}" />
    <auto-uploadimg :title="info.label" :url="info.uploadurl" :fileSize="info.filesize" v-model="model.value" @completed="updateValue();onCommand()"></auto-uploadimg>
</el-form-item>


<el-form-item v-else-if="info.type=='color'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-color-picker v-model="model.value" @change="updateValue"></el-color-picker>
</el-form-item>

<el-form-item v-else-if="info.type=='input-select'" :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-input :placeholder="info.placeholder"  v-model="model.value" class="input-with-select" :style="{width:info.width}" @change="updateValue">
        <el-select v-model="model.value" slot="prepend" @change="updateValue" :placeholder="info.placeholder">
            <el-option v-if="info.data" v-for="sitem in info.data" :label="sitem.label?sitem.label:sitem.value" :value="sitem.value"></el-option>
        </el-select>
    </el-input>
</el-form-item>

<el-form-item v-else :label="info.label?info.label:info.name" :prop="'items.'+info.index+'.value'" :rules="rules">
    <el-input :placeholder="info.placeholder" :style="{width:info.width}" v-model="model.value" @change="updateValue"></el-input>
</el-form-item>

<script>
    {
        props: ["item", "value","style"],
            model: {
            prop: 'value',
                event: 'change',
        },
        data() {
            return {
                model: {
                    value: this.value,
                },
                info: this.item,
                rules: [],
                uploadFileName: '',
                uploadError: '',
                uploading: false,
                uploadImgUrl: '',
            }
        },
        methods: {
            onUploadSuccess(response, file, fileList){
                this.uploading = false;
                this.model.value = response;
                this.updateValue();
            },
            onCommand(){
                this.$emit('command', { field: this.info, value: this.model.value });
            },
            onUploadError(err, file, fileList){
                this.uploadError = err.message;
                this.uploading = false;
            },
            onBeforeChange(file){
                this.uploadError = null;
                var maxSize = this.info.filesize ? this.info.filesize : 500;
                maxSize = maxSize * 1024;
                if (file.size > maxSize) {
                    this.$message.error('上传文件不能超过' + maxSize / 1024 + 'kb!');
                    return false;
                }
                if (this.info.type == 'uploadimg') {
                    if (file.type.indexOf('image') == -1) {
                        this.$message.error('选择的文件不是图片!');
                        return;
                    }
                    var reader = new FileReader();
                    reader.onloadend = () => {
                        this.uploadImgUrl = reader.result;
                    }
                    this.uploadImgUrl = reader.readAsDataURL(file);

                }
                this.uploadFileName = file.name;
                return true;
            },

            onUploadProcess(event, file, fileList){
                this.uploading = true;
            },
            updateValue(){
                this.$emit('change', this.model.value)
                this.info.value = this.model.value;
                this.$emit('valuechange', this.info);
            },
            loadData(){
                if (this.info.dataurl) {
                    var getdata = new beetlexAction(this.info.dataurl);
                    getdata.requested = (r) => {
                        this.info.data = r;
                    };
                    getdata.get();
                }
            },
            setValue(val){
                this.model.value = val;
            },
        },
        mounted(){
            this.loadData();
            if (this.info.rules)
                this.rules = this.info.rules;
            this.info.setValue = this.setValue;
        },
        watch: {
            item(val){
                this.info = val;
                this.model.value = val.value;
                if (this.info.rules)
                    this.rules = this.info.rules;
                this.loadData();
                this.info.setValue = this.setValue;
            },
        }
    }
</script>