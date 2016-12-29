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