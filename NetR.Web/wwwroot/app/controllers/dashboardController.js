'use strict';
app.controller('dashboardController', function ($scope, $http, apiurl, logger) {
    
    $scope.services = {};
    var successLog = logger.getLogFn('configController', 'success');
    var errorLog = logger.getLogFn('config', 'error');

    $scope.loadservices = function ()
    {
        $http.get(apiurl)
                  .success(function (data) {
                      $scope.services = data;
                  }).error(function (error) { errorLog('Error occured',error.Message,true); });

     
    };

   
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.start().then(function () {
        console.log("connected");
    });
    connection.on("recieve", service => {
        $scope.services.push(service);
    });
    
});