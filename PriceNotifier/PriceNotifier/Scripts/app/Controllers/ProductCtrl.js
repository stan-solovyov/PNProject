app.controller('ProductCtrl', ['$scope', 'productService', 'priceChangeService', '$uibModal', function ($scope, productService, priceChangeService, $uibModal) {

    var datesForChart = [];
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

    $scope.format = function (date) {
        var dateFormatted = Date.parse(date);
        return dateFormatted;
    };

    function ModalInstanceCtrl($uibModalInstance, priceChanges) {
        var ctrl = $scope;
        if (priceChanges.length === 0) {
            $scope.Note = "You don't have any history for this item.";
            $scope.show = true;
            $scope.showChart = false;
        } else {
            $scope.Note = null;
            $scope.show = false;
            $scope.showChart = true;
            $scope.priceChanges = priceChanges;
            var pricesForChart = [];
            var dataChart = [];
            angular.forEach(priceChanges,
                function (p) {
                    var date = moment(p.Date);
                    datesForChart.push(date);
                    pricesForChart.push(p.NewPrice);
                });

            for (var i = 0; i < pricesForChart.length; i++) {
                var a = {
                    x: datesForChart[i],
                    y: pricesForChart[i]
                };
                dataChart.push(a);
            }

            $scope.data = [dataChart];


            $scope.colors = ['#45b7cd', '#ff6384', '#ff8e72'];
            $scope.series = ['Price change, BYN'];
            $scope.labels = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

            $scope.datasetOverride = [
                {
                    borderWidth: 3,
                    hoverBackgroundColor: "rgba(255,99,132,0.4)",
                    hoverBorderColor: "rgba(255,99,132,1)",
                    type: 'line'
                }
            ];

            $scope.options = {
                scales: {
                    xAxes: [
                        {
                            type: 'time',
                            time: {
                                //unit:'hour',
                                displayFormats: {
                                    month: 'MMM D, hA'
                                }
                            },
                            position: 'bottom'
                        }
                    ]
                }
            };
        };
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

