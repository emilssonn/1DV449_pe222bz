
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
config(['$routeProvider', "$httpProvider", '$locationProvider', '$provide', function ($routeProvider, $httpProvider, $locationProvider, $provide) {
    'use strict';
    $httpProvider.defaults.headers.common['RequestVerificationToken'] = $("#antiForgeryToken").val();
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

    ////Used when live on Lnu server, root is not root, hack, not good
    //$provide.factory('HttpInterceptorLnu', ['$q', function ($q) {
    //    return {
    //        request: function (config) {
    //            if (config.url.contains('api') && !config.url.contains('cache')) {
    //                config.url = "/1dv409/pe222bz" + config.url;
    //            }
    //            if (config.url.contains('app')) {
    //                config.url = "/1dv409/pe222bz/" + config.url;
    //            }
    //            return config || $q.when(config);
    //        }
    //    };
    //}]);

    //$httpProvider.interceptors.push('HttpInterceptorLnu');

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