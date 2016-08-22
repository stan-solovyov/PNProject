app.factory("productService", ['$http','toaster',
    function ($http,toaster) {

        var url = '/api/Products';

        var getProducts = function () {
            return $http.get(url)
                .then(function (response) {
                    return response;
                });
        }

        var updateItem = function(product) {
            var id = product.Id;
            if (product.Checked === true) {
                return $http.put('/api/Products/' + id,
                JSON.stringify(product)
            ).success(function () {
                toaster.pop('note', "", "You began tracking this item.");
            });
            };
            return $http.put('/api/Products/' + id,
                JSON.stringify(product)
            ).success(function () {
                toaster.pop('note', "", "The item will no longer be tracked.");
                });
        };

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
            updateItem:updateItem,
            removeItem:removeItem
        };
    }]);