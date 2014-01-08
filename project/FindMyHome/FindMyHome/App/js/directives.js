
/* Directives */

angular.module('FindMyHome.directives', []).
    directive('requiredInput', function () {
        'use strict';
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$parsers.unshift(function (viewValue) {
                    if (viewValue.length > 0) {
                        // it is valid
                        ctrl.$setValidity('requiredInput', true);
                        return viewValue;
                    } else {
                        // it is invalid, return undefined (no model update)
                        ctrl.$setValidity('requiredInput', false);
                        return undefined;
                    }
                });
            }
        };
    }).
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
    });