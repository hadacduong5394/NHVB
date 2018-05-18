(function (app) {
    app.controller('statisticImportsController', statisticImportsController);
    statisticImportsController.$inject = ['$scope', 'notificationService', 'apiService', '$filter'];
    function statisticImportsController($scope, notificationService, apiService, $filter) {
        $scope.years = [];
        $scope.currentYear = 0;
        function loadyears() {
            apiService.get('api/datetimehelper/getyears', null, function (result) {
                $scope.years = result.data;
                $scope.currentYear = $scope.years[0];
                $scope.loadmonths();
                $scope.getStatistic();
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }
        loadyears();

        $scope.months = [];
        $scope.currentMonth = (new Date()).getMonth() + 1;
        $scope.loadmonths = function() {
            var config = {
                params: {
                    year: $scope.currentYear
                }
            }
            apiService.get('api/datetimehelper/getmonthlimit', config, function (result) {
                $scope.months = result.data;
                var year = (new Date()).getFullYear();
                if (year == $scope.currentYear) {
                    $scope.currentMonth = (new Date()).getMonth() + 1;
                } else {
                    $scope.currentMonth = $scope.months[0];
                }
            }, function (reason) {
                notificationService.displayError(reason.statusText);
            });
        }

        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Tổng chi'];

        $scope.chartdata = [];
        $scope.getStatistic = function() {
            var config = {
                params: {
                    month: $scope.currentMonth,
                    year: $scope.currentYear
                }
            }
            apiService.get('api/statistic/gettotalimports', config, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var totals = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
                    totals.push(item.Total);
                });
                chartData.push(totals);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }
    }
})(angular.module('statistic.module'));