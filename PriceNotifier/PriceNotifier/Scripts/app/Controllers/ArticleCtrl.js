app.controller('ArticleCtrl', ['$scope', 'articleService', 'productService', '$timeout', '$uibModal', 'limitToFilter', function ($scope, articleService, productService, $timeout, $uibModal, limitToFilter) {
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
                name: 'Id',
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
            {
                displayName: 'Date of creation',
                name: 'DateAdded',
                enableFiltering: false,
                enableSorting: true,
                enableCellEdit: false,
                cellFilter: 'date'
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
                    '<button type="button" class="btn btn-default" ng-click="grid.appScope.edit(row.entity)">Edit</button> <button type="button" class="btn btn-danger" ng-click="grid.appScope.remove(row.entity.Id)"> Remove</button>',
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
                    articleService.getArticles(true, columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
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
                articleService.getArticles(true, columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
            });
            gridApi.pagination.on.paginationChanged($scope,
                function (newPage, pageSize) {
                    paginationOptions.pageNumber = newPage;
                    paginationOptions.pageSize = pageSize;
                    articleService.getArticles(true, columnName, paginationOptions.sort, filter, filterColumn, newPage, pageSize).then(onGetArticles, onError);
                });
        }
    };

    $scope.gridOptions.appScopeProvider = $scope;

    //Datepicker
    $scope.today = function () {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.inlineOptions = {
        showWeeks: true
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.setDate = function (year, month, day) {
        $scope.dt = new Date(year, month, day);
    };

    $scope.formats = ['dd-MMMM-yyyy', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
    $scope.altInputFormats = ['M!/d!/yyyy'];

    $scope.popup1 = {
        opened: false
    };

    var onArticleDelete = function (id) {
        var data = $scope.gridOptions.data.filter(function (el) {
            return el.Id !== id;
        });
        $scope.gridOptions.data = data;
        $scope.validationMessages = null;
    };

    var onErrorValidate = function (response) {
        $scope.validationMessages = parseErrors(response);
    };

    function parseErrors(response) {
        var errors = [];
        for (var key in response.data.ModelState) {
            if (response.data.ModelState.hasOwnProperty(key)) {
                for (var i = 0; i < response.data.ModelState[key].length; i++) {
                    errors.push(response.data.ModelState[key][i]);
                }
            }
        }
        return errors;
    }

    $scope.remove = function (id) {
        articleService.removeArticle(id).then(onArticleDelete(id), onError);
    };

    var onArticleCreate = function () {
        articleService.getArticles(true,columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
        $scope.validationMessages = null;
    };

    $scope.update = function (article) {
        $scope.currentArticle = article;
        if (typeof (article.ProductName.Name) == "undefined") {
            articleService.updateArticle(article).then(onArticleCreate, onErrorValidate);
        } else {
            article.ProductId = article.ProductName.Id;
            article.ProductName = article.ProductName.Name;
            articleService.updateArticle(article).then(onArticleCreate, onErrorValidate);
        }
    };

    $scope.create = function (article) {
        article.DateAdded = $scope.dt;
        if (typeof (article.Name) == "undefined") {
            articleService.createArticle(article).then(onArticleCreate, onErrorValidate);
        } else {
            article.ProductId = article.Name.Id;
            article.Name = article.Name.Name;
            articleService.createArticle(article).then(onArticleCreate, onErrorValidate);
        }
    };

    $scope.items = function (value) {
        value = value.toLowerCase();
        return productService.getProducts(true, 1, 50, value).then(function (response) {
            return limitToFilter(response.data.Items, 15);
        });
    }

    $scope.addNewArticle = function () {
        var article = {};
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/scripts/app/Views/articleAdd.html',
            controller: ModalInstanceCtrl,
            backdrop: 'static',
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
                $scope.validationMessages = null;
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
            backdrop: 'static',
            size: 'lg',
            resolve: {
                article: article
            }
        });

        function ModalInstanceCtrl($uibModalInstance, article) {
            article.DateAdded = new Date(article.DateAdded);
            $scope.userArticle = angular.copy(article);
            $scope.ok = function () {
                $scope.validationMessages = null;
                $uibModalInstance.close();
            };
        };
    };

    articleService.getArticles(true, columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetArticles, onError);
}
]);