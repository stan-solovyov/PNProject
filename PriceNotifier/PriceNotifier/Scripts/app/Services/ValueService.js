app.factory("valueService", ['$http',
    function ($http) {

        var url = '/api/Values';

        var getValues = function () {
            return $http.get(url)
                .then(function (response) {
                    return response.data;
                });
        }

        return {
            getValues: getValues
        };
    }]);