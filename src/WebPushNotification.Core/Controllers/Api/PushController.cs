using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Umbraco.Web.WebApi;
using WebPushNotification.Core.Attributes;
using WebPushNotification.Core.Database;
using WebPushNotification.Core.Models.Api;

namespace WebPushNotification.Core.Controllers.Api
{
    public class PushController : UmbracoApiController
    {
        // GET: /umbraco/api/push/add
        [Throttle(Name = "PushSubscription", Message = "Subscription rate violated.", Seconds = 3600)]
        public bool Add(AddPushSubscriptionRequest request)
        {
            var tokenParts = request.Token.Split(':');
            ValidateToken(tokenParts[0], tokenParts[1]);

            PushSubsciptions.CreateSubscription(request.PushEndpoint, request.PushP256DH, request.PushAuth);

            return true;
        }

        private void ValidateToken(string cookieToken, string formToken)
        {
            AntiForgery.Validate(cookieToken, formToken);
        }
    }

}
