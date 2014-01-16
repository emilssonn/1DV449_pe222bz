
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
    //Bad, should only use dom functions in a directive
    $httpProvider.defaults.headers.common['RequestVerificationToken'] = $("#antiForgeryToken").val();
    $("#antiForgeryToken").remove();
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';

    ////Used when live on Lnu server, root is not root, hack, not good
    //$provide.factory('HttpInterceptorLnu', ['$q', function ($q) {
    //    return {
    //        request: function (config) {
    //            if (config.url.indexOf('api') !== -1 && config.url.indexOf('cache') === -1) {
    //                config.url = "/1dv409/pe222bz" + config.url;
    //            }
    //            if (config.url.indexOf('app') !== -1) {
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