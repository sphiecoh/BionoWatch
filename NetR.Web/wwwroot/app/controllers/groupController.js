'use strict';
app.controller('groupController', function ($scope, logger,$http) {
   
    var successLog = logger.getLogFn('configController', 'success');
    var errorLog = logger.getLogFn('config', 'error');
    $scope.load = function () {
            $http.get('/api/notification')
                .success(function (data) {
                    $scope.groups = data;
                }).error(function (error) { errorLog('Error occured', error.Message, true); });

    };
    $scope.delete = function (id,index) {
       
        $http.delete('/api/notification/groups/' + id)
            .success(function (data) {
                successLog('Deleted group');
                $scope.groups.splice(index, 1);
            }).error(function (error) { errorLog('Error occured', error.Message, true); });
    }

    $scope.saveGroup = function (group) {
        $http.post('/api/notification/group/'+group.name)
            .success(function (data) {
                $scope.groups = data;
            }).error(function (error) { errorLog('Error occured', error.Message, true); });
    }
});