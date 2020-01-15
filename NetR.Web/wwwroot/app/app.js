var app = angular.module('BionoWatch', ['ngRoute', 'chieffancypants.loadingBar','cgNotify']);
app.value('signalRServer', '');
app.value('apiurl', 'api/dashboard/');
toastr.options.timeOut = 4000;
toastr.options.positionClass = 'toast-top-right';
app.config(function ($routeProvider) {

    $routeProvider.when("/dashboard", {
        controller: "dashboardController",
        templateUrl: "app/views/dashboard.html"
    });

    $routeProvider.when("/config", {
        controller: "configController",
        templateUrl: "app/views/config.html"
    });

    $routeProvider.when("/group", {
        controller: "groupController",
        templateUrl: "app/views/group.html"
    });

    $routeProvider.otherwise({ redirectTo: "/dashboard" });

});