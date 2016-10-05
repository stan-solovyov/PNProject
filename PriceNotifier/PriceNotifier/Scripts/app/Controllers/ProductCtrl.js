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

    $scope.main = {
        page: 1,
        take: 100
    };

    $scope.openNotificatnHistory = function (size, productId) {
        $scope.productId = productId;
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/scripts/app/Views/priceHistory.html',
            controller: ModalInstanceCtrl,
            scope: $scope,
            size: size,
            resolve: {
                priceChanges: function () {
                    return priceChangeService.getPriceChangesPage(productId, $scope.main.page, $scope.main.take);
                }
            }
        });
    };

    $scope.format = function (date) {
        var dateFormatted = moment(date);
        var d = new Date(dateFormatted);
        return d;
    };

    function ModalInstanceCtrl($uibModalInstance, priceChanges) {
        if (priceChanges.length === 0) {
            $scope.Note = "You don't have any history for this item.";
            $scope.show = true;
            $scope.showChart = false;
        } else {
            $scope.Note = null;
            $scope.show = false;
            $scope.showChart = true;
            $scope.priceChanges = priceChanges.data.Items;
            $scope.main.pages = Math.ceil(priceChanges.data.Count / $scope.main.take);
            var datesForChart = [];
            var pricesForChart = [];
            var dataChart = [];
            angular.forEach($scope.priceChanges,
                function (p) {
                    var date = new Date(p.Date);
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

        $scope.next = function () {
            if ($scope.main.page > 1) {
                $scope.main.page--;
                $scope.loadPage($scope.productId);
            }
        };

        $scope.previous = function () {
            if ($scope.main.page < $scope.main.pages) {
                $scope.main.page++;
                $scope.loadPage($scope.productId);
            }
        };

        $scope.loadPage = function (productId) {
            priceChangeService.getPriceChangesPage(productId, $scope.main.page, $scope.main.take).then(function (response) {
                $scope.historyPrices = response.data.Items;
                var pChart = [];
                var dChart = [];
                var dFinal = [];
                angular.forEach($scope.historyPrices,
                    function (p) {
                        var date = new Date(p.Date);
                        dChart.push(date);
                        pChart.push(p.NewPrice);
                    });

                for (var t = 0; t < pChart.length; t++) {
                    var b = {
                        x: dChart[t],
                        y: pChart[t]
                    };
                    dFinal.push(b);
                }
                $scope.data = [dFinal];
            });
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

