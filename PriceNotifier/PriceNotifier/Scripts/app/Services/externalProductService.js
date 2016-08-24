app.factory("externalProductService", ['$http','toaster',
    function ($http,toaster) {

        var url = 'https://catalog.api.onliner.by/search/products?query=';

        var getExternalProducts = function (productname) {
            return $http.get(url + productname)
                .then(function (response) {
                    return response.data;
                });
        };

        var addProducts = function(product) {
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
                Checked:true

            };

            return $http.post('/api/Products',
                    JSON.stringify(productDto),
                    {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }
                )
                .success(function() {
                    toaster.success({ title: "Congrats!", body: "The item was added to your list." });
                });

        };

        return {
            getExternalProducts:getExternalProducts,
            addProducts:addProducts
        };
    }]);