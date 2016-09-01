app.controller('UserCtrl',
[
    '$scope', 'userService', 'uiGridConstants', function($scope, userService, uiGridConstants) {

        var paginationOptions = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        var onError = function () {
            $scope.error = "Couldn't get response from the server:(";
        };

        var onGetUsers = function (response) {
            $scope.users = response.data;
            $scope.gridOptions.totalItems = response.data.length;
            var firstRow = (paginationOptions.pageNumber - 1) * paginationOptions.pageSize;
            $scope.gridOptions.data = response.data.slice(firstRow, firstRow + paginationOptions.pageSize);
            if ($scope.users.length === 0 && response.status !== 204) {
                $scope.Message = "You don't have any items in the list yet.";
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

        var getPage = function() {
            userService.getUsers().then(onGetUsers, onError);
        }


        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
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
                    filter: {
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [
                            { value: 'Facebook', label: 'Facebook' }, { value: 'Twitter', label: 'Twitter' },
                            { value: 'Vkontakte', label: 'Vkontakte' }
                        ]
                    }
                },
                // no filter input
                {
                    displayName: 'Social network UserId',
                    name: 'SocialNetworkUserId',
                    enableFiltering: false
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
                        }

                        getPage();
                    });
                gridApi.pagination.on.paginationChanged($scope,
                    function(newPage, pageSize) {
                        paginationOptions.pageNumber = newPage;
                        paginationOptions.pageSize = pageSize;
                        getPage();
                    });
            }
        };

        $scope.gridOptions.appScopeProvider = $scope;

        userService.getUsers().then(onGetUsers, onError);
    }
]);