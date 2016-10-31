app.controller('ArticleDetailsCtrl', ['$scope', '$sce', '$uibModal', 'currentArticle', 'productService', 'articleService', function ($scope, $sce, $uibModal, currentArticle, productService, articleService) {
    $scope.currentArticle = currentArticle;
    $scope.trustedHtml = $sce.trustAsHtml(currentArticle.Body);

    //Datepicker
    $scope.today = function () {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.inlineOptions = {
        showWeeks: true
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    $scope.open1 = function () {
        $scope.popup1.opened = true;
    };

    $scope.setDate = function (year, month, day) {
        $scope.dt = new Date(year, month, day);
    };

    $scope.formats = ['dd-MMMM-yyyy', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];
    $scope.altInputFormats = ['M!/d!/yyyy'];

    $scope.popup1 = {
        opened: false
    };

    var onArticleDelete = function () {
        $scope.validationMessages = null;
    };

    var onErrorValidate = function (response) {
        $scope.validationMessages = parseErrors(response);
    };

    function parseErrors(response) {
        var errors = [];
        for (var key in response.data.ModelState) {
            if (response.data.ModelState.hasOwnProperty(key)) {
                for (var i = 0; i < response.data.ModelState[key].length; i++) {
                    errors.push(response.data.ModelState[key][i]);
                }
            }
        }
        return errors;
    }

    $scope.update = function (article) {
        if (typeof (article.ProductId.Id) == "undefined") {
            articleService.updateArticle(article).then(onArticleDelete, onErrorValidate);
        } else {
            article.ProductId = article.ProductId.Id;
            articleService.updateArticle(article).then(onArticleDelete, onErrorValidate);
        }
    };

    $scope.edit = function (article) {
        productService.getProducts().then(function (response) {
            $scope.items = response.data.Items;
        });

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/scripts/app/Views/articleEdit.html',
            controller: ModalInstanceCtrl,
            scope: $scope,
            backdrop: 'static',
            size: 'lg',
            resolve: {
                article: article
            }
        });

        function ModalInstanceCtrl($uibModalInstance, article) {
            article.DateAdded = new Date(article.DateAdded);
            $scope.article = article;
            $scope.ok = function () {
                $scope.validationMessages = null;
                $uibModalInstance.close();
            };
        };
    };
}
]);