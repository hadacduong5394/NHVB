(function () {
    angular.module('room.module', ['cvmk.common.module']).config(config);
    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('room', {
                parent: 'base',
                url: "/room",
                templateUrl: "/App/components/room/room.html",
                controller: "roomController"
            }).state('createroom', {
                parent: 'base',
                url: "/room/create",
                templateUrl: "/App/components/room/createRoom.html",
                controller: "createRoomController"
            }).state('editroom', {
                parent: 'base',
                url: "/room/edit/:id",
                templateUrl: "/App/components/room/editRoom.html",
                controller: "editRoomController"
            });
    }
})();