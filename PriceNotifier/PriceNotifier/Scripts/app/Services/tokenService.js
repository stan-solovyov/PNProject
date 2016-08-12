app.factory("tokenService", [
    function () {

        var setToken = function (token) {
            localStorage.setItem('token', token);
        }

        var logOut = function () {
            localStorage.removeItem('token');
            window.location = "/";
        };

        var getToken = function () {
            return localStorage.getItem('token');
        }

        return {
            logout: logOut,
            setToken: setToken,
            getToken: getToken
        };
    }]);