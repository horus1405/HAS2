///#source 1 1 /CDN/Scripts/app/app.init.js
/**********************************************
* This will be the main namespace of the app
***********************************************/
window.hasapp = {};
window.Translations = window.Translations || {};
///#source 1 1 /CDN/Scripts/app/app.load.js
/// <reference path="app._references.js" />
/*globals Translations*/

(function (w, $, tl) {
    'use strict';

    //on load
    $(function () {

        var $f = w.fwwc;        

    });
})(window, jQuery, Translations);
///#source 1 1 /CDN/Scripts/app/angular/app.angular.start.js
/// <reference path="../app._references.js" />

(function (angular, Data) {
    'use strict';
    //starting our main module
    var mod = angular.module('app',
                                [
                                    'ngSanitize',
                                    'ngCookies',
                                    'ui.router',
                                    'ngAnimate'
                                ]
                            );

    //fixing mvc IsAjaxRequest flag
    mod.config([
        '$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
            $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
            $httpProvider.defaults.withCredentials = true;

            // For any unmatched url, redirect to /state1
            $urlRouterProvider.otherwise("dashboard");
            //
            // Now set up the states
            $stateProvider
               .state('dashboard', {
                   url: "/dashboard",
                   templateUrl: "dashboard"
               })
              .state('setup', {
                  url: "/setup",
                  templateUrl: "setup"
              })
                  .state('setup.event', {
                      url: "/event",
                      templateUrl: "event"
                  })
                  .state('setup.basiccode', {
                      url: "/basiccode",
                      templateUrl: "basiccode"
                  })
                  .state('setup.currency', {
                      url: "/currency",
                      templateUrl: "currency"
                  })
              .state('sales', {
                  url: "/sales",
                  templateUrl: "sales"
              })
                  .state('sales.account', {
                      url: "/account",
                      templateUrl: "account"
                  })
                  .state('sales.contact', {
                      url: "/contact",
                      templateUrl: "contact"
                  })
                  .state('sales.proposal', {
                      url: "/proposal",
                      templateUrl: "proposal"
                  })
              .state('inventory', {
                  url: "/inventory",
                  templateUrl: "inventory"
              });
        }
    ]);

    /**
     * AngularJS default filter with the following expression:
     * "person in people | filter: {name: $select.search, age: $select.search}"
     * performs a AND between 'name: $select.search' and 'age: $select.search'.
     * We want to perform a OR.
     */
    mod.filter('propsFilter', function () {
        return function (items, props) {
            var out = [];

            if (angular.isArray(items)) {
                items.forEach(function (item) {
                    var itemMatches = false;

                    var keys = Object.keys(props);
                    for (var i = 0; i < keys.length; i++) {
                        var prop = keys[i];
                        var text = props[prop].toLowerCase();
                        if (item[prop].toString().toLowerCase().indexOf(text) !== -1) {
                            itemMatches = true;
                            break;
                        }
                    }

                    if (itemMatches) {
                        out.push(item);
                    }
                });
            } else {
                // Let the output be the input untouched
                out = items;
            }

            return out;
        };
    });

    angular.
        module("sprintf", []).
        filter("sprintf", function () {
            return function () {
                return sprintf.apply(null, arguments)
            }
        }).
        filter("fmt", ["$filter", function ($filter) {
            return $filter("sprintf")
        }]).
        filter("vsprintf", function () {
            return function (format, argv) {
                return vsprintf(format, argv)
            }
        }).
        filter("vfmt", ["$filter", function ($filter) {
            return $filter("vsprintf")
        }]);



    mod.filter('numberFormat', function () {
        return function (input) {
            return Globalize.format(input, Data.numberFormat);
        };
    });
})(window.angular, Data);
///#source 1 1 /CDN/Scripts/app/app.utils.js
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
///#source 1 1 /CDN/Scripts/app/angular/services/app.angular.services.requestservice.js
(function (appData) {
    'use strict';

    angular
    .module('app')
    .factory('requestService', requestService);

    requestService.$inject = ['$http', '$cookieStore', '$rootScope', '$timeout'];

    function requestService($http, $cookieStore, $rootScope, $timeout) {

        var service = {};

        var eventCode = appData.eventCode;

        var currentRequests = 0;

        //Methods       
        service.post = post;
        service.get = get;

        service.currentrequests = function () { return currentRequests };

        return service;

        /******************************************************************/
        /* Public Functions*/
        /******************************************************************/

        function post(address, postdata, callback, errorcallback) {
            var baseAddress = Data.baseAddress;
            var languageCode = (Data && Data.languageCode) ? '/' + Data.languageCode : '';
            currentRequests++;
            $http.post(baseAddress + address, postdata).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      currentRequests--;
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      currentRequests--;
			      errorcallback(response);
			  });
        };

        function get(address, data, callback, errorcallback) {
            var baseAddress = appData.baseAddress;
            var languageCode = (appData && appData.languageCode) ? '/' + appData.languageCode : '';
            currentRequests++;
            $http.get(baseAddress + languageCode + address, { params: data }, null).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      currentRequests--;
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      currentRequests--;
			      errorcallback(response);
			  });
        };


        /******************************************************************/



    }

})(Data);
///#source 1 1 /CDN/Scripts/app/angular/services/app.angular.services.listscreenmanager.js

function ListScreenManager() {
    
    var me = this; //set as global to have access in closures

    this.isprocessing = false;

    this.listview = false;
    this.editview = false;
    this.serverresponse = null;

    this.searchparams = null;        

    this.list = [];
    this.edititem = null;
    this.edititemcopy = null;    

    //function callbacks
    this.getlistcallback = null;
    this.getitemcallback = null;
    this.saveitemcallback = null;

    this.createsaveobject = function () {
        var o = angular.copy(this.list);
        //clear object
        for (var i = o.Values.length - 1; i >= 0; i--) {
            if (o.Values[i].HasChanges == 0) {
                //remove this item
                o.Values.splice(i, 1);
            }
        }
        return o;
    }

    this.loadlist = function () {
        if (this.getlistcallback) {
            this.getlistcallback(
                    function (response) {
                        this.isprocessing = false;
                        if (response.data.result) {                            
                            me.list = response.data.data;
                            console.trace(response.data.data);
                        }
                        else { this.serverresponse = response.data.message; }
                    },
                    function () { this.isprocessing = false; }, 
                    this.searchparams
                );
        }
    };

    this.showdetail = function (o) {

        if (this.getitemcallback) {
            //this.edititem = this.getitemcallback(o);
            //this.edititemcopy = angular.copy(this.edititem);

            this.getitemcallback(o, 
                function (response) {
                    me.listview = false;
                    me.editview = true;
                    me.edititem = response.data.data;
                    me.edititemcopy = angular.copy(me.edititem);
                },
                function (response) { }
            )
        }

        

    };

    this.canceledit = function () {
        this.edititem = null;
        this.listview = true;
        this.editview = false;
    };

    this.apply = function () {
        angular.copy(this.edititemcopy, this.edititem);
        this.edititemcopy = null;

        if (this.edititem.HasChanges != 1)
            this.edititem.HasChanges = 2;
        this.edititem = null;
        this.listview = true;
        this.editview = false;
    };

    this.saveitem = function () {
        this.isprocessing = true;
        var list = createsaveobject();
        this.saveitemcallback(
            list,
            function (response) {
                this.isprocessing = false;
                if (response.data.result) {
                    this.detailitem = response.data.data;
                }
                else { this.serverresponse = response.data.message; }
            },
            null
        );
    };

    this.Configure = function (config) {        
        if (config.getlistcallback) { this.getlistcallback = config.getlistcallback; }
        if (config.getitemcallback) { this.getitemcallback = config.getitemcallback; }
        if (config.saveitemcallback) { this.saveitemcallback = config.saveitemcallback; }
        if (config.listview) { this.listview = config.listview; }
        if (config.editView) { this.editView = config.editView; }
    };

};

(function () {
    'use strict';

    angular.module('app').service('listScreenManagerService', [ListScreenManager]);        

})();
///#source 1 1 /CDN/Scripts/app/angular/services/app.angular.services.authenticationService.js
(function () {
    'use strict';

    angular
    .module('app')
    .factory('AuthenticationService', AuthenticationService);

    AuthenticationService.$inject = ['$http', '$cookieStore', '$rootScope', '$timeout'];
    function AuthenticationService($http, $cookieStore, $rootScope, $timeout, UserService) {
        var service = {};

        service.Login = Login;
        service.SetCredentials = SetCredentials;
        service.ClearCredentials = ClearCredentials;

        return service;

        function Login(username, password, callback) {

            /* Dummy authentication for testing, uses $timeout to simulate api call
             ----------------------------------------------*/
            $timeout(function () {
                var response;
                response = { success: true };
                callback(response);

            }, 1000);

            /* Use this for real authentication
             ----------------------------------------------*/
            //$http.post('/api/authenticate', { username: username, password: password })
            //    .success(function (response) {
            //        callback(response);
            //    });

        }

        function SetCredentials(username, password) {
            var authdata = Base64.encode(username + ':' + password);

            $rootScope.globals = {
                currentUser: {
                    username: username,
                    authdata: authdata
                }
            };

            $http.defaults.headers.common['Authorization'] = 'Basic ' + authdata; // jshint ignore:line
            $cookieStore.put('globals', $rootScope.globals);
        }

        function ClearCredentials() {
            $rootScope.globals = {};
            $cookieStore.remove('globals');
            $http.defaults.headers.common.Authorization = 'Basic ';
        }
    }



})();
///#source 1 1 /CDN/Scripts/app/angular/services/app.angular.services.setup.js
(function (appData) {
	'use strict';

	angular
    .module('app')
    .factory('setupService', setupService);

	setupService.$inject = ['$http', '$cookieStore', 'requestService'];

	function setupService($http, $cookieStore, requestService) {
		var service = {};

		service.basicCodesSelect = basicCodesSelect;
		service.basicCodeGet = basicCodeGet;
		service.eventGet = eventGet;
		service.eventSave = eventSave;
		service.basicCodeSave = basicCodeSave;
		service.currencySelect = currencySelect;
		service.currencyGet = currencyGet;

		return service;

		function eventGet(callback, errorcallback) {
		    var baseAddress = Data.baseAddress;
		    $http.post(baseAddress + '/setup/event/get').
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      errorcallback(response);
			  });
		};

		function eventSave(obj, callback, errorcallback) {
		    var baseAddress = Data.baseAddress;
		    $http.post(baseAddress + '/setup/event/save', { event: obj }).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      if(callback) callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      if (errorcallback) errorcallback(response);
			  });
		};

		function basicCodesSelect(callback, errorcallback) {

		    var baseAddress = Data.baseAddress;

		    $http.get(baseAddress + '/setup/basiccode/list?a=1').
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      errorcallback(response);
			  });
		};

		function basicCodeGet(tableid, callback, errorcallback) {

		    var baseAddress = Data.baseAddress;

		    $http.post(baseAddress + '/setup/basiccode/get', { id: tableid }).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      errorcallback(response);
			  });
		};

		function basicCodeSave(obj, callback, errorcallback) {
		    var baseAddress = Data.baseAddress;
		    $http.post(baseAddress + '/setup/basiccode/save', { basiccodetype: obj }).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      errorcallback(response);
			  });
		};

		function currencySelect(callback, errorcallback) {
		    var baseAddress = Data.baseAddress;
		    $http.post(baseAddress + '/setup/currency/list').
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      if (callback) callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      if (errorcallback) errorcallback(response);
			  });
		};

		function currencyGet(currencyid, callback, errorcallback) {
		    var baseAddress = appData.baseAddress;
		    $http.post(baseAddress + '/setup/currency/get', { id: currencyid }).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      errorcallback(response);
			  });
		};

	}

})(Data);
///#source 1 1 /CDN/Scripts/app/angular/services/app.angular.services.sales.js
(function () {
	'use strict';

	angular
    .module('app')
    .factory('salesService', salesService);

	salesService.$inject = ['$http', '$cookieStore', '$rootScope', '$timeout'];

	function salesService($http, $cookieStore, $rootScope, $timeout) {
		var service = {};

		service.accountSelect = accountSelect;
		service.accountGet = accountGet;

		return service;

		function accountSelect(callback, errorcallback) {

			var baseAddress = Data.baseAddress;

			$http.post(baseAddress + '/sales/account/list').
			  then(function (response) {
			  	// this callback will be called asynchronously
			  	// when the response is available
			  	callback(response);
			  }, function (response) {
			  	// called asynchronously if an error occurs
			  	// or server returns response with an error status.
			  	errorcallback(response);
			  });
		}

		function accountGet(accountid, callback, errorcallback) {

			var baseAddress = Data.baseAddress;

			$http.post(baseAddress + '/sales/account/get', { id: accountid }).
			  then(function (response) {
			  	// this callback will be called asynchronously
			  	// when the response is available
			  	callback(response);
			  }, function (response) {
			  	// called asynchronously if an error occurs
			  	// or server returns response with an error status.
			  	errorcallback(response);
			  });
		}

		function accountSave(obj, callback, errorcallback) {
		    var baseAddress = Data.baseAddress;
		    $http.post(baseAddress + '/sales/account/save', { account: obj }).
			  then(function (response) {
			      // this callback will be called asynchronously
			      // when the response is available
			      callback(response);
			  }, function (response) {
			      // called asynchronously if an error occurs
			      // or server returns response with an error status.
			      errorcallback(response);
			  });
		}
	}

})();
///#source 1 1 /CDN/Scripts/app/angular/controllers/login.js

(function (angular, trans, hasapp) {

    var loginmodule = angular.module('app')
        .controller('loginController', ['$scope', '$http', 'AuthenticationService', function ($scope, $http, AuthenticationService) {

            var baseAddress = Data.baseAddress;

            $scope.userdata = { login: null, password: null };
            $scope.serverresponse = null;

            $scope.isprocessing = false;

            $scope.LoginUser = function () {
                $scope.isprocessing = true;
                AuthenticationService.Login(
                        $scope.userdata.login,
                        $scope.userdata.password,
                        function (response) {
                            $scope.isprocessing = false;
                            if (response.success) { document.location.href = Data.baseAddress;}
                            else { $scope.serverresponse = response.message; }
                        }
                );
            }

        }]);

}(window.angular, Translations, window.hasapp));
///#source 1 1 /CDN/Scripts/app/angular/controllers/setup/basiccodelistController.js

(function (angular, trans, hasapp) {

    var basiccodelist = angular.module('app')
        .controller('basiccodelistController', ['$scope', '$http', 'setupService', '$animate', function ($scope, $http, setupService, $animate) {

            var baseAddress = Data.baseAddress;

            $scope.codeTypes = [];
            $scope.edititem = null;
            $scope.edititemcopy = null;
            //viewmode
            $scope.listview = true;
            $scope.detailview = false;
            $scope.detailitem = {};
            $scope.detailitemcopy = {};

            $scope.searchfilters = {};
            $scope.searchfilterslist = {};            

            /***************************** PRIVATE FUNCTIONS**********************************/
            var createsaveobject = function () {
                var o = angular.copy($scope.detailitem);
                //clear object
                for (var i = o.Values.length - 1; i >= 0; i--) {
                    if (o.Values[i].HasChanges == 0) {
                        //remove this item
                        o.Values.splice(i, 1);
                    }
                }
                o.HasChanges = 2;
                return o;
            }
            /***************************** PRIVATE FUNCTIONS**********************************/

            /*Gets the list of basic codes types with no values*/
            $scope.LoadBasicCodes = function () {
                $scope.isprocessing = true;
                setupService.basicCodesSelect(
                    function (response) {
                        $scope.isprocessing = false;
                        if (response.data.result) { $scope.codeTypes = response.data.data; console.trace(response.data.data); }
                        else { $scope.serverresponse = response.data.message; }
                    }, null
                );
            };

            /*Gets a single basic code type with values and adds it to the list*/
            $scope.GetBasicCode = function (basiccodeType) {
                $scope.isprocessing = true;
                if (!basiccodeType.Values || basiccodeType.Values.length == 0) {
                    setupService.basicCodeGet(
                        basiccodeType.ID,
                        function (response) {
                            $scope.isprocessing = false;
                            if (response.data.result) {
                                basiccodeType.Values = response.data.data;
                                $scope.detailitem = basiccodeType.Values;
                            }
                            else { $scope.serverresponse = response.data.message; }
                        },
                        null
                    );
                }
                else {
                    $scope.detailitem = basiccodeType.Values;
                    $scope.isprocessing = false;
                }
            };

            /* Adds a new item to the values list of basic code type*/
            $scope.addvalue = function () {
                var newitem = {
                    ID: 0,
                    HasChanges: 1,
                    ValueCode: '',
                    ValueName: '',
                    Value1: null,
                    Value2: null,
                    Value3: null,
                    Value4: null,
                    TableId: $scope.detailitem.TableId
                };
                $scope.detailitem.Values.push(newitem);
                $scope.edititem = newitem;
                $scope.edititemcopy = angular.copy(newitem);

            };

            $scope.edit = function (o) {
                $scope.edititem = o;
                $scope.edititemcopy = angular.copy(o);
            };

            $scope.delete = function (o, index) {
                if (o.HasChanges == 1) {
                    $scope.detailitem.Values.splice(index, 1);
                }
                else {
                    o.HasChanges = 3;
                }                
            };

            $scope.apply = function () {
                angular.copy($scope.edititemcopy, $scope.edititem);
                $scope.edititemcopy = null;

                if ($scope.edititem.HasChanges != 1)
                        $scope.edititem.HasChanges = 2;
                $scope.edititem = null;
            };            

            $scope.saveitem = function () {
                $scope.isprocessing = true;
                var item = createsaveobject();
                setupService.basicCodeSave(
                    item,
                    function (response) {
                        $scope.isprocessing = false;
                        if (response.data.result) {
                            $scope.detailitem = response.data.data;
                        }
                        else { $scope.serverresponse = response.data.message; }
                    },
                    null
                );
            };

            $scope.canceledit = function () {
                $scope.edititem = null;
                $scope.edititemcopy = null;
            };
            
            $scope.showdetail = function (basiccodeType) {
                $scope.listview = false;
                $scope.detailview = true;
                $scope.GetBasicCode(basiccodeType);
                console.trace($scope.detailitem);
            };

            $scope.showlist = function () {

                $scope.listview = true;
                $scope.detailview = false;
            };

            $scope.LoadBasicCodes();

            console.trace('$scope.codeTypes:'+$scope.codeTypes.length);

        }]);

}(window.angular, Translations, window.hasapp));
///#source 1 1 /CDN/Scripts/app/angular/controllers/setup/eventController.js
(function (angular, trans, hasapp) {

    var basiccodelist = angular.module('app')
        .controller('eventController', ['$scope', '$http', 'setupService', function ($scope, $http, setupService) {

            var baseAddress = Data.baseAddress;

            //Controller Properties
            $scope.item = {};

            $scope.getitem = function () {
                $scope.isprocessing = true;
                setupService.eventGet(
                    function (response) {
                        $scope.isprocessing = false;
                        if (response.data.result) {
                            $scope.item = response.data.data;
                            $scope.item.EndDate = new Date($scope.item.EndDate);
                            $scope.item.StartDate = new Date($scope.item.StartDate);
                        }
                        else { $scope.serverresponse = response.data.message; }
                    },
                    null
                );
            };

            $scope.saveitem = function () {
                $scope.isprocessing = true;
                $scope.item.HasChanges = 2;
                setupService.eventSave(
                    $scope.item,
                    function (response) {
                        $scope.isprocessing = false;
                        if (response.data.result) {
                            $scope.item = response.data.data;
                            $scope.item.EndDate = new Date($scope.item.EndDate);
                            $scope.item.StartDate = new Date($scope.item.StartDate);
                        }
                        else { $scope.serverresponse = response.data.message; }
                    },
                    null
                );
            };

            $scope.getitem();
            console.trace('$scope.item:' + $scope.item);            

        }]);

}(window.angular, Translations, window.hasapp));
///#source 1 1 /CDN/Scripts/app/angular/controllers/setup/currencyController.js
(function (angular, trans, hasapp) {

    var basiccodelist = angular.module('app')
        .controller('currencyController', ['$scope', '$http', 'setupService', 'listScreenManagerService', function ($scope, $http, setupService, listScreenManagerService) {
                      

            $scope.screenmanager = listScreenManagerService;

            $scope.screenmanager.Configure(
                {
                    listview:true, 
                    getlistcallback: setupService.currencySelect,
                    getitemcallback: $scope.getitem,
                    saveitemcallback: setupService.currenciesSave
                }
            );

            $scope.screenmanager.loadlist();

            $scope.getitem = function (o) {
                return o;
            };

        }]);

}(window.angular, Translations, window.hasapp));
///#source 1 1 /CDN/Scripts/app/angular/controllers/sales/accountlistController.js

(function (angular, trans, hasapp) {

    var accountlist = angular.module('app')
        .controller('accountlistController', ['$scope', '$http', 'salesService', 'listScreenManagerService', function ($scope, $http, salesService, listScreenManagerService) {

            var baseAddress = Data.baseAddress;

            $scope.screenmanager = listScreenManagerService;

            $scope.screenmanager.Configure(
                {
                    listview: true,
                    getlistcallback: salesService.accountSelect,
                    getitemcallback: salesService.accountGet,
                    saveitemcallback: salesService.accountSave
                }
            );

            $scope.screenmanager.loadlist();            

        }]);

}(window.angular, Translations, window.hasapp));
