
/* Services */

angular.module('FindMyHome.services', ['ngResource']).
    //Do a full search, look for cached search
    factory('SearchFactory', ['$resource', 'SearchFactoryCache', function ($resource, SearchFactoryCache) {
        'use strict';
        return $resource("/api/search/?searchTerms=:searchTerms", {}, {
            get: { method: 'GET', params: { searchTerms: '@searchTerms' }, cache: SearchFactoryCache }
        });
    }]).
    //Search term autocomplete, not cached
    factory("SeachTermAutoCompleteFactory", ['$resource', '$http', function ($resource, $http) {
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
    }]).
    //Venue search autocomplete
    factory("VenueAutoCompleteFactory", ['$resource', '$http', function ($resource, $http) {
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
    }]).
    //Cache for preloaded objecttypes, cached in memory
    factory("ObjectTypesCache", ['$cacheFactory', function ($cacheFactory) {
        'use strict';
        return $cacheFactory("ObjectTypesCache");
    }]).
    //Get the preloaded object types, will not cause a HTTP call
    factory("ObjectTypesFactory", ['$resource', 'ObjectTypesCache', function ($resource, ObjectTypesCache) {
        'use strict';
        return $resource("/cache/api/adObjectTypes", {}, {
            get: { method: 'GET', cache: ObjectTypesCache }
        });
    }]).
    //Cache for preloaded last searches, cached in memory
    factory("LastSearchesCache", ['$cacheFactory', function ($cacheFactory) {
        'use strict';
        return $cacheFactory("LastSearchesCache");
    }]).
    //Get the preloaded last searches, will not cause a HTTP call
    factory("LastSearchesFactory", ['$resource', 'LastSearchesCache', function ($resource, LastSearchesCache) {
        'use strict';
        return $resource("/cache/api/lastSearches", {}, {
            get: { method: 'GET', cache: LastSearchesCache }
        });
    }]).
    //Cache for searches
    //This cache uses localstorage to store the data returned from the server
    //Data is stored with the url as key
    //All searches made will first look here and see if the search is already cached
    //Will only save successfull calls
    //Each time a search is found in cache it will be checked so that its valid.
    //Valid = the NextUpdate time has not passed
    factory("SearchFactoryCache", ['$cacheFactory', '$window', function ($cacheFactory, $window) {
        'use strict';
        var cacheFactory = $cacheFactory("SearchFactoryCache");
        if (!checkLocalStorage()) {
            return cacheFactory;
        }

        cacheFactory.put = function (key, value) {
            //Only store successfull calls
            if (value && value[0] === 200) {
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
            }
        };

        cacheFactory.get = function (key) {
            var value = $window.localStorage.getItem(key);
            if (value !== null) {
                var obj = JSON.parse(value);

                //Check when next update will be, if a update has been done, make http call
                var timestamp = Date.parse(JSON.parse(obj[1]).AdsContainer.NextUpdate);
                if (timestamp > Date.now()) {
                    return obj;
                }
                else {
                    cacheFactory.remove(key);
                }
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
            } catch (e) {
                return false;
            }
        }

        return cacheFactory;
    }]).
    //Store the last searches made by a logged in user.
    //No duplicates are allowed
    //Makes the last searches available to all parts of the application to watch for changes
    factory('SearchesFactory', function () {
        'use strict';

        var searches = [];

        var add = function (value) {
            var index = searches.indexOf(decodeURI(value));
            if (index > -1) {
                searches.splice(index, 1);
            } else if (searches.length == 10) {
                searches.shift();
            }
            searches.push(decodeURI(value));
        };

        return {
            searches: searches,
            add: add
        };
    });