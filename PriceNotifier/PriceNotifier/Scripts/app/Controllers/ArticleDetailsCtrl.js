app.controller('ArticleDetailsCtrl', ['$scope', '$sce', 'currentArticle', function ($scope, $sce, currentArticle) {
    $scope.currentArticle = currentArticle;
    $scope.trustedHtml = $sce.trustAsHtml(currentArticle.Body);
}
]);