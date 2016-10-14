app.factory("priceChangeService", ['$http',
    function ($http) {

        var url = '/api/PriceHistories/';

        var getPriceChangesPage = function (productId, provider, currentPage, recordsPerPage) {
            var request = url + productId + "/" + provider + "?$skip=" + (currentPage - 1) * recordsPerPage + "&$top=" + recordsPerPage + "&$orderby=" + "Date" + " " + "desc";

            return $http.get(request + "&$count=true")
                 .then(function (response) {
                     return response;
                 });
        };

        return {
            getPriceChangesPage: getPriceChangesPage
        };
    }]);