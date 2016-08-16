/// <reference path="../angular.min.js" />
var app = angular.module('MyApp', ['angular-loading-bar', 'toaster', 'ngAnimate']);

app.config(['$httpProvider', function ($httpProvider, $routeProvider) {
    $httpProvider.interceptors.push('tokenInterceptorService');

    //$routeProvider.when("/home", {
    //    controller: "homeController",
    //    templateUrl: "/app/views/home.html"
    //});

    //$routeProvider.when("/login", {
    //    controller: "loginController",
    //    templateUrl: "/app/views/login.html"
    //});

    //$routeProvider.otherwise({ redirectTo: "/home" });

}]);
