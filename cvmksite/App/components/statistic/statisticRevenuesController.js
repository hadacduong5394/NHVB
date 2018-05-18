(function (app) {
    app.controller('statisticRevenuesController', statisticRevenuesController);
    statisticRevenuesController.$inject = ['$scope', 'notificationService', 'apiService', '$filter'];
    function statisticRevenuesController($scope, notificationService, apiService, $filter) {
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
        $scope.loadmonths = function () {
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
        $scope.series = ['Doanh số', 'Lợi nhuận'];

        $scope.chartdata = [];
        $scope.getStatistic = function () {
            var config = {
                params: {
                    month: $scope.currentMonth,
                    year: $scope.currentYear
                }
            }
            apiService.get('api/statistic/getrevanues', config, function (response) {
                debugger;
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date, 'dd/MM/yyyy'));
                    revenues.push(item.Revanues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }
        
    }
})(angular.module('statistic.module'));