<div>

    <el-form :style="formStyle" :class="formClass" v-if="rows.length==1" :inline="true" :size="size" :label-width="rows.labelwidth+'px'" :model="data" ref="customForm">
        <auto-property v-for="(item,col) in rows[0]" v-if="!item.hide" v-model="item.value" :item="item" @valuechange="valueChange" @command="onCommand">
        </auto-property>
    </el-form>

    <el-form v-else :size="size" :label-width="labelwidth+'px'" :model="data" ref="customForm" :style="formStyle" :class="formClass">
        <el-row v-for="row in rows">
            <el-col v-for="(item,col) in row" v-if="!item.hide" :span="onGetWidth(col+1,item,row)">
                <div class="grid-content bg-purple">
                    <auto-property v-model="item.value" :item="item" @valuechange="valueChange" @command="onCommand">

                    </auto-property>
                </div>
            </el-col>
        </el-row>
    </el-form>
</div>
<script>
    {
        props: ["info", "value", "size", "url", "style", "class"],
            data(){
            return {
                rows: [],
                data: { items: [] },
                labelwidth: this.info ? (this.info.labelwidth ? this.info.labelwidth : 120) : 120,
                dataUrl: this.url,
                sourceData: this.value,
                formStyle: this.style,
                formClass: this.class,
            }
        },
        model: {
            prop: 'value',
                event: 'change',
        },
        methods: {
            onGetWidth(col, item, row){
                if (row.length == 1)
                    return 24;
                var max = 24;
                var buttons = 0;
                if (item.type.indexOf('button') >= 0)
                    return 2;
                row.forEach(v => {
                    if (v.type.indexOf('button') >= 0)
                        buttons++;
                });
                max = max - buttons*2;
                var width = parseInt(max / (row.length - buttons));
                return width;

            },
            onSave(e){
                var action = new beetlexAction(e.field.saveurl);
                action.requested = (r) => {

                };
                action.post(e.data);
            },
            onCommand(e){
                e.data = this.getData();

                var success;
                this.$refs['customForm'].validate((valid) => {
                    success = valid;
                })
                e.success = success;
                if (e.field.saveurl) {
                    this.onSave(e);
                }
                else {
                    this.$emit('command', e);
                }
            },
            setInfo(val)
            {
                if (val.labelwidth)
                    this.labelwidth = val.labelwidth;
                else
                    this.labelwidth = 120;
                this.init(val.items, val.data);
            },
            init(items, data){

                this.data.items = [];
                this.rows = [];
                var rowItems = [];
                for (i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (item.type == 'checkbox' && !item.value)
                        item.value = [];
                    item.index = i;
                    this.data.items.push(item);
                    rowItems.push(item);
                    if (item.eof == true) {
                        this.rows.push(rowItems);
                        rowItems = [];
                    }
                }
                if (rowItems.length > 0)
                    this.rows.push(rowItems);
                this.$refs['customForm'].clearValidate();
                if (data) {
                    this.setData(data);
                }
            },

            getField(name){
                var result;
                this.data.items.forEach(v => {
                    if (name.toLowerCase() == v.name.toLowerCase()) {
                        result = v;
                    }
                });
                return result;
            },
            valueChange(info){
                this.$emit('change', this.getData());
                // this.init(this.data.items);
                //this.$refs['customForm'].validate((valid) => {
                //    result = valid;
                //});
                this.$emit('fieldchange', { item: info, properties: this.data.items })
            },
            success(){
                var result = null;
                this.$refs['customForm'].validate((valid) => {
                    result = valid;
                });
                return result;
            },
            getData(){
                if (!this.sourceData)
                    this.sourceData = new Object();
                var result = this.sourceData;
                for (i = 0; i < this.data.items.length; i++) {
                    if (!this.data.items[i].hide)
                        this.getProperty(result, this.data.items[i]);
                }
                return result;
            },
            setData(val){
                if (val) {
                    for (i = 0; i < this.data.items.length; i++)
                        this.setProperty(val, this.data.items[i]);
                }
            },
            setProperty(obj, info){
                if (info.type == 'button' || info.type == 'link') {
                    return;
                }
                if (info.type == 'checkbox' && !obj) {
                    info.value = [];
                }
                else {
                    if (obj[info.name])
                        info.value = obj[info.name];
                    else
                        info.value = null;
                }

                if (info.setValue) {
                    info.setValue(info.value);

                }
            },
            getProperty(obj, info){
                if (info.parent) {
                    if (!obj[info.parent])
                        obj[info.parent] = new Object();
                    obj[info.parent][info.name] = info.value;
                }
                else {
                    obj[info.name] = info.value;
                }
            },
            loadData(url){
                if (url && beetlex) {
                    var getDetail = new beetlexAction('/beetlex/apidoc/GetApiDetail');
                    getDetail.requested = (r) => {
                        this.column = r.col;
                        this.labelwidth = r.labelwidth;
                        this.init(r.items);
                        this.setData(this.value);
                        this.$emit('change', this.getData());
                        this.$emit('completed', r.items);
                    };
                    getDetail.get({ url: url });
                }
            }

        },
        mounted(){
            if (this.info)
                this.init(this.info.items);
            this.setData(this.sourceData);
            this.loadData(this.dataUrl);
        },
        watch: {
            info(val){
                this.setInfo(val);
            },
            value(val)
            {
                this.sourceData = val;
                this.setData(val);

            },
            url(val){
                this.dataUrl = val;
                this.loadData(this.dataUrl);
            },
        }
    }
</script>