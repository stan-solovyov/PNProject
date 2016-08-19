app.factory("tokenService", [
    function () {
        var deleteCookie = function(name) {
            document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
        };

        var setToken = function (token) {
            localStorage.setItem('token', token);
        }

        var logOut = function () {
            localStorage.removeItem('token');
            deleteCookie("Token");
            window.location = "/login";
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