app.factory("externalProductService", ['$http',
    function ($http) {

        var url = 'https://catalog.api.onliner.by/search/products?query=';

        var getExternalProducts = function (productname, page) {
            if (page === undefined || page<1) {
                page = 1;
            }

            return $http.get(url + productname + "&page=" + page)
                .then(function (response) {
                    return response.data;
                });
        };

        return {
            getExternalProducts: getExternalProducts
        };
    }]);