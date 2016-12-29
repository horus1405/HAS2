/// <reference path="../app._references.js" />

(function (w, $, tl, swal) {
    'use strict';

    var $f = w.hasapp || {};

    //utilities
    $f.utils = (function () {        

        function parseBool(bool) {
            return bool.toString().toLowerCase() === 'true';
        }

        function sameTopPosition(elem, follower) {

            var f = jQuerize(elem);
            var s = jQuerize(follower);
            if (is$Null(f) || is$Null(s)) return;


            var diff = f.offset().top - s.offset().top;
            s.offset({ top: f.offset().top });
            var b = s.css('padding-bottom').replace('px', '');
            s.css('padding-bottom', b + diff + 'px');

        }

        function jQuerize(obj) {
            if (!obj) return jQuery();
            var result = obj;
            if ($.isPlainObject(obj))
                result = $(obj);
            return result;
        }

        function is$Null(obj) {
            var o = jQuerize(obj);
            return !!!o[0];
        }

        function isAuthenticated() {
            return getBoolHidden('#isAuthenticated');
        }

        function getLoginPath() {
            var res = $('#loginPath').val();
            return (function () {
                return res;
            })();
        }

        function getBoolHidden(id) {
            var b = $(id).val();
            var res = parseBool(b);
            return (function () {
                return res;
            })();
        }

        function showMessage(msg, title) {
            showMessage(msg, title, null);
        }

        function showMessage(msg, title, ourCallback) {
            if ($('#Modal h3').length == 0) $('#Modal').prepend('<h3></h3>');
            $('#Modal h3').html(title);
            $('#Modal p').html(msg);
            $('#Modal').addClass('width500');
            $('#Backdrop, #Modal').addClass('visible');

            if (ourCallback != null) {
                $('.close-modal, #Backdrop').on('click', ourCallback);
            }
        }

        function showWarning(msg, title, buttonText, buttoncallback) {
            showAlert('warning', msg, title, (buttonText ? buttonText : tl.Close), buttoncallback);
        }

        function showWarningWithCancelButton(msg, title, buttonText, buttoncallback) {
            showAlertWithCancelButton('warning', msg, title, (buttonText ? buttonText : tl.Close), buttoncallback);
        }

        function showSuccess(msg, title, buttonText, buttoncallback) {
            showAlert('success', msg, title, (buttonText ? buttonText : tl.Close), buttoncallback);
        }

        function showError(msg, buttoncallback, buttonText) {
            showAlert('error', (msg || tl.GenericError), tl.Error, buttonText || tl.Close, buttoncallback);
        }

        function showAlert(type, msg, title, buttonText, buttoncallback) {
            w.sweetAlertInitialize();
            swal({
                title: htmlUnescape(title),
                text: htmlUnescape(msg),
                type: type,
                confirmButtonText: buttonText
            }, buttoncallback);
        }

        function showAlertWithCancelButton(type, msg, title, buttonText, buttoncallback) {
            w.sweetAlertInitialize();
            swal({
                title: htmlUnescape(title),
                text: htmlUnescape(msg),
                type: type,
                confirmButtonText: htmlUnescape(buttonText),
                showCancelButton: true,
                confirmButtonColor: '#DD6B55',
                closeOnConfirm: true
            }, buttoncallback);
        }

        function htmlUnescape(value) {
            return String(value)
                .replace(/&quot;/g, '"')
                .replace(/&#39;/g, "'")
                .replace(/&lt;/g, '<')
                .replace(/&gt;/g, '>')
                .replace(/&amp;/g, '&');
        }

        function extractErrorMessage(data, errorCodes) {
            var errmsg = data && errorCodes  && data.errorcode ? errorCodes[data.errorcode.toString()] : undefined;
            if (typeof errmsg === 'undefined')
                errmsg = data && data.errormessage ? data.errormessage : tl.UnknownError;
            else
                errmsg += data.data ? ' ' + data.data : '';
            return errmsg;
        }

        function showDataError(data, callback, buttonText, errorCodes) {
            var errorMessage = extractErrorMessage(data, errorCodes);
            showError(errorMessage, callback, buttonText);
        }

        function isInvalidBrowser() {
            var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
            if (isChrome) return true;
            var isSafari = /Safari/.test(navigator.userAgent) && /Apple Computer/.test(navigator.vendor);
            if (isSafari) return true;
            var isIE = /MSIE/.test(navigator.userAgent);
            if (isIE) return true;
            return false;
        }

        function fromIsoDateToDate(strDate) {
            if (!strDate) {
                return new Date(null);
            }

            if (isInvalidBrowser()) {
                /*ignore jslint start*/
                strDate = strDate.replace(/\+.+/, ''); //removing possible +2:00
                strDate = strDate.replace(/\..+/, ''); //removing .400Z
                /*ignore jslint end*/
                var parts = strDate.split('T');
                if (parts && parts.length === 2 && strDate !== '0001-01-01T00:00:00') {
                    var date = parts[0].split('-');
                    var time = parts[1].split(':');

                    if (date && date.length === 3 && time && time.length === 3) {
                        var struct = { year: date[0], month: date[1] - 1, day: date[2], hours: time[0], minutes: time[1], seconds: time[2] };
                        return new Date(struct.year, struct.month, struct.day, struct.hours, struct.minutes, struct.seconds, 0);
                    }
                }
            } else {
                return new Date(strDate);
            }
            return null;
        }

        function getUtcDate() {
            var now = new Date();
            var utc = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds());
            return utc;
        }

        function formatString() {
            var s = arguments[0];
            for (var i = 0; i < arguments.length - 1; i++) {
                var reg = new RegExp("\\{" + i + "\\}", "gm");
                s = s.replace(reg, arguments[i + 1]);
            }
            return s;
        }

        return {            
            parseBool: parseBool,
            sameTopPosition: sameTopPosition,
            jQuerize: jQuerize,
            is$Null: is$Null,
            isAuthenticated: isAuthenticated,
            getLoginPath: getLoginPath,
            getBoolHidden: getBoolHidden,
            showMessage: showMessage,
            showWarning: showWarning,
            showWarningWithCancelButton : showWarningWithCancelButton,
            showError: showError,
            showSuccess: showSuccess,
            extractErrorMessage: extractErrorMessage,
            showDataError: showDataError,
            fromIsoDateToDate: fromIsoDateToDate,
            getUtcDate: getUtcDate,
            formatString: formatString
        };

    })();

})(window, jQuery, window.Translations, window.swal);