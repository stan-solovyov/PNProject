app.controller('ArticleCtrl', ['$scope', 'articleService', 'productService', '$timeout', '$uibModal', function ($scope, articleService, productService, $timeout, $uibModal) {
    var columnName = "", filterColumn = "", filter = '';

    var paginationOptions = {
        pageNumber: 1,
        pageSize: 25,
        sort: ""
    };
    paginationOptions.sort = "";
    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    var onGetArticles = function (response) {
        $scope.gridOptions.totalItems = response.data.Count;
        $scope.gridOptions.data = response.data.Items;
        if (response.data.Items.length === 0 && response.status !== 204) {
            $scope.Message = "You don't have any articles.";
            $scope.demonstrate = true;
        } else {
            $scope.Message = null;
            $scope.demonstrate = false;
        }
    };

    var onArticleUpdate = function () {
    };


    $scope.gridOptions = {
        paginationPageSizes: [25, 50, 75],
        paginationPageSize: 25,
        enableFiltering: true,
        useExternalPagination: true,
        useExternalSorting: true,
        useExternalFiltering: true,
        enableCellEditOnFocus: true,
        columnDefs: [
            // default
            {
                name: 'ArticleId',
                displayName: 'Id',
                enableFiltering: false,
                enableCellEdit: false
            },
            { name: 'Title', displayName: 'Title', headerCellClass: $scope.highlightFilteredHeader, enableCellEdit: false },
            // pre-populated search field
            {
                displayName: 'Summary',
                name: 'Summary',
                enableSorting: false,
                enableFiltering: false,
                enableCellEdit: false
            },
            // no filter input
            {
                displayName: 'Body',
                name: 'Body',
                enableFiltering: false,
                enableSorting: false,
                enableCellEdit: false
            },
            {
                displayName: 'DateAdded',
                name: 'DateAdded',
                enableFiltering: false,
                enableSorting: true,
                enableCellEdit: false
            },
            {
                displayName: 'Ready to publish',
                name: 'IsPublished',
                enableFiltering: false,
                enableSorting: false,
                enableCellEdit: false
            },
            {
                name: ' ',
                cellTemplate:
                    '<button type="button" class="btn btn-danger" ng-click="grid.appScope.remove(row.entity.ArticleId)"> Remove</button> <button type="button" class="btn btn-default" ng-click="grid.appScope.edit(row.entity)">Edit</button>',
                enableFiltering: false,
                enableSorting: false,
                enableCellEdit: false
            }
        ],
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
            $scope.gridApi.core.on.sortChanged($scope,
                function (grid, sortColumns) {
                    columnName = "";
                    paginationOptions.sort = "";
                    if (sortColumns.length === 0) {
                        paginationOptions.sort = "";
                    } else {
                        paginationOptions.sort = sortColumns[0].sort.direction;
                        columnName = sortColumns[0].name;
                    }
                    articleService.getArticles(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
                });
            $scope.gridApi.core.on.filterChanged($scope, function () {
                var grid = this.grid;
                filter = '';
                filterColumn = "";
                angular.forEach(grid.columns, function (value) {
                    if (value.filters[0].term) {
                        filterColumn = value.colDef.name;
                        filter = value.filters[0].term;
                    }
                });
                articleService.getArticles(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
            });
            gridApi.pagination.on.paginationChanged($scope,
                function (newPage, pageSize) {
                    paginationOptions.pageNumber = newPage;
                    paginationOptions.pageSize = pageSize;
                    articleService.getArticles(columnName, paginationOptions.sort, filter, filterColumn, newPage, pageSize).then(onGetArticles, onError);
                });
        }
    };

    $scope.gridOptions.appScopeProvider = $scope;

    var onArticleDelete = function () {
        articleService.getArticles(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
    };

    $scope.remove = function (id) {
        articleService.removeArticle(id).then(onArticleDelete, onError);
    };

    $scope.update = function (article) {
        articleService.updateArticle(article).then(onArticleDelete, onError);
    };

    $scope.create = function (article) {
        article.DateAdded = new Date(Date.now());
        article.ProductId = article.ProductId.Id;
        articleService.createArticle(article).then(onArticleDelete, onError);
    };

    $scope.addNewArticle = function () {
        productService.getProducts().then(function (response) {
            $scope.items = response.data;
        });
        var article = {};
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/scripts/app/Views/articleAdd.html',
            controller: ModalInstanceCtrl,
            scope: $scope,
            size: 'lg',
            resolve: {
                article: article
            }
        });

        function ModalInstanceCtrl($uibModalInstance, article) {
            $scope.article = article;
            article.IsPublished = true;
            $scope.ok = function () {
                $uibModalInstance.close();
            };
        };
    }

    $scope.edit = function (article) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/scripts/app/Views/articleEdit.html',
            controller: ModalInstanceCtrl,
            scope: $scope,
            size: 'lg',
            resolve: {
                article: article
            }
        });

        function ModalInstanceCtrl($uibModalInstance, article) {
            $scope.article = article;
            $scope.ok = function () {
                $uibModalInstance.close();
            };
        };
    };

    articleService.getArticles(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
}
]);