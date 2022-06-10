<div :style="style" class="md-editor">
    <el-tabs v-model="isEdit" type="card" @tab-click="if(isEdit!='编辑'){onView()}" size="mini">
        <el-tab-pane name="编辑">
            <div slot="label">
                <span>
                    编辑(Markdown)
                </span>
            </div>
            <div style="display:inline">
                <div style="display:inline-block;padding:5px 0px;">


                    <i class="el-icon-loading" v-if="uploading" style="position: absolute; margin-top: -16px;"></i>
                    <input @change="onSelectFile" id="file-input" multiple type="file" name="name" style="display: none;" />
                    <el-button size="mini" @click="addTag">
                        <i class="fa-solid fa-indent"></i>
                    </el-button>
                    <el-button size="mini" @click="document.getElementById('file-input').click()">
                        <i class="fa-solid fa-upload"></i>
                    </el-button>
                </div>
            </div>
            <textarea id="txtDocument" @change="onChange" :rows="rows" v-model="text" @drop="onFileDrop" @paste="onFilePaste"
                      class="el-textarea__inner" @keyup="autoheight($event)" style="border-radius:0px;">
                                        
            </textarea>
            <div v-html="compiledCommentMarkdown" class="post-detail-view" id="editViewer1" style="display:none">

            </div>
        </el-tab-pane>
        <el-tab-pane label="预览" name="预览">
            <div v-html="compiledCommentMarkdown" class="post-detail-view" id="editViewer" style="background-color: #fff; padding: 5px; height: 80px;overflow-y:auto">

            </div>
            <br />
        </el-tab-pane>
    </el-tabs>

</div>
<style>
    .md-editor .el-tabs__content {
        overflow: hidden;
        position: relative;
        padding: 10px;
        border-style: solid;
        border-width: 1px;
        border-color: #e4e7ed;
        border-top-style: none;
    }
    .md-editor .el-tabs__item:hover {
        color: #409EFF;
        cursor: pointer
    }
   

    .md-editor .el-tabs__header {
        margin: 0px;
    }
</style>
<script>
    export default {
        props: ["maxHeight", "style", "value", "rows"],
        data() {
            return {
                isEdit: '编辑',
                text: '',
                textBox: null,
                uploading: false,
                showfaces: false,

            };
        },
        model: {
            prop: 'value',
            event: 'change',
        },
        methods: {
            getData() {
                var text = document.getElementById('editViewer1').innerText;
                var html = marked(this.text);
                return { html: html, text: text }
            },
            onView() {
                setTimeout(() => {
                    document.querySelectorAll('pre code').forEach((block) => {
                        hljs.highlightBlock(block);
                    })
                }, 200);
            },
            onChange() {
                this.$emit('change', this.text);
            },
            autoheight(x) {
                this.textBox = x.target;
                var value = 30;
                x.target.style.height = value + 'px';
                var max = 200;
                if (this.maxHeight)
                    max = this.maxHeight;
                value = (30 + x.target.scrollHeight);
                if (value > max)
                    value = max;
                value = parseInt(value);
                x.target.style.height = value + "px";
                this.$emit('resize', value);
                var view = document.getElementById('editViewer');
                view.style.height = (value + 40) + 'px';

            },
            addTag() {
                this.onInsertUrl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            },
            onInsertUrl(url) {
                var el = document.getElementById("txtDocument");
                var newText = url;
                var start = el.selectionStart;
                var end = el.selectionEnd;
                var oldtext = el.value;
                var before = oldtext.substring(0, start)
                var after = oldtext.substring(end, oldtext.length)
                el.value = (before + newText + after)
                el.selectionStart = el.selectionEnd = start + newText.length
                el.focus()
                this.text = el.value;
                this.onChange();
            },
            onFilePaste: function (event) {
                var items = event.clipboardData.items;
                console.log("upload paste", event);

                if (items.length > 0) {
                    if (items[0].kind == 'file') {
                        var file = items[0].getAsFile();
                        console.log('upload paste', file);
                        this.onUploadFile(file);
                    }
                }
                var imgdata = event.clipboardData.getData('text/html');
                if (imgdata) {
                    var regex = /<img.*?src="(.*?)"/;
                    var src = regex.exec(imgdata.toString())[1];
                    if (src) {
                        console.log("upload paste url", src);
                        var action = new beetlexAction('/admin/UploadImageUrl');
                        action.http = true;
                        action.asyncget({ url: src }).then(r => {
                            console.log("upload result ", r);
                            str = '![](' + r + ')\r\n';
                            this.onInsertUrl(str);
                        });
                    }
                    return;
                }
            },
            onFileDrop: function (event) {
                event.preventDefault();
                console.log('upload Drop', event.dataTransfer.files[0]);
                for (i = 0; i < event.dataTransfer.files.length; i++)
                    this.onUploadFile(event.dataTransfer.files[i]);
            },
            onSelectFile: function (e) {
                //console.log(e);
                if (e.target.files.length > 0) {
                    for (i = 0; i < e.target.files.length; i++) {
                        this.onUploadFile(e.target.files[i]);
                    }
                    document.getElementById('file-input').value = null;
                }
            },
            onUploadFile(file) {
                var match = /((\.jpeg)|(\.jpg)|(\.png)|(\.zip)|(\.rar)){1}/ig

                var _this = this;
                var filename = file.name.toLowerCase();
                if (!filename.match(match)) {
                    this.$errorMsg(file.name + '只支持(jpg, png, zip或rar)文件上传!')
                    return;
                }
                var isImg = file.type.indexOf('image/') >= 0;
                if (file.size > 1024 * 1024 * page.uploadMaxSize) {
                    this.$errorMsg(file.name + '超过' + page.uploadMaxSize + 'MB!');
                    return;
                }

                var uploadhandler = (url, data) => {
                    console.log("upload data", url, data);
                    this.uploading = true;
                    this.$putCatch(url, data)
                        .then((response) => {
                            console.log('upload completed', response);
                            var str;
                            if (isImg)
                                str = '![](' + response + ')\r\n';
                            else
                                str = '[' + filename + '](' + response + ')\r\n';
                            _this.onInsertUrl(str);
                            this.uploading = false;
                        })
                        .catch((error) => {
                            console.log('upload error', error.response);
                            this.$errorMsg(error.response.data);
                            this.uploading = false;
                        });
                };
                var filereader = new FileReader();
                filereader.onload = (event) => {
                    if (isImg) {
                        uploadhandler("/admin/UploadFile?name=" + encodeURI(filename) + "&isimage=true", event.target.result);
                    }
                    else {
                        uploadhandler("/admin/UploadFile?name=" + encodeURI(filename) + "&isimage=false", event.target.result);
                    }
                };
                filereader.readAsArrayBuffer(file);
            },
            onResizeImage: function (file, callback) {
                var rotate = 0;
                var execute = () => {
                    var filename = file.name;
                    var img = document.createElement("img");
                    img.onload = function () {
                        var canvas = document.createElement("canvas");
                        var MAX_WIDTH = 1920;
                        var MAX_HEIGHT = 1080;
                        var width = img.width;
                        var height = img.height;
                        if (rotate == 90 || rotate == 270) {
                            var h = width;
                            width = height;
                            height = h;
                        }
                        if (width > height) {
                            if (width > MAX_WIDTH) {
                                height *= MAX_WIDTH / width;
                                width = MAX_WIDTH;
                            }
                        } else {
                            if (height > MAX_HEIGHT) {
                                width *= MAX_HEIGHT / height;
                                height = MAX_HEIGHT;
                            }
                        }
                        canvas.width = width;
                        canvas.height = height;
                        var ctx = canvas.getContext("2d");
                        if (rotate == 270) {
                            ctx.rotate(270 * Math.PI / 180);
                            ctx.drawImage(img, -height, 0, height, width);
                        }
                        else if (rotate == 180) {
                            ctx.rotate(180 * Math.PI / 180);
                            ctx.drawImage(img, -width, -height, width, height);
                        }
                        else if (rotate == 90) {
                            ctx.rotate(90 * Math.PI / 180);
                            ctx.drawImage(img, 0, -width, height, width);
                        }
                        else {
                            ctx.drawImage(img, 0, 0, width, height);
                        }

                        var dataUrl = canvas.toDataURL('image/jpeg', 0.98);
                        var byteString = atob(dataUrl.split(',')[1]);
                        var ab = new ArrayBuffer(byteString.length);
                        var ia = new Uint8Array(ab);
                        for (var i = 0; i < byteString.length; i++) {
                            ia[i] = byteString.charCodeAt(i);
                        }
                        callback(filename, ab);
                    }
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        img.src = e.target.result;
                    }
                    reader.readAsDataURL(file);
                }
                EXIF.getData(file, function () {
                    var _orientation = EXIF.getTag(this, 'Orientation');
                    if (_orientation == 3) {
                        rotate = 180;
                    } else if (_orientation == 6) {
                        rotate = 90;
                    } else if (_orientation == 8) {
                        rotate = 270;
                    } else {
                        rotate = 0;
                    }
                    execute();
                });
            },
        },
        watch: {
            value(val) {
                this.text = val;
                this.isEdit = '编辑';
                if (this.rows) {
                    var view = document.getElementById('editViewer');
                    view.style.height = parseInt(this.rows * 20) + 'px';
                }
            },
        },
        computed: {
            compiledCommentMarkdown() {
                if (!this.text)
                    return '';
                var result = marked(this.text);
                result = this.$changeImg(result, 800, true);
                return result;
            }
        }
    }
</script>