app.factory("priceChangeService", ['$http',
    function ($http) {

        var url = '/api/PriceHistories/';

        var getPriceChanges = function (productId) {

            return $http({
                method: 'get',
                url: url + productId,
                headers: {
                    'Content-type': 'application/json'
                }
            }).then(function (response) {
                return response.data;
            });
        };

        return {
            getPriceChanges: getPriceChanges
        };
    }]);