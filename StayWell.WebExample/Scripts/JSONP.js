function JsonpCall(service, callback) {
    var separator = '?';
    if (service.indexOf('?') > 0) {
        separator = '&';
    }
    service += separator + 'callback=' + callback;
    service += '&applicationId=' + appId;
    service += '&random=' + Math.random();
    var tag = "<script type='text/javascript' src='" + service + "'></script>";
    $('body').append(tag);
}