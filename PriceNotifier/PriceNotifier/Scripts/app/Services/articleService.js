app.factory("articleService", ['$http', 'toaster',
    function ($http, toaster) {

        var url = '/api/Articles/';

        var getArticles = function (flag, name, order, filter, filterColumn, currentPage, recordsPerPage) {

            var request = url + "?$skip=" + (currentPage - 1) * recordsPerPage + "&$top=" + recordsPerPage;

            if (name && order) {
                request = request + "&$orderby=" + name + " " + order;
            }

            if (filter && filterColumn) {
                request = request + "&$filter=" + "contains(" + filterColumn + "," + '%27' + filter + '%27' + ")";
            }

            request = request + "&showAllArticles=" + flag;

            return $http.get(request + "&$count=true")
                .then(function (response) {
                    return response;
                });
        }

        var getSpecificArticle = function (articleId) {
            return $http.get(url + articleId).then(function (response) {
                return response.data;
            });
        };

        var updateArticle = function (article) {
            return $http.put(url,
                JSON.stringify(article)
            ).success(function () {
                toaster.pop('success', "", "The article was successfully updated.");
            });
        };

        var createArticle = function (article) {
            return $http.post(url,
                JSON.stringify(article)
            ).success(function () {
                toaster.pop('success', "", "The article was successfully created.");
            });
        };

        var removeArticle = function (id) {
            return $http({
                method: 'DELETE',
                url: url + id,
                headers: {
                    'Content-type': 'application/json'
                }
            })
                .success(function () {
                    toaster.pop('success', "Congrats!", "The article was removed successfully.");
                });
        };

        return {
            createArticle: createArticle,
            getSpecificArticle: getSpecificArticle,
            getArticles: getArticles,
            updateArticle: updateArticle,
            removeArticle: removeArticle
        };
    }]);