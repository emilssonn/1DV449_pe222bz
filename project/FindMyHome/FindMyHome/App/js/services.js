
/* Services */

angular.module('FindMyHome.services', ['ngResource']).
    factory('SearchFactory', function ($resource, SearchFactoryCache) {
        return $resource("/api/search/?searchTerms=:searchTerms", {}, {
            get: { method: 'GET', params: { searchTerms: '@searchTerms' }, cache: SearchFactoryCache }
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
    }).
    factory("VenueAutoCompleteFactory", function ($resource, $http) {
        return {
            getVenueList: function (term) {
                var request = '/api/venueTerm/?VenueTerm=' + term;

                return $http.get(request).then(function (response) {
                    return response.data;
                },
                function (response) {
                    //Fail silently
                    return [];
                });
            }
        };
    }).
    factory("ObjectTypesCache", function ($cacheFactory) {
        return $cacheFactory("ObjectTypesCache");
    }).
    factory("ObjectTypesFactory", ['$resource', 'ObjectTypesCache', function ($resource, ObjectTypesCache) {
        return $resource("/api/adObjectTypes", {}, {
            get: { method: 'GET', cache: ObjectTypesCache }
        });
    }]).
    factory("SearchFactoryCache", function ($cacheFactory, $window) {
        var cacheFactory = $cacheFactory("SearchFactoryCache");
        //ÄNDRA, använda nextupdate datum för o kolla om den ska uppdateras.


        cacheFactory.put = function (key, value) {
            if (!checkLocalStorage()) {
                return null;
            }
            if (value !== null && typeof value === "object") {
                value = JSON.stringify(value);
            }
            try {
                $window.localStorage.setItem(key, value);
            } catch (e) {
                //Empty the entire cache, not optimal
                $window.localStorage.clear();
                $window.localStorage.setItem(key, value);
            }
            //for (var x in localStorage) console.log(x + "=" + ((localStorage[x].length * 2) / 1024 / 1024).toFixed(2) + " MB");
        };

        cacheFactory.get = function (key) {
            if (!checkLocalStorage()) {
                return undefined;
            }
            var value = $window.localStorage.getItem(key);
            if (value !== null) {
                return JSON.parse(value);
            }
            return undefined;
        };

        cacheFactory.remove = function (key) {
            if (!checkLocalStorage()) {
                return;
            }
            if (key !== null) {
                $window.localStorage.removeItem(key);
            }
        };

         /**
	     * Check localStorage support
	     * @source http://diveintohtml5.info/detect.html#storage
	     * @return {bool}
	     */
        var checkLocalStorage = function() {
            try {
                return 'localStorage' in $window && $window['localStorage'] !== null;
            } catch(e){
                return false;
            }
        };

        return cacheFactory;
    });