app.controller('ProductCtrl', ['$scope', 'productService', function ($scope, productService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    $scope.message = "There are no items in thrack list.";
    var onUserProducts = function (response) {
        $scope.dbproducts = response.data;
        if ($scope.dbproducts.length === 0 && response.status!== 204) {
            $scope.Message = "You don't have any items in the list yet.";
            $scope.demonstrate = true;
        } else {
            $scope.Message = null;
            $scope.demonstrate = false;
        }
    };

    var onUserDelete = function () {
        productService.getProducts().then(onUserProducts, onError);
    };

    $scope.update = function (product) {
        productService.updateItem(product).then(onUserDelete, onError);
    };

    $scope.remove = function (product) {
        productService.removeItem(product).then(onUserDelete, onError);
    };

    productService.getProducts().then(onUserProducts, onError);
}
]);