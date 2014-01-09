﻿
/* Directives */

angular.module('FindMyHome.directives', []).
    directive('serverError', function() {
        'use strict';
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ctrl) {
                return element.on('change keyup paste', function () {
                    return scope.$apply(function () {
                        return ctrl.$setValidity('server', true);
                    });
                });
            }
        };
    }).
    directive('input', function () {
        /*
         * Soruce: http://blog.jdriven.com/2013/09/how-angularjs-directives-renders-model-value-and-parses-user-input/
         */
        return {
            require: '?ngModel',
            restrict: 'E',
            link: function ($scope, $element, $attrs, ngModelController) {
                var inputType = angular.lowercase($attrs.type);

                if (!ngModelController || inputType === 'radio' ||
                        inputType === 'checkbox') {
                    return;
                }

                ngModelController.$formatters.unshift(function (value) {
                    if (ngModelController.$invalid && angular.isUndefined(value)
                            && typeof ngModelController.$modelValue === 'string') {
                        return ngModelController.$modelValue;
                    } else {
                        return value;
                    }
                });
            }
        };
    }).
    directive("preloadObjectTypes", ["ObjectTypesCache",
        function (objectTypesCache) {
            return {
                link: function (scope, element, attrs) {
                    objectTypesCache.put(attrs.preloadObjectTypes, element.html());
                    element.remove();
                }
            };
        }
    ]).
    directive('checkList', function() {
        return {
            scope: {
                list: '=checkList',
                value: '@'
            },
            link: function(scope, elem, attrs) {
                var handler = function(setup) {
                    var checked = elem.prop('checked');
                    var index = scope.list.indexOf(scope.value);

                    if (checked && index === -1) {
                        if (setup)
                            elem.prop('checked', false);
                        else
                            scope.list.push(scope.value);
                    } else if (!checked && index !== -1) {
                        if (setup)
                            elem.prop('checked', true);
                        else
                            scope.list.splice(index, 1);
                    }
                };
      
                var setupHandler = handler.bind(null, true);
                var changeHandler = handler.bind(null, false);
            
                elem.on('change', function() {
                    scope.$apply(changeHandler);
                });
                scope.$watch('list', setupHandler, true); 
            }
        };
    });