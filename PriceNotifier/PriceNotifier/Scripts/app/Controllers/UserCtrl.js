app.controller('UserCtrl', ['$scope', 'userService', 'uiGridConstants', function ($scope, userService, uiGridConstants) {

    $scope.gridOptions = {
        enableFiltering: true,
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
          name: 'SocialNetworkName', filter: {
              type: uiGridConstants.filter.SELECT,
              selectOptions: [{ value: 'Facebook', label: 'Facebook' }, { value: 'Twitter', label: 'Twitter' }, { value: 'Vkontakte', label: 'Vkontakte' }]
          }
      },
      // no filter input
      {
          displayName: 'Social network UserId',
          name: 'SocialNetworkUserId', enableFiltering: false
      },
      {
          name: ' ',
          cellTemplate: '<button type="button" class="btn btn-danger" ng-click="grid.appScope.remove({row.entity.id})"> Remove user </button>',
          enableFiltering: false,
          enableSorting: false
      }
        ]
    };

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };
    $scope.gridOptions.appScopeProvider = $scope;
    var onGetUsers = function (response) {
        $scope.users = response.data;
        $scope.gridOptions.data = response.data;
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

    userService.getUsers().then(onGetUsers, onError);
}
]);