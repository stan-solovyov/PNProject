app.controller('UserCtrl',
[
    '$scope', 'userService', 'uiGridValidateService', '$timeout', function ($scope, userService, uiGridValidateService, $timeout) {
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

        var onGetUsers = function (response) {
            $scope.gridOptions.totalItems = response.data.Count;
            $scope.gridOptions.data = response.data.Items;
            if (response.data.Items.length === 0 && response.status !== 204) {
                $scope.Message = "You don't have any users.";
                $scope.demonstrate = true;
            } else {
                $scope.Message = null;
                $scope.demonstrate = false;
            }
        };

        var onUserUpdate = function () {
        };


        var validationFactory = function (newValue) {
            if ((/^[а-яА-Яa-zA-Z]+$/.test(newValue)) && newValue.length < 25 && newValue !== "") {
                return true;
            } else {
                return false;
            }
        };

        uiGridValidateService.setValidator('notEmpty',
        function () {
            return function (oldValue, newValue) {
                return validationFactory(newValue);
            };
        },
        function () {
            return 'Username should contain only alphabetical characters be less than 25 characters long.';
        }
  );

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
                { name: 'Username', displayName: 'Username', headerCellClass: $scope.highlightFilteredHeader, enableCellEdit: true, validators: { notEmpty: "" }, cellTemplate: 'ui-grid/cellTitleValidator' },
                // pre-populated search field
                {
                    displayName: 'Social Network',
                    name: 'SocialNetworkName',
                    enableSorting: false,
                    enableFiltering: false,
                    enableCellEdit: false
                },
                // no filter input
                {
                    displayName: 'Social network UserId',
                    name: 'SocialNetworkUserId',
                    enableFiltering: false,
                    enableSorting: false,
                    enableCellEdit: false
                },
                {
                    displayName: 'Total tracked items',
                    name: 'CountTrackedItems',
                    enableFiltering: false,
                    enableSorting: true,
                    enableCellEdit: false
                },
                {
                    name: ' ',
                    cellTemplate:
                        '<button type="button" class="btn btn-danger" ng-click="grid.appScope.remove(row.entity.Id)"> Remove user </button>',
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
                        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetUsers, onError);
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
                    userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetUsers, onError);
                });
                gridApi.pagination.on.paginationChanged($scope,
                    function (newPage, pageSize) {
                        paginationOptions.pageNumber = newPage;
                        paginationOptions.pageSize = pageSize;
                        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, newPage, pageSize).then(onGetUsers, onError);
                    });
                gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef) {
                    var rowCol = $scope.gridApi.cellNav.getFocusedCell();
                    var a;
                    $timeout(function () {
                        a = uiGridValidateService.isInvalid(rowEntity, colDef);
                        if (!a) {
                            userService.updateUser(rowCol.row.entity).then(onUserUpdate, onError);
                        }
                    }, 0);
                });
            }
        };

        $scope.gridOptions.appScopeProvider = $scope;

        var onUserDelete = function () {
            userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetUsers, onError);
        };

        $scope.remove = function (id) {
            userService.removeUser(id).then(onUserDelete, onError);
        };

        if (!columnName && !filterColumn) {
            userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, paginationOptions.pageNumber, paginationOptions.pageSize).then(onGetUsers, onError);
        }
    }
]);