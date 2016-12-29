
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