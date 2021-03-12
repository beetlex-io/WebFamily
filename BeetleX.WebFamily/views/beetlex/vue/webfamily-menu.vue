<el-menu :default-active="active" :mode="menutype" @select="handleOpen" class="menu-bar" :collapse="full!='min'" size="small">
    <template v-for="item in menus">
        <el-submenu v-if="item.Childs.length>0" :index="item.ID"  v-bind:key="item.ID">
            <template slot="title">
                <i v-if="item.Img" :class="item.Img"></i>
                <span>{{item.Name}}</span>
            </template>

            <el-menu-item v-for="sitem in item.Childs" :index="sitem.ID" v-bind:key="sitem.ID">
                <i v-if="sitem.Img" :class="sitem.Img"></i>
                <span slot="title">{{sitem.Name}}</span>
            </el-menu-item>

        </el-submenu>
        <el-menu-item v-else :index="item.ID">
            <i v-if="item.Img" :class="item.Img"></i>
            <span slot="title">{{item.Name}}</span>
        </el-menu-item>
    </template>
</el-menu>


<script>
    export default {
        props: ['token', 'size', 'menutype'],
        data: function () {
            return {
                full: this.size,
                webTitle: this.title,
                menus: this.token ? this.token : [],
                active: '',
            }
        },
        methods: {
            handleOpen(key, keyPath) {
                switch (key) {
                    case 'extend-menu':
                        this.OnResizeMenu('min');
                        break;
                    default:
                        this.$openMenu(key);
                        break;
                }
            },

            OnResizeMenu(value) {
                this.full = value;
                this.$emit('resize', value)
            },
        },
        watch: {
            size(val) {
                this.full = val;
            },
            token(val) {
                this.menus = val;

                if (this.menus.length > 0)
                    this.active = this.menus[0].ID;
            },
            title(val) {
                this.webTitle = val;
            }
        },
        mounted: function () {

        }
    }
</script>