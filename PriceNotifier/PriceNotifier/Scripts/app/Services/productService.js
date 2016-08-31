app.factory("productService", ['$http','toaster',
    function ($http,toaster) {

        var url = '/api/Products/';

        var getProducts = function () {
            return $http.get(url)
                .then(function (response) {
                    return response;
                });
        }

        var addProducts = function (product) {
            product.hiding = true;

            var price;
            if (product.prices === null) {
                price = null;
            } else {
                price = product.prices.price_min.amount;
            }

            var productDto = {
                Name: product.full_name,
                Price: price,
                ExternalProductId: product.id,
                Url: product.html_url,
                ImageUrl: product.images.icon,
                Checked: true

            };

            return $http.post(url,
                    JSON.stringify(productDto),
                    {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                )
                .success(function() {
                    toaster.success({ title: "Congrats!", body: "The item was added to your list." });
                    product.hiding = false;
                }).error(function () { product.hiding = false; });

        };

        var updateItem = function(product) {
            if (product.Checked === true) {
                return $http.put(url,
                JSON.stringify(product)
            ).success(function () {
                toaster.pop('note', "", "You began tracking this item.");
            });
            };
            return $http.put(url,
                JSON.stringify(product)
            ).success(function () {
                toaster.pop('note', "", "The item will no longer be tracked.");
                });
        };

        var removeItem = function (product) {

            var id = product.Id;

            return $http({
                    method: 'DELETE',
                    url: url + id,
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
            addProducts:addProducts,
            updateItem:updateItem,
            removeItem:removeItem
        };
    }]);