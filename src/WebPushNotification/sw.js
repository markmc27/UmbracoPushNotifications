self.addEventListener('push', function (event) {
    if (!(self.Notification && self.Notification.permission === 'granted')) {
        return;
    }

    console.log('[Service Worker] Push Received.');
    console.log("[Service Worker] Push had this data: ", event.data);

    var data = {};
    if (event.data) {
        data = event.data.json();
    }

    console.log('Notification Recieved:');
    console.log(data);

    var title = data.title;
    var options = {
        body: data.message,
        icon: 'assets/images/Logo.png',
        badge: 'assets/images/Logo.png',
        tag: 'announcement',
        data: {
            url: data.url
        }
    };

    event.waitUntil(self.registration.showNotification(title, options));
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();

    event.waitUntil(clients.matchAll({ includeUncontrolled: true, type: 'window' }).then(windowClients => {
        var data = event.notification.data;
        for (var i = 0; i < windowClients.length; i++) {
            var client = windowClients[i];
            if (client.url === data.url && 'focus' in client) {
                return client.focus();
            }
        }

        if (clients.openWindow) {
            return clients.openWindow(data.url);
        }

    }));
});