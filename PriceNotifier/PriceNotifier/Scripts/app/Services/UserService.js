app.factory("userService", ['$http', 'toaster',
    function ($http, toaster) {

        var url = '/api/Users';

        var getUsers = function () {
            return $http.get(url)
                .then(function (response) {
                    return response;
                });
        }

        var updateUser = function (user) {
            return $http.put('/api/Users/',
                JSON.stringify(user)
            ).success(function () {
                toaster.pop('success', "", "The user profile was successfully updated.");
            });
        };

        var removeUser = function (id) {
            return $http({
                method: 'DELETE',
                url: '/api/Users/' + id,
                headers: {
                    'Content-type': 'application/json'
                }
            })
                .success(function () {
                    toaster.pop('success', "Congrats!", "The user profile was removed successfully.");
                });
        };

        return {
            getUsers: getUsers,
            updateUser: updateUser,
            removeUser: removeUser
        };
    }]);