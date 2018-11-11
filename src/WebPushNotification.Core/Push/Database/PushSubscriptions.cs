using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using WebPushNotification.Core.Database.Pocos;

namespace WebPushNotification.Core.Database
{
    public static class PushSubsciptions
    {
        private static readonly string TABLE_NAME = "PushSubscriptions";
        public static PushSubscription CreateSubscription(string endpoint, string p256dh, string pushAuth)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;

            var subscription = new PushSubscription
            {
                PushEndpoint = endpoint,
                PushP256DH = p256dh,
                PushAuth = pushAuth,
                CreatedOn = DateTime.UtcNow
            };

            db.Insert(subscription);

            return subscription;
        }

        public static IList<PushSubscription> GetSubscriptions()
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;

            var sql = new Sql()
                .Select("*")
                .From(TABLE_NAME)
                .OrderByDescending("CreatedOn");

            var pushSubscriptions = db.Query<PushSubscription>(sql);

            return pushSubscriptions.ToList();
        }

        public static int GetSubscriptionCount()
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;

            var sql = new Sql()
                .Select("*")
                .From(TABLE_NAME);

            var pushSubscriptions = db.Query<PushSubscription>(sql);

            return pushSubscriptions != null ? pushSubscriptions.Count() : 0;
        }

        public static void RemoveSubscription(PushSubscription subscription)
        {
            var dc = ApplicationContext.Current.DatabaseContext;
            var db = ApplicationContext.Current.DatabaseContext.Database;
            db.Delete<PushSubscription>(subscription);
        }
    }

}
