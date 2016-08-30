app.controller('UserCtrl', ['$scope', 'userService', function ($scope, userService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    var onGetUsers = function (response) {
        $scope.users = response.data;
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