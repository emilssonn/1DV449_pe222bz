
angular.module('FindMyHome', [
    'ngRoute',
    'ngResource',
    'ui.bootstrap',
    'ui.slider',
    'FindMyHome.filters',
    'FindMyHome.services',
    'FindMyHome.directives',
    'FindMyHome.controllers'
]).
config(['$routeProvider', "$httpProvider", '$locationProvider', function ($routeProvider, $httpProvider, $locationProvider) {
    'use strict';
    $httpProvider.defaults.headers.common['RequestVerificationToken'] = $("#antiForgeryToken").val();
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

    $routeProvider.
        when('/search/:searchTerms/:page?', {
            templateUrl: 'app/views/search.html',
            controller: 'SearchCtrl',
            reloadOnSearch: false
        }).
        when('/', {
            templateUrl: 'app/views/search.html',
            controller: 'SearchCtrl'
        }).
        otherwise({ redirectTo: '/' });
}]);