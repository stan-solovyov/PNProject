app.factory("userService", ['$http', 'toaster',
    function ($http, toaster) {

        var url = '/api/Users/';

        var getUsers = function (name, order, filter, filterColumn, currentPage, recordsPerPage) {

            var request = url + "?$skip=" + (currentPage - 1) * recordsPerPage + "&$top=" + recordsPerPage;

            if (name && order) {
                request = request + "&$orderby=" + name + " " + order;
            }

            if (filter && filterColumn) {
                request = request + "&$filter=" + "contains(" + filterColumn + "," + '%27' + filter + '%27' + ")";
            }

            return $http.get(request + "&$count=true")
                .then(function (response) {
                    return response;
                });
        }

        var updateUser = function (user) {
            return $http.put(url,
                JSON.stringify(user)
            ).success(function () {
                toaster.pop('success', "", "The user profile was successfully updated.");
            });
        };

        var removeUser = function (id) {
            return $http({
                method: 'DELETE',
                url: url + id,
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