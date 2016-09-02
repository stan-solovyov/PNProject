app.controller('UserCtrl',
[
    '$scope', 'userService', 'uiGridConstants', function($scope, userService, uiGridConstants) {
        var columnName="",filterColumn, filter = '';

        var paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        var onError = function () {
            $scope.error = "Couldn't get response from the server:(";
        };

        var onGetUsers = function (response) {
            $scope.gridOptions.totalItems = response.data.length;
            var firstRow = (paginationOptions.pageNumber - 1) * paginationOptions.pageSize;
            $scope.gridOptions.data = response.data.slice(firstRow, firstRow + paginationOptions.pageSize);
            if (response.data.length === 0 && response.status !== 204) {
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

        $scope.update = function (user) {
            userService.updateItem(user).then(onUserDelete, onError);
        };

        $scope.remove = function (id) {
            userService.removeUser(id).then(onUserDelete, onError);
        };

        $scope.gridOptions = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            enableFiltering: true,
            useExternalPagination: true,
            useExternalSorting: true,
            useExternalFiltering: true,
            columnDefs: [
                // default
                {
                    name: 'Id',
                    displayName: 'Id'
                },
                { name: 'Username', displayName: 'Username', headerCellClass: $scope.highlightFilteredHeader },
                // pre-populated search field
                {
                    displayName: 'Social Network',
                    name: 'SocialNetworkName',
                    enableSorting: false,
                    enableFiltering: false
                },
                // no filter input
                {
                    displayName: 'Social network UserId',
                    name: 'SocialNetworkUserId',
                    enableFiltering: false,
                    enableSorting: false
                },
                {
                    name: ' ',
                    cellTemplate:
                        '<button type="button" class="btn btn-danger" ng-click="grid.appScope.remove(row.entity.Id)"> Remove user </button>',
                    enableFiltering: false,
                    enableSorting: false
                }
            ],
            onRegisterApi: function(gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope,
                    function(grid, sortColumns) {
                        if (sortColumns.length === 0) {
                            paginationOptions.sort = null;
                        } else {
                            paginationOptions.sort = sortColumns[0].sort.direction;
                            columnName = sortColumns[0].name;
                        }
                        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn).then(onGetUsers, onError);
                    });
                $scope.gridApi.core.on.filterChanged( $scope, function() {
                    var grid = this.grid;
                    filter = '';
                    filterColumn = null;
                    angular.forEach(grid.columns, function (value) {
                        if (value.filters[0].term) {
                            filterColumn = value.colDef.name;
                            filter = value.filters[0].term;
                        }
                    });
                    userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn).then(onGetUsers, onError);
                        });
                gridApi.pagination.on.paginationChanged($scope,
                    function(newPage, pageSize) {
                        paginationOptions.pageNumber = newPage;
                        paginationOptions.pageSize = pageSize;
                        userService.getUsers(columnName, paginationOptions.sort, filter, filterColumn).then(onGetUsers, onError);
                    });
            }
        };

        $scope.gridOptions.appScopeProvider = $scope;

        userService.getUsers(columnName, paginationOptions.sort,null,null).then(onGetUsers, onError);
    }
]);