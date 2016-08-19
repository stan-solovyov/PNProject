app.factory("productService", ['$http','toaster',
    function ($http,toaster) {

        var url = '/api/Products';

        var getProducts = function () {
            return $http.get(url)
                .then(function (response) {
                    return response;
                });
        }

        var removeItem = function (product) {

            var id = product.Id;

            return $http({
                    method: 'DELETE',
                    url: '/api/Products/' + id,
                    headers: {
                        'Content-type': 'application/json'
                    }
                })
                .success(function () {
                    toaster.pop('success', "Congrats!", "The item was removed from your list.");
                });

        };

        return {
            getProducts: getProducts,
            removeItem:removeItem
        };
    }]);