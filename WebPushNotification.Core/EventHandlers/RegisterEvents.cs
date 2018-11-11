using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using WebPushNotification.Core.Database.Pocos;

namespace WebPushNotification.Core.EventHandlers
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            //Check if the DB table does NOT exist
            if (!db.TableExist("PushSubscriptions"))
            {
                db.CreateTable<PushSubscription>(false);
            }
        }
    }

}
