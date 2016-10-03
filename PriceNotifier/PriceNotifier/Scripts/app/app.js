/// <reference path="../angular.min.js" />
var app = angular.module('MyApp', ['angular-loading-bar', 'toaster', 'ngAnimate', 'ngRoute', 'ui.grid', 'ui.grid.pagination', 'ui.grid.edit', 'ui.grid.cellNav', 'ui.grid.validate', 'ui.bootstrap','chart.js']);

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

    $routeProvider.when("/users", {
        controller: "UserCtrl",
        templateUrl: "/scripts/app/Views/users.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);

app.directive('animateOnChange', function ($animate, $timeout) {
    return function (scope, elem, attr) {
        scope.$watch(attr.animateOnChange, function (nv, ov) {
            if (nv !== ov) {
                var c = nv > ov ? 'change-up' : 'change';
                $animate.addClass(elem, c).then(function () {
                    $timeout(function () { $animate.removeClass(elem, c) }, 4000);
                });
            }
        });
    };
});
