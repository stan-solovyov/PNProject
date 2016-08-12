/// <reference path="../angular.min.js" />
var app = angular.module('MyApp', []);

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('tokenInterceptorService');
}]);