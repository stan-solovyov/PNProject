app.factory("tokenService",
[
    $scope, $http, function ($scope, $http) {

        var getToken = function() {
            var value = localStorage.getItem('X-Auth');
            return $http({
                method: 'GET',
                url: "/api/Values",
                headers: { 'X-Auth': value }
            });
        };
    
        var getRepos = function(user){
            return $http.get(user.repos_url)
                    .then(function(response){
                        return response.data;
                    });
        };
    
        return {
            getToken: getToken,
            getRepos:getRepos
      
        };
    }
]);