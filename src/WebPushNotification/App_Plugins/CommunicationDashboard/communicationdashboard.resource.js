angular.module('umbraco.resources').factory('communicationResource',
    function ($q, $http, umbRequestHelper) {
        return {
            getHistory: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/Communication/Communication/GetHistory"),
                    "Failed to retrieve communication history");
            },
            getSubscriptionCount: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/Communication/Communication/GetSubscriptionCount"),
                    "Failed to get subscription count");
            },
            addAnnouncement: function (announcement) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/Communication/Communication/NewAnnouncement", announcement),
                    "Failed to create new announcement");
            }
        };
    }
); 