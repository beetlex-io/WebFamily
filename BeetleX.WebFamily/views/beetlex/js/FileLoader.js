function UploadLoader(file, blocksize) {
    this.blockSize = 1024 * 32;
    if (blocksize)
        this.blockSize = blocksize;
    this.file = file;
    this.size = file.size;
    this.name = file.name;
    this.pages = Number.parseInt(this.size / this.blockSize);
    this.index = 0;
    if (this.size % this.blockSize > 0)
        this.pages++;
    this.onLoad = null;
    this.onUpload = null;
    this.uploadUrl = null;
    this.percent = "0";
    this.loadSize = 0;
    this.parameters = {};
}

UploadLoader.prototype.read = function (callback) {
    var readHandler = this.onLoad;
    if (callback)
        readHandler = callback;
    var reader = new FileReader();
    var _this = this;
    var start = this.index * this.blockSize;
    var end = start + this.blockSize;
    if (end >= this.size)
        end = this.size;
    this.loadSize += end - start;
    reader.onload = function () {
        var data = this.result;
        var first = _this.index == 0;
        var eof = _this.index == _this.pages - 1;
        var name = _this.name;
        _this.index++;
        _this.UpdatePercent();
        if (readHandler) {
            readHandler.call(_this, { data: data, first: first, eof: eof, name: name, start: start, end: end });
        }
    };
    var block = this.file.slice(start, end);
    reader.readAsArrayBuffer(block)
}
UploadLoader.prototype.UpdatePercent = function () {
    num = parseFloat(this.index);
    total = parseFloat(this.pages);
    if (isNaN(num) || isNaN(total)) {
        return;
    }
    this.percent = total <= 0 ? "0" : (Math.round(num / total * 10000) / 100.00 + '');
}
UploadLoader.prototype.uploadItem = function (e) {
    var url = this.uploadUrl + '?_id=1';
    Object.getOwnPropertyNames(this.parameters).forEach(v => {
        if (this.parameters[v])
            url += '&' + v + '=' + encodeURI(this.parameters[v]);
    });
    url += '&name=' + encodeURI(e.name) + "&eof=" + e.eof + "&first=" + e.first;
    var action = new beetlexAction(url);
    action.asyncput(e.data).then(r => {
        if (e.eof == false) {
            if (this.onUploadBlock)
                this.onUploadBlock(this);
            this.read(this.uploadItem);
        }
        else {
            if (this.onUpload)
                this.onUpload(this);
        }
    });
}

UploadLoader.prototype.upload = function (url, uploadCompleted, uploadBlock) {
    this.onUpload = uploadCompleted;
    this.onUploadBlock = uploadBlock;
    this.uploadUrl = url;
    this.read(this.uploadItem);
}