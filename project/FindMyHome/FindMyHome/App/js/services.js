
/* Services */

angular.module('FindMyHome.services', ['ngResource']).
    factory('SearchFactory', function ($resource, SearchFactoryCache) {
        'use strict';
        return $resource("/api/search/?searchTerms=:searchTerms", {}, {
            get: { method: 'GET', params: { searchTerms: '@searchTerms' }, cache: SearchFactoryCache }
        });
    }).
    factory("VenueSearchFactory", function ($resource) {
        'use strict';
        return $resource("/api/tagSearch/?categories=:searchTerms", {}, {
            get: { method: 'GET', params: { searchTerms: '@searchTerms' } }
        });
    }).
    factory("SeachTermAutoCompleteFactory", function ($resource, $http) {
        'use strict';
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
        'use strict';
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
        'use strict';
        return $cacheFactory("ObjectTypesCache");
    }).
    factory("ObjectTypesFactory", ['$resource', 'ObjectTypesCache', function ($resource, ObjectTypesCache) {
        'use strict';
        return $resource("/api/adObjectTypes", {}, {
            get: { method: 'GET', cache: ObjectTypesCache }
        });
    }]).
    factory("SearchFactoryCache", function ($cacheFactory, $window) {
        'use strict';
        var cacheFactory = $cacheFactory("SearchFactoryCache");
        if (!checkLocalStorage()) {
            return cacheFactory;
        }

        cacheFactory.put = function (key, value) {
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
            var value = $window.localStorage.getItem(key);
            if (value !== null) {
                var obj = JSON.parse(value);

                //Check when next update will be, if a update have been done, make http call
                var timestamp = Date.parse(JSON.parse(obj[1]).AdsContainer.NextUpdate);
                if (timestamp > Date.now())
                    return obj;
                else
                    cacheFactory.remove(key);
            }
            return undefined;
        };

        cacheFactory.remove = function (key) {
            if (key !== null) {
                $window.localStorage.removeItem(key);
            }
        };

         /**
	     * Check localStorage support
	     * @source http://diveintohtml5.info/detect.html#storage
	     * @return {bool}
	     */
        function checkLocalStorage() {
            try {
                return 'localStorage' in $window && $window['localStorage'] !== null;
            } catch(e){
                return false;
            }
        };

        return cacheFactory;
    });