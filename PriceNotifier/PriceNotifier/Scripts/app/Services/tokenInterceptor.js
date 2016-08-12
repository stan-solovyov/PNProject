app.factory('tokenInterceptorService', ['$q', 'tokenService', function ($q, tokenService) {

    var tokenInterceptorServiceFactory = {};

    var request = function (config) {
        var authData = tokenService.getToken();
        if (authData) {
           config.headers["X-Auth"] = authData;
        }

        return config;
    }

    var responseError = function (rejection) {
        if (rejection.status === 401) {
            tokenService.Logout();
        }

        return $q.reject(rejection);
    }

    tokenInterceptorServiceFactory.request = request;
    tokenInterceptorServiceFactory.responseError = responseError;

    return tokenInterceptorServiceFactory;
}]);