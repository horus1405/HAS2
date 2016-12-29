
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