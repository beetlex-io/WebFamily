
var __resizeHandlers = [];
function __addResizeHandler(handler) {
    __resizeHandlers.push(handler);
};
function __resize() {
    setTimeout(() => {
        __resizeHandlers.forEach(v => {
            v();
        });
    }, 100);
}
window.onresize = function () {
    __resizeHandlers.forEach(v => {
        v();
    });
};



//timer
var __app__timers = [];

var __app__timeCount = 0;

__addTimerHandler = function (callback) {
    __app__timers.push(callback);
};

__runTimer = function () {
    try {
        __app__timeCount++;
        __app__timers.forEach((v) => {
            v(__app__timeCount);
        });
    }
    catch (err) {
        console.log("run timer error", err);
    }
};
setInterval(__runTimer, 1000);

//vue
Vue.prototype.$nformat = function (value) {
    return new Intl.NumberFormat().format(value);
}
Vue.prototype.$confirmMsg = function (msg, callback, cancel) {
    this.$confirm(msg, 'Question', {
        confirmButtonText: 'Confirm',
        cancelButtonText: 'Cancel',
        type: 'warning'
    }).then(() => { callback(); }).catch(() => {
        if (cancel)
            cancel();
    });
};
Vue.prototype.$errorMsg = function (msg) {
    this.$message({
        message: msg,
        type: 'error',
        duration: 5000,
        showClose: true,
    });
};
Vue.prototype.$successMsg = function (msg) {
    this.$message({
        message: msg,
        type: 'success',
        duration: 5000,
        showClose: true,
    });
};
Vue.prototype.$confirmInput = function (msg, title, callback, pattern, errormsg) {
    this.$prompt(msg, title, {
        confirmButtonText: 'Confirm',
        cancelButtonText: 'Cancel',
        inputPattern: pattern,
        inputErrorMessage: errormsg
    }).then((value) => {
        callback(value)
    }).catch(() => { });
}
Vue.prototype.$confirmPassword = function (msg, callback) {
    this.$prompt(msg, "Password", {
        confirmButtonText: 'Confirm',
        cancelButtonText: 'Cancel',
        inputType: 'password'
    }).then((value) => {
        callback(value)
    }).catch(() => { });
}
Vue.prototype.$post = function (url, data) {
    var action = new beetlexAction(url);
    return action.asyncpost(data);
}
Vue.prototype.$get = function (url, data) {
    var action = new beetlexAction(url);
    return action.asyncget(data);
}
Vue.prototype.$put = function (url, data) {
    var action = new beetlexAction(url);
    return action.asyncput(data);
}