
app.controller('MainCtrl', ['$scope', 'tokenService', 'externalProductService' ,function ($scope, tokenService, externalProductService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
        $scope.flag = false;
    };

    
    var onUserCompleteProducts = function (data) {
        $scope.products = data.products;
    };

    var onUserAddProducts = function (product) {
        product.hiding = false;
        
    };

    $scope.Logout = function () {
        tokenService.logout();
    };

    $scope.addToList = function (product) {
       product.hiding = true;
        externalProductService.addProducts(product).then(onUserAddProducts(product), onError);
    };

    $scope.search = function (productname) {
        externalProductService.getExternalProducts(productname).then(onUserCompleteProducts, onError);
    };
}
]);
