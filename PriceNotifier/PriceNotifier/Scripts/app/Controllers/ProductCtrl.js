app.controller('ProductCtrl', ['$scope', '$location', 'productService', 'priceChangeService', '$uibModal', 'pagerService', function ($scope, $location, productService, priceChangeService, $uibModal, pagerService) {

    $scope.price = $.connection.priceHub;
    $scope.price.client.updatePrice = function (p) {
        $scope.updatedPrice = p;
        angular.forEach($scope.dbproducts,
            function (product) {
                if (product.Id === $scope.updatedPrice.ProductId) {
                    product.MinPrice = $scope.updatedPrice.Price;
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

    $scope.provider = 'Onliner';
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
                    return priceChangeService.getPriceChangesPage(productId, $scope.provider, $scope.main.page, $scope.main.take);
                }
            }
        });
    };

    function ModalInstanceCtrl($uibModalInstance, priceChanges) {
        $scope.showPages = true;
        if (priceChanges.data.Count === 0) {
            $scope.Note = "You don't have any history for this item.";
            $scope.show = true;
            $scope.showChart = false;
            $scope.showPages = false;
        } else {
            $scope.Note = null;
            $scope.show = false;
            $scope.showChart = true;
            $scope.priceChanges = priceChanges.data.Items;
            $scope.main.pages = Math.ceil(priceChanges.data.Count / $scope.main.take);
            if ($scope.main.pages === 1) {
                $scope.showPages = false;
            };
            var datesForChart = [];
            var pricesForChart = [];
            var dataChart = [];
            angular.forEach($scope.priceChanges,
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
                                    month: 'MMM D, YYYY'
                                }
                            },
                            position: 'bottom'
                        }
                    ]
                }
            };
        };

        $scope.openOnlinerHistory = function () {
            $scope.provider = 'Onliner';
            $scope.main.page = 1;
            priceChangeService.getPriceChangesPage($scope.productId, $scope.provider, $scope.main.page, $scope.main.take).then(function (response) {
                ModalInstanceCtrl($uibModalInstance, response);
            });
        }

        $scope.openMigomHistory = function () {
            $scope.provider = 'Migom';
            $scope.main.page = 1;
            priceChangeService.getPriceChangesPage($scope.productId, $scope.provider, $scope.main.page, $scope.main.take).then(function (response) {
                ModalInstanceCtrl($uibModalInstance, response);
            });
        }

        $scope.open1KHistory = function () {
            var provider = '1K';
            $scope.main.page = 1;
            priceChangeService.getPriceChangesPage($scope.productId, provider, $scope.main.page, $scope.main.take).then(function (response) {
                ModalInstanceCtrl($uibModalInstance, response);
            });
        }


        $scope.ok = function () {
            $scope.main.page = 1;
            $uibModalInstance.close();
        };

        $scope.next = function () {
            if ($scope.main.page > 1) {
                $scope.main.page--;
                $scope.loadPage($scope.productId, $scope.provider);
            }
        };

        $scope.previous = function () {
            if ($scope.main.page < $scope.main.pages) {
                $scope.main.page++;
                $scope.loadPage($scope.productId, $scope.provider);
            }
        };

        $scope.loadPage = function (productId, provider) {
            priceChangeService.getPriceChangesPage(productId, provider, $scope.main.page, $scope.main.take).then(function (response) {
                $scope.historyPrices = response.data.Items;
                var pChart = [];
                var dChart = [];
                var dFinal = [];
                angular.forEach($scope.historyPrices,
                    function (p) {
                        var date = moment(p.Date);
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

    var pageSize = 10;
    var currentPage = 1;

    $scope.message = "There are no items in track list.";
    var onUserProducts = function (response) {
        $scope.pager = pagerService.getPager(response.data.Count, currentPage);
        $scope.totalPages = Math.ceil(response.data.Count / pageSize);
        $scope.dbproducts = response.data.Items;
        if ($scope.dbproducts.length === 0 && response.status !== 204) {
            $scope.Message = "You don't have any items in the list yet.";
            $scope.demonstrate = true;
        } else {
            $scope.Message = null;
            $scope.demonstrate = false;
        }
    };

    $scope.setPage = setPage;
    function setPage(page) {
        if (page < 1 || page > $scope.totalPages) {
            return;
        }
        currentPage = page;
        productService.getProducts(false, page, pageSize,$scope.query).then(onUserProducts, onError);
    }

    var onUserDelete = function () {
        productService.getProducts(false, currentPage, pageSize, $scope.query).then(onUserProducts, onError);
    };

    $scope.update = function (product) {
        productService.updateItem(product).then(onUserDelete, onError);
    };

    $scope.remove = function (product) {
        productService.removeItem(product).then(onUserDelete, onError);
    };

    $scope.search = function () {
        if ($scope.query) {
            currentPage = 1;
            productService.getProducts(false, currentPage, pageSize, $scope.query).then(onUserProducts, onError);
        }
    };

    productService.getProducts(false, currentPage, pageSize,null).then(onUserProducts, onError);
}
]);

