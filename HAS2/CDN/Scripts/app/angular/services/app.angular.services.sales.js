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