<div>
    <div :class="[full=='min'?'menu_full':'menu_min']">
        <div class="menu-panel">
            <div v-if="full=='min'" class="menu-panel-header">
                <div class="web-logo">
                    <img :src="logo" />

                </div>
                <div class="web-title">{{webTitle}}</div>
                <div class="menu-resize-btn">
                    <a title="最小化菜单" size="mini" @click="OnResizeMenu('max')">
                        <i class="el-icon-caret-left"></i>
                    </a>
                </div>
            </div>
            <div v-else class="menu-resize-min-btn">
                <a title="最大化菜单" size="mini" @click="OnResizeMenu('min')">
                    <i class="el-icon-caret-right"></i>
                </a>
            </div>
            <el-menu default-active="1" @select="handleOpen" class="menu-bar" :collapse="full!='min'" size="small">
                <template v-for="item in menus">
                    <webfamily-submenu v-if="item.Childs.length>0" :token="item">

                    </webfamily-submenu>
                    <el-menu-item v-else :index="item.ID">
                        <i v-if="item.Img" :class="item.Img"></i>
                        <span slot="title">{{item.Name}}</span>
                    </el-menu-item>
                </template>
            </el-menu>
        </div>
    </div>
</div>

<script>
    export default {
        props: ['token', 'title', 'logo'],
        data: function () {
            return {
                full: 'min',
                webTitle: this.title,
                menus: this.token ? this.token : []
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
            token(val) {
                this.menus = val;
                console.log();
            },
            title(val) {
                this.webTitle = val;
            }
        },
        mounted: function () {

        }
    }
</script>