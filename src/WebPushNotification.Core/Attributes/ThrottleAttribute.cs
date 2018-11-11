using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using WebPushNotification.Core.Helpers;
using WebPushNotification.Core.Models.Api;

namespace WebPushNotification.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public int Seconds { get; set; }
        public string Message { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var key = string.Concat(Name, "-", ClientHelper.GetClientIp(actionContext.Request));
            var allowExecute = false;

            if (HttpRuntime.Cache[key] == null)
            {
                HttpRuntime.Cache.Add(key, true, null, DateTime.Now.AddSeconds(Seconds), Cache.NoSlidingExpiration, CacheItemPriority.Low, null);

                allowExecute = true;
            }

            if (!allowExecute)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                     HttpStatusCode.Conflict,
                     new ThrottleResponse
                     {
                         Seconds = Seconds,
                         Error = Message
                     }
                 );
            }
        }
    }
}
