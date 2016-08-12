app.factory("valueService", ['$http',
    function ($http) {

        var url = 'https://catalog.api.onliner.by/search/products?query=iphon';

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