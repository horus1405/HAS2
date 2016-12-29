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