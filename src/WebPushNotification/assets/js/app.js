window.APP = (function (module, $) {
    "use strict";

    var m = module;

    $(function () {
        m.pushModule && m.pushModule.init();
    }); 

    return module;
})(window.APP || {}, window.jQuery);