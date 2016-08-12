
app.controller('MainCtrl', ['$scope', 'tokenService', 'valueService', function ($scope, tokenService, valueService) {

    var onError = function () {
        $scope.error = "Couldn't get response from the server:(";
    };

    var onUserComplete = function (data) {
        $scope.values = data;
        $scope.message = "You'are authorized!";
    };

    $scope.Logout = function () {
        tokenService.logout();
    };

    $scope.Authorize = function () {
        valueService.getValues().then(onUserComplete, onError);
    };
}
]);
