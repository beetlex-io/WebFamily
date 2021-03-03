function autoField() {
    this.label = null;
    this.name = null;
    this.type = null;
    this.data = null;
    this.parent = null;
    this.dataurl = null;
    this.value = null;
    this.nulloption = false;
    this.readonly = false;
    this.title = '';
    this.hide = false;
    this.row = 3;
    this.eof = false;
    this.uploadurl = null;
    this.filesize = 500;
    this.rules = [];
    this.width = null;
    this.minWidth = '';
    this.cols = 1;
    this.height = null;
    this.saveurl = null;
    this.pwdtype = null;
    this.defaultimg = null;
    this.labelwidth = 120;
    this.icon = null;
    this.edit = false;
    this.placeholder = '';
}

autoField.prototype.validator = function (validator) {
    this.rules.push({ validator: validator, trigger: 'blur' });
}


autoField.prototype.require = function (message, type, trigger) {
    if (!trigger)
        trigger = 'blur';
    this.rules.push({ message: message, type: type, trigger: trigger, required: true });
    return this;
}
autoField.prototype.isEmail = function (message, required, trigger) {
    if (!trigger)
        trigger = 'blur';
    this.rules.push({ message: message, type: 'email', trigger: trigger, required: required });
    return this;
}
autoField.prototype.isNumber = function (message, required, trigger) {
    if (!trigger)
        trigger = 'blur';
    this.rules.push({ message: message, type: 'number', trigger: trigger, required: required });
    return this;
}

autoField.prototype.isNumberRange = function (message, max, min, trigger) {
    if (!trigger)
        trigger = 'blur';
    this.rules.push({ message: message, type: 'number', trigger: trigger, max: max, min: min });
    return this;
}

autoField.prototype.range = function (message, type, max, min, trigger) {
    if (!trigger)
        trigger = 'blur';
    this.rules.push({ message: message, type: type, trigger: trigger, max: max, min: min });
    return this;
}
autoField.prototype.IsHost = function (message, required) {
    var compareExpression = (rule, value, callback) => {
        if (required == true && !value) {
            callback(new Error(message));
            return;
        }
        if (!value) {
            callback();
            return;
        }
        if (!value.match(/http[s]{0,1}:\/\/((\w+(\.\w+)+)|(localhost))(\:[0-9]+){0,1}/i))
            callback(new Error(message));
        else
            callback();
    };
    this.rules.push({ validator: compareExpression, trigger: 'blur', required: required });
}
autoField.prototype.match = function (regex, message, required) {
    var compareExpression = (rule, value, callback) => {

        if (required && !value) {
            callback(new Error(message));
            return;
        }
        if (!value) {
            callback();
            return;
        }
        if (!value.match(regex))
            callback(new Error(message));
        else
            callback();
    };
    this.rules.push({ validator: compareExpression, trigger: 'blur', required: required });
}
autoField.prototype.compare = function (editor, field, message) {
    var compareExpression = (rule, value, callback) => {
        var password = editor.getField(this.name);
        var cpassword = editor.getField(field);
        if (password.value != cpassword.value)
            callback(new Error(message));
        else
            callback();
    };
    this.rules.push({ validator: compareExpression, trigger: 'blur' });
}

autoField.prototype.addValue = function (value, label) {
    if (!this.data) {
        this.data = [];
    }
    this.data.push({ label: label, value: value })
    return this;
};
function autoData() {
    this.data = null;
    this.items = [];
    this.labelwidth = 120;
}

autoData.prototype.setValue = function (val) {
    if (val) {
        for (i = 0; i < this.items.length; i++) {
            var p = this.items[i].name;
            if (val.hasOwnProperty(p))
                this.items[i].value = val[p];
        }
    }
};
autoData.prototype.addSpace = function (eof) {
    var item = new autoField();
    item.type = "space";
    item.eof = eof;

    this.items.push(item);
    return item;
}
autoData.prototype.addButton = function (name, label, eof) {
    var item = new autoField();
    item.type = "button";
    item.name = name;
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addIconButton = function (name, label, icon, eof) {
    var item = new autoField();
    item.type = "iconbutton";
    item.label = label,
        item.name = name;
    item.icon = icon;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addLabel = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "label";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addHtml = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "html";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addLink = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "link";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addDate = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "date";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addTime = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "time";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addNumber = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "number";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};

autoData.prototype.addText = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "text";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addRemark = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "remark";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addSwitch = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "switch";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addSelect = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "select";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addRadio = function (name, label, eof) {
    var item = new autoField();
    item.type = "radio";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addCheckBox = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "checkbox";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};
autoData.prototype.addPassword = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "password";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};

autoData.prototype.addSetPassword = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "setpassword";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
}

autoData.prototype.addViewPassword = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "viewpassword";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
}

autoData.prototype.addRate = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "rate";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};

autoData.prototype.addUpload = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "upload";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};

autoData.prototype.addImg = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "img";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};

autoData.prototype.addUploadImg = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.type = "uploadimg";
    item.label = label;
    item.eof = eof;
    this.items.push(item);
    return item;
};

autoData.prototype.addColor = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.eof = eof;
    item.type = "color";
    item.label = label;
    this.items.push(item);
    return item;
};

autoData.prototype.addInputSelect = function (name, label, eof) {
    var item = new autoField();
    item.name = name;
    item.eof = eof;
    item.type = "input-select";
    item.label = label;
    this.items.push(item);
    return item;
};

autoData.prototype.bindForm = function (form) {

    form.setInfo(this);

};

autoData.prototype.bindGrid = function (grid) {
    grid.setColumns(this.items);
};