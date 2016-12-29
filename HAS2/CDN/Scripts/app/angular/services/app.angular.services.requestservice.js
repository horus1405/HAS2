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