
app.controller('MainCtrl', ['$scope', 'tokenService', 'valueService', 'externalProductService', 'toaster', function ($scope, tokenService, valueService, externalProductService, toaster) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
        $scope.flag = false;
    };

    var onUserCompleteValues = function (data) {
        $scope.values = data;
    };

    var onUserCompleteProducts = function (data) {
        $scope.products = data.products;
    };

    var onUserAddProducts = function () {
        $scope.flag = false;
        toaster.success({ title: "Congrats!", body: "The item was added to your list." });
    };

    $scope.Logout = function () {
        tokenService.logout();
    };

    $scope.addToList = function (product) {
        $scope.flag = true;
        externalProductService.addProducts(product).then(onUserAddProducts, onError);
    };

    $scope.search = function (productname) {
        externalProductService.getExternalProducts(productname).then(onUserCompleteProducts, onError);
    };
}
]);
