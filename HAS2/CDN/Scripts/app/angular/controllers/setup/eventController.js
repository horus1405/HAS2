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