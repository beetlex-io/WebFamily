<div class="folder-panel">
    <h3 style="font-size:14pt; padding: 0px; color: #fff; margin: 0px; text-align: center; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #fff;padding-bottom:14px; ">{{this.$getTitle()}}</h3>
    <div class="folder-list">
        <ul class="folder-menus">
            <li :class="[!selectItem?'active-folder':'']" @click="selectItem=null;$emit('select',null)"> <i class="fa-regular fa-folder-closed"></i>所有</li>
            <li v-for="item in folders" :class="[selectItem==item.ID?'active-folder':'']" @click="selectItem=item.ID;$emit('select',item.ID)">
            <i class="fa-regular fa-folder-closed"></i>{{item.Name}}
            <span v-if="item.Total" style="float:right;padding-right:20px;">{{item.Total}}</span>
            </li>
        </ul>
    </div>
</div>
<script>
    export default {
        data() {
            return {
                folders: [],
                selectItem: null,
            };
        },
        methods: {
            onListFolders() {
                this.$get('/admin/FolderList').then(r => {
                    this.folders = r;
                });
            }
        },
        mounted() {
            this.onListFolders();
        }
    }
</script>
<style>
    .folder-menus {
        padding: 0px;
        margin: 0px;
        margin-top: 10px;
        padding-top: 10px;
        position: absolute;
        top: 40px;
        left: 2px;
        right: 0px;
        bottom: 0px;
        overflow-y: auto;
    }


        .folder-menus li i {
            margin-left: 20px;
            margin-right: 10px;
        }

        .folder-menus li {
            border-left-style: solid;
            border-left-width: 4px;
            border-left-color: rgb(255 255 255 / 0.00);
            cursor: pointer;
            color: #fff;
            padding: 10px 0px;
        }

        .folder-menus .active-folder {
            background-color: rgb(255 255 255 / 0.14);
            border-left-style: solid;
            border-left-width: 4px;
            border-left-color: #fff;
            text-shadow: 1px 1px 6px rgb(0 0 0 / 0.80);
        }

        .folder-menus li:hover {
            background-color: rgb(255 255 255 / 0.14);
            border-left-color: #fff;
            text-shadow: 1px 1px 6px rgb(0 0 0 / 0.30);
        }

    .folder-panel {
        position: absolute;
        top: 0px;
        bottom: 0px;
        left: 0px;
        width: 280px;
        background-color: #e9e9e9;
        padding: 10px 0px;
        background: linear-gradient(-45deg, #ee7752, #09aa84, #6641d3, #09aa84);
        background-size: 400% 400%;
        animation: gradient 15s ease infinite;
    }

    @keyframes gradient {
        0% {
            background-position: 0% 50%;
        }

        50% {
            background-position: 100% 50%;
        }

        100% {
            background-position: 0% 50%;
        }
    }
</style>