var page = {};

beetlex.errorHandlers['401'] = function () {
    page.OnOpenLogin();
};
Vue.prototype.$getID = function () {
    page.controlid = page.controlid + 1;
    return page.controlid;
};
Vue.prototype.$pformat = function (num, total) {
    var value = (Math.round(num / total * 10000) / 100.00);
    if (value > 100)
        value = 100;
    if (value < 0)
        value = 0;
    return total <= 0 ? "0%" : value + "%";
};

Vue.prototype.$getTitle = function () {
    return page.title;
};

Vue.prototype.$toHome = function () {
    page.onToHome();
};
Vue.prototype.$userLogin = function (name) {
    page.onLogin(name);
};
Vue.prototype.$userSignout = function () {
    page.onSignout();
};
Vue.prototype.$openLogin = function () {
    page.OnOpenLogin();
};
Vue.prototype.$loadWebInfo = function () {
    page.OnLoadInfo();
};
Vue.prototype.$closeWindow = function (id) {
    page.OnCloseWindows(id);
};
Vue.prototype.$openWindow = function (id, title, model, data) {
    page.OnOpenWindow({ id: id, title: title, model: model, data: data });
};
Vue.prototype.$getWindows = function () {
    return page.OnGetWindows();
};
Vue.prototype.$openMenu = function (id) {
    page.onOpenMenu(id);
};
Vue.prototype.$activeWindow = function () {
    return page.activeModel;
};
Vue.prototype.$getUser = function () {
    return { name: page.user, role: page.role };
};
Vue.prototype.$addResize = function (handler) {
    __addResizeHandler(handler);
};
Vue.prototype.$getSN = function () {
    if (page.sn)
        return page.sn;
    return {};
};
Vue.prototype.$resizeWindow = function () {
    __resize();
};
var pageData = {
    controlid: 1,
    windows: [],
    menuSize: 'min',
    menus: [],
    mustLogin: true,
    user: '',
    role: '',
    loginModel: 'webfamily-login',
    headerModel: '',
    homeModel: '',
    homeName: 'Home',
    footerModel: '',
    logoImg: '/images/logo.png',
    title: '',
    activeName: 'home',
    tabsEnabled: true,
    parentName: '',
    loginDialogVisible: false,
    activeModel: {
        model: '', data: {}, id: ''
    },
    loginHandler: null,
    sn: {},
    appName: 'null'
};
page = new Vue({
    el: '#app',
    data: pageData,
    methods: {
        OnGetWindows() {
            return this.windows;
        },
        OnCloseWindows(id) {
            if (id == 'home')
                return;
            var index = -1;
            for (i = 0; i < this.windows.length; i++) {
                if (this.windows[i].id == id) {
                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                this.windows.splice(index, 1);
                if (this.activeModel.id == id || this.activeName == id) {
                    this.activeModel = this.windows[index - 1];
                    this.activeName = this.activeModel.id;
                }
                this.$resizeWindow();
            }
        },
        onLogin(name) {
            window.location.reload();
        },

        onSignout() {
            this.$get('/website/Signout').then(r => {
                this.user = null;
                if (this.mustLogin == true && !this.user) {
                    this.OnOpenLogin();
                }
                else {
                    this.onToHome();
                }
            });
        },
        onToHome() {
            this.$openWindow('home', this.homeName, this.homeModel);
        },
        OnLoadInfo() {
            this.$get('/website/LoadInfo').then(r => {
                this.menus = r.Menus;
                this.mustLogin = r.MustLogin;
                this.loginModel = r.LoginModel
                this.headerModel = r.HeaderModel;
                this.homeModel = r.HomeModel;
                this.logoImg = r.LogoImg;
                this.title = r.Title;
                this.user = r.User;
                this.role = r.Role;
                this.homeName = r.HomeName;
                this.tabsEnabled = r.TabsEnabled;
                this.appName = r.AppName;
                document.title = this.title;
                this.$get('/__GET_SN?name=' + this.appName).then(r => {
                    this.sn = r;
                    console.log("getsn", this.sn);
                });
                if (this.mustLogin == true && !this.user) {
                    this.OnOpenLogin();
                }
                else {
                    this.onToHome();
                }

            });
        },
        onTabClick() {
            this.$resizeWindow();
        },
        onOpenMenu(id) {
            this.menus.forEach(v => {
                var item = this.OnFindMenu(v, id);
                if (item) {
                    this.$openWindow(item.ID, item.Name, item.Model, {});
                    return;
                }
            });
        },
        OnFindMenu(item, key) {
            if (item.ID == key)
                return item;
            for (var i = 0; i < item.Childs.length; i++) {
                var result = this.OnFindMenu(item.Childs[i], key);
                if (result)
                    return result;
            }
            return null;
        },
        OnOpenWindow(e) {
            var has = false;
            for (i = 0; i < this.windows.length; i++) {
                var item = this.windows[i];
                if (item.id == e.id) {
                    has = true;
                }
            }
            if (has == false) {
                this.windows.push(e);
            }
            this.activeModel = e;
            this.activeName = e.id;
            this.$resizeWindow();

        },
        OnOpenLogin: function () {
            if (this.loginHandler) {
                this.loginHandler();
            }
            else {
                this.loginDialogVisible = true;
            }
        },
    },
    mounted() {
        this.OnLoadInfo();

    }
})


