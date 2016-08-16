app.factory("externalProductService", ['$http',
    function ($http) {

        var url = 'https://catalog.api.onliner.by/search/products?query=';

        var getProducts = function (productname) {
            return $http.get(url + productname)
                .then(function (response) {
                    return response.data;
                });
        };

        var addProducts = function(product) {
            var productDto = {
                Name: product.full_name,
                Price: product.prices.price_min.amount,
                ProductId: product.id,
                Url: product.html_url,
                ImageUrl: product.images.icon
                
            };

           return $http.post('/api/Products',
                JSON.stringify(productDto),
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            );

        } 

        return {
            getExternalProducts: getProducts,
            addProducts:addProducts
        };
    }]);