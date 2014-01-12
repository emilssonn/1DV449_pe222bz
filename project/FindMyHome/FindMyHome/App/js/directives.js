
/* Directives */

angular.module('FindMyHome.directives', []).
    directive('serverError', function () {
        'use strict';
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ctrl) {
                scope.$watch(attrs.ngModel, function (value) {
                    ctrl.$setValidity('server', true);
                });
                /*
                return element.on('change keyup paste', function () {
                    return scope.$apply(function () {
                        return ctrl.$setValidity('server', true);
                    });
                });*/
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
            'use strict';
            return {
                link: function (scope, element, attrs) {
                    objectTypesCache.put(attrs.preloadObjectTypes, element.html());
                    element.remove();
                }
            };
        }
    ]).
    directive("preloadLastSearches", ["LastSearchesCache",
        function (LastSearchesCache) {
            'use strict';
            return {
                link: function (scope, element, attrs) {
                    LastSearchesCache.put(attrs.preloadLastSearches, element.html());
                    element.remove();
                }
            };
        }
    ]).
    directive('checkList', function () {
        'use strict';
        return {
            scope: {
                list: '=checkList',
                value: '@'
            },
            link: function (scope, elem, attrs) {
                var handler = function (setup) {
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

                elem.on('change', function () {
                    scope.$apply(changeHandler);
                });
                scope.$watch('list', setupHandler, true);
            }
        };
    }).
    directive('venueCategoriesTags', function () {
        'use strict';
        return {
            restrict: 'A',
            scope: {
                venues: '=venueCategoriesTags',
                value: '=ngModel'
            },
            link: function (scope, element, attrs) {
                element.on('keypress', function (event) {
                    if (event.keyCode === 13) {
                        if (scope.value) {
                            var index = scope.venues.indexOf(scope.value);

                            if (index === -1) {
                                scope.venues.push(scope.value);
                                scope.value = "";
                                scope.$apply(scope.venues);
                            }
                        }
                    }
                });
            }
        };
    });