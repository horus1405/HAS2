
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