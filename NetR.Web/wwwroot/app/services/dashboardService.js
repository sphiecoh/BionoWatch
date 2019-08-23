'use strict'
app.factory('dashboardDataService', function ($http, toaster) {

    var serviceBase = '/api/Dashboard/';
    var service = {};

    var _refreshDashboard = function () {
        $http.get(serviceBase+'')
    }

    var _loadservice = function ()
    {
        $http.get(serviceBase + 'GetServices')
                   .success(function (data) {
                       return data;
                   });
                   
    };

    service.refreshDashboard = _refreshDashboard;
    service.loadservices = _loadservice;
    return service;

});

'use strict';

app.factory('signalRHubProxy', ['$rootScope', 'signalRServer',
    function ($rootScope, signalRServer) {
        function signalRHubProxyFactory(serverUrl, hubName, startOptions) {
            var connection = $.hubConnection(signalRServer);
            var proxy = connection.createHubProxy(hubName);
            connection.start(startOptions).done(function () { });

            return {
                on: function (eventName, callback) {
                    alert(eventName);
                    proxy.on(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                },
                off: function (eventName, callback) {
                    proxy.off(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                },
                invoke: function (methodName, callback) {
                    proxy.invoke(methodName)
                        .done(function (result) {
                            $rootScope.$apply(function () {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                },
                connection: connection
            };
        };

        return signalRHubProxyFactory;
    }]);

