(function (app) {
    app.filter('statusFilter', function () {
        return function (status) {
            if (status == true) {
                return "Kích hoạt";
            } else {
                return "Tạm khóa";
            }
        }
    });
})(angular.module('cvmk.common.module'));