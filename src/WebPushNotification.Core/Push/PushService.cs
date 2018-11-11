using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebPush;
using WebPushNotification.Core.Database;

namespace WebPushNotification.Core.Push
{
    public class PushService : IPushService
    {
        public PushService()
        {
            vapidPublicKey = ConfigurationManager.AppSettings["Push:VapidPublicKey"];
            vapidPrivateKey = ConfigurationManager.AppSettings["Push:VapidPrivateKey"];
        }

        private readonly string vapidPublicKey;
        private readonly string vapidPrivateKey;
        public async Task SendNotificationsAsync(string title, string message, string clickUrl)
        {
            var payload = JsonConvert.SerializeObject(new { title, message, url = clickUrl });

            var vapidDetails = new VapidDetails("mailto:contact@pushnotificationumbraco.com", vapidPublicKey, vapidPrivateKey);
            var webPushClient = new WebPushClient();
            foreach (var subscription in PushSubsciptions.GetSubscriptions())
            {
                var pushSubscription = new PushSubscription(subscription.PushEndpoint, subscription.PushP256DH, subscription.PushAuth);
                try
                {
                    await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                }
                catch (WebPushException e)
                {
                    switch (e.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Gone:
                            PushSubsciptions.RemoveSubscription(subscription);
                            break;
                    }
                }
            }
        }
    }

}
