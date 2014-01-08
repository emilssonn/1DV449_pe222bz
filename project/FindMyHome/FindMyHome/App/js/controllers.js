

/* Controllers */

angular.module('FindMyHome.controllers', [])
    .controller('VenueSearchCtrl', ['$scope', '$routeParams', '$location', 'VenueSearchFactory',
        function ($scope, $routeParams, $location, SearchFactory) {
            'use strict';

            $scope.search = function (searchTerm) {

            };
        }
    ])
    .controller('SearchCtrl', ['$scope', '$routeParams', '$location', 'SearchFactory', 'SeachTermAutoCompleteFactory',
        function ($scope, $routeParams, $location, SearchFactory, SeachTermAutoCompleteFactory) {
            'use strict';
            $scope.master = {};

            if ($routeParams.searchTerms !== undefined) {
                searchCall();
            }

            var searchCall = function () {
                $scope.master = angular.copy($routeParams.searchTerms);
                $scope.SearchTerms = $routeParams.searchTerms;
                $scope.errors = {};

                SearchFactory.get({ searchTerms: $routeParams.searchTerms }, function (data) {
                    $scope.searchResult = data;
                }, function (response) {
                    angular.forEach(response.data.ModelState, function (errors, field) {
                        $scope.searchForm[field].$setValidity('server', false);
                        $scope.errors[field] = errors.join(', ');
                    });
                });
            };

            $scope.getList = function (term) {
                return SeachTermAutoCompleteFactory.getSearchTermsList(term);
            };

            $scope.search = function (searchTerm) {
                $location.path('/search/' + searchTerm);
            };

            $scope.isUnchanged = function (searchTerm) {
                return angular.equals(searchTerm, $scope.master);
            };

        }
    ]);