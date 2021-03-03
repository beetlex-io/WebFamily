<template>
    <el-submenu :index="info.ID">
        <template slot="title">
            <i v-if="info.Img" :class="info.Img" ></i>
            <span>{{info.Name}}</span>
        </template>
        <template v-for="item in info.Childs">
            <webfamily-submenu v-if="item.Childs.length>0" :token="item">

            </webfamily-submenu>
            <el-menu-item v-else :index="item.ID">
                <i v-if="item.Img" :class="item.Img"></i>
                <span slot="title">{{item.Name}}</span>
            </el-menu-item>
        </template>
    </el-submenu>
</template>
<script>
    export default {
        props: ['token'],
        data() {
            return {
                info: this.token
            };
        },
        watch: {
            token(val) {
                this.info = val;
            }
        },
        mounted() {

        }
    }
</script>