<div style="width:1260px;margin:auto;">
    <div id="document-view" v-html="compiledCommentMarkdown" class="post-detail-view" style="background-color: #fff; padding: 5px;">

    </div>
</div>
<script>
    export default {
        props: ['token'],
        data() {
            return {
                document: {
                    Content: ''
                }
            };
        },
        methods: {
            onLoad() {
                if (this.token && this.token.ID) {
                    this.$get('/admin/DocumentGet', { id: this.token.ID }).then(r => {
                        this.document = r;
                    });
                }
                else {
                    this.$clearObject(this.document)
                }

            }
        },
        watch: {
            token(val) {
                this.token = val;
                this.onLoad();
            },
        },
        computed: {
            compiledCommentMarkdown() {
                if (!this.document.Content)
                    return '';
                var result = marked(this.document.Content);
                result = this.$changeImg(result, 400, true);
                setTimeout(() => {
                    document.querySelectorAll('pre code').forEach((block) => {
                        hljs.highlightBlock(block);
                    })
                    setTimeout(() => {
                        var collections = document.getElementsByClassName("el-dialog__body");
                        for (i = 0; i < collections.length; i++)
                            collections[i].scrollTop = 0;

                    }, 100);
                }, 200);
                return result;
            }
        },
        mounted() {
            this.onLoad();
        }
    }
</script>