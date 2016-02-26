var PluginSDK = (function () {

    var _self = {
        Initialize: Initialize,
    }

    function Initialize(applicationId) {
        if (applicationId == null || applicationId == undefined) {
            console.log("ApplicationId is a required parameter.");
            return;
        }

        //Try to load the plugins
        var divElement = $(".staywell-centers");
        if (divElement != null && divElement.length>0) {
            CenterPlugin.Initialize(divElement, applicationId);
        }

        var divElement = $(".staywell-atoz");
        if (divElement != null && divElement.length > 0) {
            AtoZPlugin.Initialize(divElement, applicationId);
        }
    }

    return _self;

}());