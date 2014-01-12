

/* Controllers */

angular.module('FindMyHome.controllers', [])
    .controller('VenueSearchCtrl', ['$scope', '$routeParams', '$location', 'VenueAutoCompleteFactory',
        function ($scope, $routeParams, $location, VenueAutoCompleteFactory) {
            'use strict';

            $scope.getList = function (term) {
                return VenueAutoCompleteFactory.getVenueList(term);
            };

            $scope.removeVenue = function (value) {
                var index = $scope.Search.venues.indexOf(value);
                if (index > -1) {
                    $scope.Search.venues.splice(index, 1);
                }
            }
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
            $scope.Search.venues = []
            $scope.Search.checkedObjectTypes = [];
            $scope.master = angular.copy($scope.Search);

            var searchCall = function () {
                $scope.errors = {};

                var obj = $.extend({}, $routeParams, $location.search());
                obj.maxPrice = parseInt(obj.maxPrice) || 0;
                obj.maxRent = parseInt(obj.maxRent) || 0;

                var searchParams = angular.copy(obj);

                obj.checkedObjectTypes = [];
                obj.venues = obj.venues ? obj.venues.split(',') : [];
                $scope.Search = obj;
                $scope.master = angular.copy(obj);

                searchParams.Offset = 30 * (searchParams.page || 1) - 30;
                searchParams.Limit = 30;
                delete searchParams.page;

                SearchFactory.get(searchParams, function (data) {
                    $scope.searchResult = data;
                    $scope.itemsPerPage = adsPerPage;
                    $scope.totalItems = data.AdsContainer.TotalCount;
                    $scope.maxSize = 5;
                    $scope.currentPage = searchParams.page || 1;

                }, function (response) {
                    if (response.data.Error) {
                        $scope.serverError = response.data.Error;
                    }
                    angular.forEach(response.data.ModelState, function (errors, field) {
                        $scope.searchForm[field].$setValidity('server', false);
                        $scope.errors[field] = errors.join(', ');
                    });
                });
            };

            $scope.clearErrors = function () {
                $scope.serverError = undefined;
            };

            $scope.getList = function (term) {
                return SeachTermAutoCompleteFactory.getSearchTermsList(term);
            };

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

            $scope.setPage = function (pageNo) {
                $location.search('page', pageNo);
                searchCall();
            };

            $scope.isUnchanged = function (search) {
                return angular.equals(getCleanObject(search), getCleanObject($scope.master));
            };

            var getCleanObject = function (object) {
                var newObject = {};
                angular.forEach(object, function (value, prop) {
                    if (value) {
                        newObject[prop] = value;
                    }
                });
                return newObject;
            };

            if ($routeParams.searchTerms !== undefined) {
                searchCall();
            }
        }
    ])
    .controller('ObjectTypesCtrl', ['$scope', '$routeParams', 'ObjectTypesFactory',
        function ($scope, $routeParams, ObjectTypesFactory) {
            'use strict';
            ObjectTypesFactory.get(function (data) {
                $scope.objectTypes = data.ObjectTypes;
                var objectTypesArray = $routeParams.objectTypes ? $routeParams.objectTypes.split(',') : [];
                angular.forEach(objectTypesArray, function (value) {
                    if (data.ObjectTypes.indexOf(value) !== -1)
                        addObjectType(value);
                });
            });

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
            LastSearchesFactory.get(function (data) {
                angular.forEach(data.Searches, function (value) {
                    SearchesFactory.add('/search/' + value);
                });
            });
            $scope.searches = SearchesFactory.searches;
        }
    ]);


