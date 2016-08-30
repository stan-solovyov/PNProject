﻿
app.controller('MainCtrl', ['$scope', 'tokenService', 'externalProductService', 'productService', 'pagerService', function ($scope, tokenService, externalProductService,productService, pagerService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
        $scope.flag = false;
    };

    var onUserCompleteProducts = function (data) {
        $scope.products = data.products;
        $scope.pager = pagerService.getPager(data.total, data.page.current);
    };

    $scope.setPage = setPage;

    function setPage(page) {
        $scope.pager = {};
        externalProductService.getExternalProducts($scope.productname, page)
            .then(onUserCompleteProducts, onError);
    }

    var onUserAddProducts = function (product) {
        product.hiding = false;
    };

    $scope.Logout = function () {
        tokenService.logout();
    };

    $scope.addToList = function (product) {
        product.hiding = true;
        productService.addProducts(product).then(onUserAddProducts(product), onError);
    };

    $scope.search = function (productname) {
        $scope.productname = productname;
        externalProductService.getExternalProducts(productname).then(onUserCompleteProducts, onError);
    };
}
]);
