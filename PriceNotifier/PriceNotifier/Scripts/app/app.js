/// <reference path="../angular.min.js" />
var app = angular.module('MyApp', ['angular-loading-bar', 'toaster', 'ngAnimate','ngRoute']);

app.config(['$httpProvider', '$routeProvider', '$locationProvider', function ($httpProvider, $routeProvider, $locationProvider) {
    $httpProvider.interceptors.push('tokenInterceptorService');

    $routeProvider.when("/", {
        controller: "MainCtrl",
        templateUrl: "/scripts/app/Views/main.html"
    });

    $routeProvider.when("/myitems", {
        controller: "ProductCtrl",
        templateUrl: "/scripts/app/Views/items.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);
