app.controller('ArticleCtrlForUser',
[
    '$scope','articleService', 'pagerService',
    function ($scope, articleService, pagerService) {

        $scope.allArticles = true;

        var pageSize = 20;
        var currentPage = 1;

        var onError = function () {
            $scope.error = "Couldn't get response from the server:(";
        };

        var onGetArticles = function (response) {
            $scope.pager = pagerService.getPager(response.data.Count, currentPage, pageSize);
            $scope.totalPages = Math.ceil(response.data.Count / pageSize);
            $scope.articles = response.data.Items;
            if ($scope.articles.length === 0 && response.status !== 204) {
                $scope.Message = "There are no articles.";
                $scope.demonstrate = true;
            } else {
                $scope.Message = null;
                $scope.demonstrate = false;
            }
        };

        $scope.setPage = setPage;

        function setPage(page) {
            if (page < 1 || page > $scope.totalPages) {
                return;
            }
            currentPage = page;
            if ($scope.allArticles) {
                articleService.getArticles(true, "DateAdded", "desc", null, null, page, pageSize).then(onGetArticles, onError);
            } else {
                articleService.getArticles(false, "DateAdded", "desc", null, null, page, pageSize).then(onGetArticles, onError);
            }
            
        }

        $scope.update = function () {
            setPage(1);
            if ($scope.allArticles === false) {
                articleService.getArticles(false, "DateAdded", "desc", null, null, currentPage, pageSize).then(onGetArticles, onError);
            }

            if ($scope.allArticles === true) {
                articleService.getArticles(true, "DateAdded", "desc", null, null, currentPage, pageSize).then(onGetArticles, onError);
            }

        };

        articleService.getArticles(true,"DateAdded","desc",null,null,currentPage, pageSize).then(onGetArticles, onError);

    }
]);
