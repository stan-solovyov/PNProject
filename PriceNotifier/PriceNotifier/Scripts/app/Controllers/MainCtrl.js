
(function() {
    app.controller('MainCtrl',['$scope', '$http', function($scope, $http) {

            var onError = function() {
                $scope.error = "Couldn't get response from the server:(";
            };

            var onUserComplete = function(response) {
                $scope.values = response;
                $scope.message = "You'are authorized!";
            };

            $scope.Logout = function() {
                localStorage.removeItem('X-Auth');
                window.location = "/";
            };

            var value = localStorage.getItem('X-Auth');
            $scope.Authorize = function() {
                $http({
                        method: 'GET',
                        url: "/api/Values",
                        headers: { 'X-Auth': value }
                    })
                    .success(onUserComplete)
                    .error(onError);
            };
        }
    ]);
}());