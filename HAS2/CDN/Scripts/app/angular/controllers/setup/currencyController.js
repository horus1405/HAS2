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