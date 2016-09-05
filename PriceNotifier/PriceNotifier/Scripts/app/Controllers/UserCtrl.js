app.controller('UserCtrl',
[
    '$scope', 'userService', 'uiGridValidateService', function ($scope, userService, uiGridValidateService) {
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
            $scope.gridOptions.totalItems = response.data.TotalItems;
            $scope.gridOptions.data = response.data.Data;
            if (response.data.Data.length === 0 && response.status !== 204) {
                $scope.Message = "You don't have any active users.";
                $scope.demonstrate = true;
            } else {
                $scope.Message = null;
                $scope.demonstrate = false;
            }
        };

        var onUserDelete = function () {
            userService.getUsers().then(onGetUsers, onError);
        };

        var onUserUpdate = function () {
        };

        $scope.remove = function (id) {
            userService.removeUser(id).then(onUserDelete, onError);
        };

        uiGridValidateService.setValidator('startWith',
        function (argument) {
            return function (newValue) {
                if (newValue==="") {
                    return true; // We should not test for existence here
                } else {
                    return newValue.startsWith(argument);
                }
            };
        },
        function (argument) {
            return 'You must insert username';
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
                    enableCellEdit: false
                },
                { name: 'Username', displayName: 'Username', headerCellClass: $scope.highlightFilteredHeader, enableCellEdit: true, validators: {  startWith: null }, cellTemplate: 'ui-grid/cellTitleValidator' },
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
                        if (sortColumns.length === 0) {
                            paginationOptions.sort = "";
                        } else {
                            paginationOptions.sort = sortColumns[0].sort.direction;
                            columnName = sortColumns[0].name;
                        }
                        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, "", "").then(onGetUsers, onError);
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
                    userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, "", "").then(onGetUsers, onError);
                });
                gridApi.pagination.on.paginationChanged($scope,
                    function (newPage, pageSize) {
                        columnName = "";
                        paginationOptions.pageNumber = newPage;
                        paginationOptions.pageSize = pageSize;
                        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, newPage, pageSize).then(onGetUsers, onError);
                    });
                gridApi.edit.on.afterCellEdit($scope, function () {
                    var rowCol = $scope.gridApi.cellNav.getFocusedCell();
                    if (rowCol !== null) {
                        userService.updateUser(rowCol.row.entity).then(onUserUpdate, onError);
                    }
                });
                gridApi.validate.on.validationFailed($scope, function () {
                    //alert("Please insert username");
                });
            }
        };

        $scope.gridOptions.appScopeProvider = $scope;

        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn, null, null).then(onGetUsers, onError);
    }
]);