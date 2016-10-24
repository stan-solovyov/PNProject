/// <reference path="../angular.min.js" />
var app = angular.module('MyApp', ['angular-loading-bar', 'toaster', 'ngAnimate', 'ui.router', 'ui.grid', 'ui.grid.pagination', 'ui.grid.edit', 'ui.grid.cellNav', 'ui.grid.validate', 'ui.bootstrap', 'chart.js', 'textAngular']);

app.config(['$stateProvider', '$urlRouterProvider', '$httpProvider', '$locationProvider', function ($stateProvider, $urlRouterProvider, $httpProvider, $locationProvider) {
    $httpProvider.interceptors.push('tokenInterceptorService');

    $stateProvider
        .state('main',
        {
            url: '/main',
            templateUrl: 'scripts/app/Views/main.html',
            controller: 'MainCtrl'
        })

        .state('myitems',
        {
            url: '/myitems',
            templateUrl: '/scripts/app/Views/items.html',
            controller: 'ProductCtrl'
        })

        .state('articlesDetails',
        {
            url: '/articlesDetails/:ArticleId',
            templateUrl: '/scripts/app/Views/article.html',
            controller: 'ArticleDetailsCtrl',
            resolve: {
                currentArticle: ['$stateParams', 'articleService', function ($stateParams, articleService) {
                    return articleService.getSpecificArticle($stateParams.ArticleId, null, function (response) {
                        return response.data;
                    });
                }]
            }
        })

        .state('users',
        {
            url: '/users',
            templateUrl: '/scripts/app/Views/users.html',
            controller: 'UserCtrl'
        })

        .state('articles',
        {
            url: '/articles',
            templateUrl: '/scripts/app/Views/articles.html',
            controller: 'ArticleCtrl'
        });

    $urlRouterProvider.otherwise('/main');

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
