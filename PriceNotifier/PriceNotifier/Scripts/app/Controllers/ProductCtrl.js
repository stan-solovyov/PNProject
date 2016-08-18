app.controller('ProductCtrl', ['$scope', 'productService', 'toaster', function ($scope, productService, toaster) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    $scope.message = "There are no items in thrack list.";
    var onUserProducts = function (response) {
        $scope.dbproducts = response.data;
    };

    $scope.pop = function () {
        toaster.pop('success', "title", "text");
    };

    var onUserDelete = function () {
        productService.getProducts().then(onUserProducts, onError);
        $scope.pop();
    };

    $scope.remove = function (product) {
        productService.removeItem(product).then(onUserDelete, onError);
    };

    productService.getProducts().then(onUserProducts, onError);
}
]);