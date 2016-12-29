
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