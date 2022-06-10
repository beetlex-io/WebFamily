<div style="padding:10px">
    <el-form label-position="right" label-width="80px" size="mini">
        <el-form-item label="标题">
            <el-input v-model="data.Title"></el-input>
        </el-form-item>
        <el-form-item label="文章目录">
            <el-select placeholder="请选择" v-model="data.Folder">
                <el-option label="无" value="">
                </el-option>
                <el-option v-for="item in folders"
                           :key="item.ID"
                           :label="item.Name"
                           :value="item.ID">
                </el-option>
            </el-select>
        </el-form-item>
        <el-form-item label="标签">
            <el-input v-model="data.Tag"></el-input>
        </el-form-item>
        <el-form-item label="发布">
            <el-switch v-model="data.Enabled">
            </el-switch>
        </el-form-item>
        <el-form-item style="margin-left:0px;">
            <helpviewer-doc-md maxHeight="500" v-model="data.Content" ref="editor" rows="20"></helpviewer-doc-md>
        </el-form-item>
        <el-form-item>
            <el-button @click="onSave" style="float:right;">保存</el-button>
        </el-form-item>
    </el-form>

</div>
<script>
    export default {
        props: ['token'],
        data() {
            return {
                folders: [],
                data: {
                    ID: '',
                    Title: '',
                    Folder: '',
                    Tag: '',
                    SourceUrl: '',
                    Enabled: false,
                    Content: '',
                    Summary: '',
                },
            };
        },
        methods: {
            onListFolders() {
                this.$get('/admin/FolderList').then(r => {
                    this.folders = r;
                });
            },
            onSave() {
                if (!this.data.Title) {
                    this.$errorMsg('请输入文章标题!');
                    return;
                }
                if (!this.data.Content) {
                    this.$errorMsg('请输入文章内容!');
                    return;
                }
                this.data.Summary = this.$refs.editor.getData().text;
                console.log(this.data);
                this.$post('/admin/DocumentModify', this.data).then(r => {
                    if (this.data.ID) {
                        this.$emit('close', false);
                    }
                    else {
                        this.$emit('close', true);
                    }
                });
            },
            onLoad() {
                this.onListFolders();
                if (this.token && this.token.ID) {
                    this.$get('/admin/DocumentGet', { id: this.token.ID }).then(r => {
                        this.data = r;
                    });
                }
                else {
                    this.$clearObject(this.data)
                    this.data.Enabled = false;
                }

            },
        },
        watch: {
            token(val) {
                this.token = val;
                this.onLoad();
            },
        },
        mounted() {
            this.onLoad();
        }
    }
</script>