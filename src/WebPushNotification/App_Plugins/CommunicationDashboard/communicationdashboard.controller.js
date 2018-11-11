angular.module("umbraco").controller("CommunicationDashboardController", function ($scope, appState, notificationsService, communicationResource) {
    var vm = this;
    vm.historyLog = [];
    vm.loaded = false;
    vm.addNewButtonState = "init";
    vm.toggleDrawer = toggleDrawer;
    vm.openOverlay = openOverlay;
    vm.subscriptionCount = "loading...";

    communicationResource.getHistory().then(function (response) {
        vm.historyLog = response;
        console.log(response);
        vm.loaded = true;
    });

    communicationResource.getSubscriptionCount().then(function (response) {
        vm.subscriptionCount = response;
    });

    function addAnnouncment(announcement) {
        console.log(announcement);
        var request = {
            shortName: announcement.shortName,
            heading: announcement.heading,
            text: announcement.announcementText,
            types: []
        };

        for (type in announcement.types) {
            var typeIsChecked = announcement.types[type].checked;
            if (typeIsChecked) {
                request.types.push(type);
            }
        }

        communicationResource.addAnnouncement(request).then(function (response) {
            vm.addNewButtonState = "success";
            notificationsService.success("Announcement", "Email and/or Push announcements sent successfully!");
            debugger;

            if (request.types.indexOf("Facebook") > -1) {
                FB.api('/604843296202940/feed', 'post', {
                    access_token: 'EAADkqUgFstkBABwPLf4NGnUfG8cIDildLgbChjg53ZB0ouBxdV8ljEZBFz7J8h4ZAKsqZCepaBfYla8LbgaWinY0iZCLNFxx7ui271GbGG3ZCbZBvBDCc1reliG0cZBL1ZCIEJAce2iey9prLpZA8wkeWO8EBujJnrdOEZAWgU9h7mZBZCjNw9UEv8dRXd2cKevtoTaKW7BIkIsn8AGbfIZBuxqKPz',
                    message: request.text,
                    link: response.url
                }, function (response) {
                    if (!response || response.error) {
                        notificationsService.success("Announcement", "Facebook announcement failed!");
                    } else {
                        notificationsService.success("Announcement", "Facebook announcement sent successfully!");
                    }
                });
            }

            communicationResource.getHistory().then(function (response) {
                vm.historyLog = response;
                console.log(response);
            });
        });
    };

    function toggleDrawer() {
        var showDrawer = appState.getDrawerState("showDrawer");

        var model = {
            firstName: "Super",
            lastName: "Man"
        };

        appState.setDrawerState("view", "/App_Plugins/CommunicationDashboard/drawer.html");
        appState.setDrawerState("model", model);
        appState.setDrawerState("showDrawer", !showDrawer);
    };

    function openOverlay() {
        vm.overlay = {
            view: "/App_Plugins/CommunicationDashboard/addAnnoncement.html",
            title: 'Add new announcement',
            submitButtonLabel: 'Add announcement',
            show: true,
            announcement: {
                shortName: '',
                heading: '',
                announcementText: '',
                types: {
                    'Email': {
                        type: 'Email',
                        checked: false
                    },
                    'Facebook': {
                        type: 'Facebook',
                        checked: false
                    },
                    'Push': {
                        type: 'Notification',
                        checked: false,
                        tooltip: {
                            show: false, 
                            event: null,
                            mouseOver: function (event) {
                                this.show = true;
                                this.event = event;
                            },
                            mouseLeave: function (event) {
                                this.show = false;
                                this.event = null;
                            },
                        }
                    }
                },
                toggleAnnouncementType: function (type) {
                    type.checked = !type.checked;
                }
            },
            submit: function (model) {
                vm.addNewButtonState = "busy";
                addAnnouncment(model.announcement);
                vm.overlay.show = false;
                vm.overlay = null;
            },
            close: function (oldModel) {
                vm.overlay.show = false;
                vm.overlay = null;
            }
        }
    };
});