app.factory("exchangeRateService", ['$http', 
    function ($http) {

        var url = 'https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.xchange%20where%20pair%20in%20(%22USDBYN%22%2C%22EURBYN%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys';
        var getExchangeRates = function () {

            var request = url;

            return $http.get(request,
                {
                    'dataType': 'jsonp'
                })
                .then(function (response) {
                    return response;
                });
        }
        return {
            getExchangeRates: getExchangeRates
        };
    }]);