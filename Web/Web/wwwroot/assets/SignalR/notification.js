var notics = $.connection.notificationHub;

notics.client.newNotification = function (notic) {
    EtsPanel.Notifications.showNotification(notic.Text);
    EtsPanel.Notifications.playNotificationSound();
}

notics.client.updateNotificationsCount = function (count) {
    EtsPanel.Notifications.setNotificationCountsTo(count);
}

notics.client.updateNotificationsList = function (notifications) {
    EtsPanel.Notifications.updateNotificationsList(notifications);
}

notics.client.notificationsHasBeenRead = function() {
    EtsPanel.Notifications.makeNotificationRead();
}

// start the connection
$.connection.hub.start().done(function() {
    SignalRNotification.getAllNotifications();
});

$(".notification-trigger").click(function () {
    SignalRNotification.makeNotificationsRead();
});

SignalRNotification = {
    getAllNotifications : function () {
        notics.server.updateNotifications();
    },
    makeNotificationsRead : function() {
        notics.server.makeNotificationsRead();
    }
}