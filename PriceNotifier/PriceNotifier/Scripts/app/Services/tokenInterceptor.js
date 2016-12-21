app.factory('tokenInterceptorService', ['$q', 'tokenService', 'toaster', function ($q, tokenService, toaster) {

    var tokenInterceptorServiceFactory = {};

    var request = function (config) {
        var authData = tokenService.getToken();

        if (authData && (!config.url.includes('onliner')) && (!config.url.includes('yahoo'))) {
            config.headers["X-Auth"] = authData;
        }

        return config;
    }

    var responseError = function (rejection) {
        if (rejection.status === 401) {
            tokenService.logout();
        }

        if (rejection.status === 409) {
            toaster.pop('warning', "Beware!", "The item is already in your list");
        }
        return $q.reject(rejection);
    }

    tokenInterceptorServiceFactory.request = request;
    tokenInterceptorServiceFactory.responseError = responseError;

    return tokenInterceptorServiceFactory;
}]);