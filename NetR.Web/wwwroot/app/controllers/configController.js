'use strict';
app.controller('configController', function ($scope, $http, logger, apiurl ) {
    $scope.pollingInterval = {Key: 'PollingInterval' };
    $scope.email = {};
    $scope.config = { Enabled : true};
    $scope.services = {};
    var getLogFn = logger.getLogFn('configController','success');
    var errorlog = logger.getLogFn('config','error');
    

    $scope.loadConfiguration = function () {
       
        Get();
        
    };

    function Get()
    {
        $http.get('api/configuration')
           .success(function (data) {
               $scope.pollingInterval = data.interval;
               $scope.emailRecipients = data.emailReciptients;
               $scope.services = data.services;
           })
           .error(function (error) {
               errorlog('error occured', error.Message, true);
           });
    }
   
    $scope.saveInterval = function () {
       
        var data = $scope.pollingInterval;
        $http({
            method: 'POST',
            url: 'api/configuration/interval?pollingInterval=' + data.Value,
            data: data
        }).success(function (data, status, headers, config) {
          
            getLogFn('Saved successfully', null, true);
        }).error(function (data, status, headers, config) {                      
           
            errorlog('error occured', data.Message, true);
        });
    
    };

    $scope.saveEmail = function (email) {
        $http.post('api/configuration/notification?email=' + email.Email).success(function (data) {
            getLogFn('Saved successfully', null, true);
            $scope.emailRecipients.push(data);
            $scope.email = {};
        }).error(function (error) { errorlog('error occured', error.Message, true); });
    };


    $scope.deleteEmail = function (eId , index) {
        $http.post(apiurl + 'RemoveEmail/' + eId).success(function () {
            getLogFn('Saved successfully', null, true);
            $scope.emailRecipients.splice(index , 1);
        }).error(function (error) { errorlog('error occured', error.Message, true); });
    };


    $scope.saveConfig = function ()
    {
        if ($scope.frmConfig.$invalid)
            return;
        var data = $scope.config;
        data.Status = 'Unchecked';
        $http.post('api/configuration', data).success(function (service) {
            getLogFn('Saved successfully', null, true);
            $scope.services.push(service);
        }).error(function (error) { errorlog('error occured', error.Message, true); });
       
        $scope.config = {};
    };

    $scope.enabledChanged = function (config)
    {
        console.log(config.Enabled);
        $http({
            method: 'POST',
            url: apiurl+'AddServiceConfiguration',
            data: config
        }).success(function (data, status, headers, config) {
            getLogFn('Saved successfully', null, true);
        }).error(function (data, status, headers, config) {
            errorlog('error occured', data.Message, true);
        });
    }
    $scope.deleteService = function (id, index) {
        $http.post(apiurl + 'RemoveService/' + id).success(function () {
            getLogFn('Saved successfully', null, true);
            $scope.services.splice(index, 1);
        }).error(function (error) { errorlog('error occured', error.Message, true); });

    };
   

});