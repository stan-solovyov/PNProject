
app.controller('MainCtrl', ['$scope', 'tokenService', 'externalProductService', 'productService', 'pagerService', '$timeout', function ($scope, tokenService, externalProductService, productService, pagerService, $timeout) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    $scope.showButtons = false;
    var onUserCompleteProducts = function (data) {
        $scope.products = data.products;
        $scope.pager = pagerService.getPager(data.total, data.page.current);
        $scope.totalPages = data.page.last;
        productService.getFilteredProducts(false, data.products).then(function (userProducts) {
            $scope.userProducts = userProducts.data.Items;

            for (var j = 0; j < $scope.products.length; j++) {
                $scope.products[j].alreadyInTheList = false;
                for (var i = 0; i < $scope.userProducts.length; i++) {
                    if ($scope.userProducts[i].ExternalProductId === $scope.products[j].id.toString()) {
                        $scope.products[j].alreadyInTheList = true;
                        console.log($scope.userProducts[i].ExternalProductId, $scope.products[j].id.toString());
                        break;
                    }
                }
            }

            $scope.showButtons = true;
        }, onError);
        $scope.showLoading = false;
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
        $scope.showLoading = true;
        $scope.productname = productname;
        externalProductService.getExternalProducts(productname).then(onUserCompleteProducts, onError);
    };
}
]);
