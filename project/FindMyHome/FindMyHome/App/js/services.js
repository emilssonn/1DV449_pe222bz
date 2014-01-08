
/* Services */

angular.module('FindMyHome.services', ['ngResource']).
    factory('SearchFactory', function ($resource) {
        return $resource("/api/search/?searchTerms=:searchTerms", {}, {
            get: { method: 'GET', params: { searchTerms: '@searchTerms' } }
        });
    }).
    factory("VenueSearchFactory", function ($resource) {
        return $resource("/api/tagSearch/?categories=:searchTerms", {}, {
            get: { method: 'GET', params: { searchTerms: '@searchTerms' } }
        });
    }).
    factory("SeachTermAutoCompleteFactory", function ($resource, $http) {
        return {
            getSearchTermsList: function (terms) {
                var request = '/api/searchTerm/?searchTerms=' + terms;

                return $http.get(request).then(function (response) {
                    return response.data;
                },
                function (response) {
                    //Fail silently
                    return [];
                });
            }
        };
    });