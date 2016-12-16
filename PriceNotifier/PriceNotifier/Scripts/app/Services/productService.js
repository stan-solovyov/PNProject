app.factory("productService", ['$http', 'toaster',
    function ($http, toaster) {

        var url = '/api/Products/';

        var getProducts = function (showAllProducts, currentPage, recordsPerPage, query) {

            var request = url + "?showAllProducts=" + showAllProducts;
            if (currentPage && recordsPerPage) {
                request = request + "&$skip=" + (currentPage - 1) * recordsPerPage + "&$top=" + recordsPerPage + "&$count=true";
            }

            if (query) {
                request = request + "&query=" + query;
            } else {
                request = request + "&query=";
            }

            return $http.get(request)
                .then(function (response) {
                    return response;
                });
        }

        var getFilteredProducts = function (showAllProducts, productsId) {
            var request = url + "?showAllProducts=" + showAllProducts;
            if (productsId.length !== 0) {
                request = request + "&$filter=" + 'ExternalProductId' + '%20' + 'eq' + '%20' + '%27' + productsId[0].id.toString() + '%27';
                if (productsId.length > 1) {
                    for (var i = 1; i < productsId.length; i++) {
                        request = request + '%20' + 'or' + '%20' + 'ExternalProductId' + '%20' + 'eq' + '%20' + '%27' + productsId[i].id.toString() + '%27';
                    }
                }
            }

            return $http.get(request)
                .then(function (response) {
                    return response;
                });
        }

        var addProducts = function (product) {
            product.hiding = true;

            var MinPrice, MaxPrice;
            if (product.prices === null) {
                MinPrice = 0;
                MaxPrice = 0;
            } else {
                MinPrice = product.prices.price_min.amount;
                MaxPrice = product.prices.price_max.amount;
            }

            var productDto = {
                Name: product.full_name,
                MinPrice: MinPrice,
                MaxPrice: MaxPrice,
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
                .success(function () {
                    toaster.success({ title: "Congrats!", body: "The item was added to your list." });
                    product.hiding = false;
                }).error(function () { product.hiding = false; });

        };

        var updateItem = function (product) {
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
            getFilteredProducts: getFilteredProducts,
            addProducts: addProducts,
            updateItem: updateItem,
            removeItem: removeItem
        };
    }]);