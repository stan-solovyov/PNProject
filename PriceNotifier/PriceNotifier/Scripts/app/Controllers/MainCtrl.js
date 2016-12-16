
app.controller('MainCtrl', ['$scope', 'tokenService', 'externalProductService', 'productService', 'pagerService', function ($scope, tokenService, externalProductService, productService, pagerService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    var onUserCompleteProducts = function (data) {
        $scope.products = data.products;
        productService.getFilteredProducts(false, data.products).then(function(data) {
            $scope.userProducts = data.data.Items;
            angular.forEach($scope.userProducts,
            function (productDB) {
                angular.forEach($scope.products,
            function (product) {
                var a = product.id.toString();
                if (productDB.ExternalProductId === a) {
                    product.alreadyInTheList = true;
                }
            });
            });
        }, onError);
        $scope.pager = pagerService.getPager(data.total, data.page.current);
        $scope.totalPages = data.page.last;
    };


    $scope.setPage = setPage;
    function setPage(page) {
        if (page < 1 || page > $scope.totalPages) {
            return;
        }

        externalProductService.getExternalProducts($scope.productname, page)
            .then(onUserCompleteProducts, onError);
    }

    var onUserAddProducts = function () {
    };

    $scope.Logout = function () {
        tokenService.logout();
    };

    $scope.addToList = function (product) {
        product.hiding = true;
        productService.addProducts(product).success(function () {
            product.alreadyInTheList = true;
            return onUserAddProducts;
        }).error(onError);
    };

    $scope.search = function (productname) {
        $scope.productname = productname;
        externalProductService.getExternalProducts(productname).then(onUserCompleteProducts, onError);
    };
}
]);
