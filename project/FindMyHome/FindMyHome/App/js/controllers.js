

/* Controllers */

angular.module('FindMyHome.controllers', [])
    .controller('VenueSearchCtrl', ['$scope', '$routeParams', '$location', 'VenueSearchFactory',
        function ($scope, $routeParams, $location, SearchFactory) {
            'use strict';

            $scope.search = function (searchTerm) {

            };
        }
    ])
    .controller('SearchCtrl', ['$scope', '$routeParams', '$location', 'SearchFactory', 'SeachTermAutoCompleteFactory', '$route',
        function ($scope, $routeParams, $location, SearchFactory, SeachTermAutoCompleteFactory, $route) {
            'use strict';
            $scope.master = {};

            if ($routeParams.searchTerms !== undefined) {
                searchCall(true);
            }

            var adsPerPage = 30;
            var offset = 0;
            var limit = 0;

            function searchCall(full) {
                $scope.errors = {};

                if (full) {
                    var newObject = $.extend({}, $routeParams);
                    delete newObject.page;
                    $scope.master = angular.copy(newObject);
                    $scope.Search = newObject;
                }

                var newObject = $.extend({}, $routeParams, $location.search());
                newObject.Offset = 30 * (newObject.page || 1) - 30;
                newObject.Limit = 30;
                var page = newObject.page;
                delete newObject.page;
                SearchFactory.get(newObject, function (data) {
                    $scope.searchResult = data;
                    $scope.itemsPerPage = adsPerPage;
                    $scope.totalItems = data.AdsContainer.TotalCount;
                    $scope.maxSize = 5;
                    $scope.currentPage = page || 1;
                }, function (response) {
                    angular.forEach(response.data.ModelState, function (errors, field) {
                        $scope.searchForm[field].$setValidity('server', false);
                        $scope.errors[field] = errors.join(', ');
                    });
                });
            };

            /// <summary>
            /// Copies an object, excluding all properties that do not have a value.
            /// </summary>
            /// <param name="object"></param>
            /// <returns type="object"></returns>
            function getCleanObject(object) {
                var newObject = {};
                angular.forEach(object, function (value, prop) {
                    if (value && value !== "") {
                        newObject[prop] = value;
                    }
                });
                return newObject;
            };

            $scope.getList = function (term) {
                return SeachTermAutoCompleteFactory.getSearchTermsList(term);
            };

            $scope.search = function (search) {
                var newObject = $.extend({}, search);
                var path = '/search/' + newObject.searchTerms;
                delete newObject.searchTerms;
                $location.path(path).search(getCleanObject(newObject));
                $route.reload();
            };
            
            $scope.setPage = function (pageNo) {
                $location.search('page', pageNo);
                searchCall();
            };

            $scope.isUnchanged = function (search) {
                return angular.equals(getCleanObject(search), $scope.master);
            };
        }
    ]);