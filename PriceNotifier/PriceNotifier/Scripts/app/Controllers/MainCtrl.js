
app.controller('MainCtrl', ['$scope', 'tokenService', 'externalProductService', 'productService', 'pagerService', function ($scope, tokenService, externalProductService,productService, pagerService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    var onUserCompleteProducts = function (data) {
        $scope.products = data.products;
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
        productService.addProducts(product).success(onUserAddProducts).error(onError);
    };

    $scope.search = function (productname) {
        $scope.productname = productname;
        externalProductService.getExternalProducts(productname).then(onUserCompleteProducts, onError);
    };
}
]);
