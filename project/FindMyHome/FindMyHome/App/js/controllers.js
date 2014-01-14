
/* Controllers */

angular.module('FindMyHome.controllers', [])
    .controller('VenueSearchCtrl', ['$scope', '$routeParams', '$location', 'VenueAutoCompleteFactory',
        function ($scope, $routeParams, $location, VenueAutoCompleteFactory) {
            'use strict';

            //Autocomplete
            $scope.getList = function (term) {
                return VenueAutoCompleteFactory.getVenueList(term);
            };

            $scope.removeVenue = function (value) {
                var index = $scope.Search.venues.indexOf(value);
                if (index > -1) {
                    $scope.Search.venues.splice(index, 1);
                }
            };
        }
    ])
    .controller('SearchCtrl', ['$scope', '$routeParams', '$location', '$route', 'SearchFactory', 'SeachTermAutoCompleteFactory', '$route', 'SearchesFactory',
        function ($scope, $routeParams, $location, $route, SearchFactory, SeachTermAutoCompleteFactory, ObjectTypesFactory, SearchesFactory) {
            'use strict';

            var adsPerPage = 30,
                offset = 0,
                limit = 0;

            //Init values
            $scope.Search = {};
            $scope.Search.venues = [];
            $scope.Search.checkedObjectTypes = [];
            $scope.master = angular.copy($scope.Search);

            //Do a search
            var searchCall = function () {
                $scope.errors = {};

                //Get the search params
                var obj = $.extend({}, $routeParams, $location.search());
                obj.maxPrice = parseInt(obj.maxPrice, 10) || 0;
                obj.maxRent = parseInt(obj.maxRent, 10) || 0;

                var searchParams = angular.copy(obj);

                obj.checkedObjectTypes = [];
                obj.venues = obj.venues ? obj.venues.split(',') : [];
                //Set form and form master values for validation
                $scope.Search = obj;
                $scope.master = angular.copy(obj);

                searchParams.Offset = 30 * (searchParams.page || 1) - 30;
                searchParams.Limit = 30;
                $scope.currentPage = searchParams.page || 1;
                delete searchParams.page;

                //Search call
                SearchFactory.get(searchParams, function (data) {
                    $scope.searchResult = data;
                    $scope.itemsPerPage = adsPerPage;
                    $scope.totalItems = data.AdsContainer.TotalCount;
                    $scope.maxSize = 5;
                }, function (response) {
                    if (response.data.Error) {
                        $scope.serverError = response.data.Error;
                    }
                    angular.forEach(response.data.ModelState, function (errors, field) {
                        if (field === 'ObjectTypes') {
                            $scope.serverError = errors.join(', ');
                            return;
                        }
                        $scope.searchForm[field].$setValidity('server', false);
                        $scope.errors[field] = errors.join(', ');
                    });
                });
            };

            //Clears all none field bound server errors
            $scope.clearErrors = function () {
                $scope.serverError = undefined;
            };

            //Autocomplete
            $scope.getList = function (term) {
                return SeachTermAutoCompleteFactory.getSearchTermsList(term);
            };

            //Get the search params, set the url, reload the route
            $scope.doSearch = function (search) {
                var newObject = angular.copy(search);
                var path = '/search/' + newObject.searchTerms;
                delete newObject.searchTerms;
                if (newObject.checkedObjectTypes) {
                    newObject.objectTypes = newObject.checkedObjectTypes.join(",");
                    delete newObject.checkedObjectTypes;
                }

                if (newObject.venues) {
                    newObject.venues = newObject.venues.join(",");
                }
                $location.path(path).search(getCleanObject(newObject));
                SearchesFactory.add($location.url());
                $route.reload();
            };

            //Change page
            $scope.setPage = function (pageNo) {
                $location.search('page', pageNo);
                searchCall();
            };

            //Check if the model has changed
            $scope.isUnchanged = function (search) {
                return angular.equals(getCleanObject(search), getCleanObject($scope.master));
            };

            //Gets the a clean object, all properties without a value is removed
            var getCleanObject = function (object) {
                var newObject = {};
                angular.forEach(object, function (value, prop) {
                    if (value) {
                        newObject[prop] = value;
                    }
                });
                return newObject;
            };

            //On page load, check if a search should be made
            if ($routeParams.searchTerms !== undefined) {
                searchCall();
            }
        }
    ])
    .controller('ObjectTypesCtrl', ['$scope', '$routeParams', 'ObjectTypesFactory',
        function ($scope, $routeParams, ObjectTypesFactory) {
            'use strict';

            //Get the ad object types
            ObjectTypesFactory.get(function (data) {
                $scope.objectTypes = data.ObjectTypes;
                //See if any object types should be checked
                var objectTypesArray = $routeParams.objectTypes ? $routeParams.objectTypes.split(',') : [];
                angular.forEach(objectTypesArray, function (value) {
                    if (data.ObjectTypes.indexOf(value) !== -1)
                        addObjectType(value);
                });
            });

            //Check a objecttype
            function addObjectType(obj) {
                if ($scope.Search.checkedObjectTypes.indexOf(obj) !== -1)
                    return;
                $scope.Search.checkedObjectTypes.push(obj);
                $scope.master.checkedObjectTypes.push(obj);
            }
        }
    ])
    .controller('LastSearchesCtrl', ['$scope', 'SearchesFactory', 'LastSearchesFactory',
        function ($scope, SearchesFactory, LastSearchesFactory) {
            //Get the last searches made
            LastSearchesFactory.get(function (data) {
                angular.forEach(data.Searches, function (value) {
                    SearchesFactory.add('/search/' + value);
                });
            });
            $scope.searches = SearchesFactory.searches;
            
        }
    ]);


