app.controller('ProductCtrl', ['$scope', 'productService', 'priceChangeService', '$uibModal', function ($scope, productService, priceChangeService, $uibModal) {

    $scope.price = $.connection.priceHub;
    $scope.price.client.updatePrice = function (p) {
        $scope.updatedPrice = p;
        angular.forEach($scope.dbproducts,
            function (product) {
                if (product.Id === $scope.updatedPrice.ProductId) {
                    product.Price = $scope.updatedPrice.Price;
                    $scope.boxClass = true;
                    $scope.$apply();
                }
            });
    };

    $.connection.hub.start()
     .done(function () {
         console.log('Now connected, connection ID=' + $.connection.hub.id);
         $scope.price.server.joinGroup("Clients");
     })
     .fail(function () { console.log('Could not Connect!'); });

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    $scope.openNotificatnHistory = function (size, productId) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/scripts/app/Views/priceHistory.html',
            controller: ModalInstanceCtrl,
            scope: $scope,
            size: size,
            resolve: {
                priceChanges: function () {
                    return priceChangeService.getPriceChanges(productId);
                }
            }
        });
    };

    function ModalInstanceCtrl($uibModalInstance, priceChanges) {
        var ctrl = $scope;
        if (priceChanges.length === 0) {
            $scope.Note = "You don't have any history for this item.";
            $scope.show = true;
        } else {
            $scope.Note = null;
            $scope.show = false;
            $scope.priceChanges = priceChanges;
        }

        $scope.ok = function () {
            $uibModalInstance.close();
        };
    };

    $scope.message = "There are no items in track list.";
    var onUserProducts = function (response) {
        $scope.dbproducts = response.data;
        if ($scope.dbproducts.length === 0 && response.status !== 204) {
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

