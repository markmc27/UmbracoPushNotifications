using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPushNotification.Core
{
    public static class Caching
    {
        public static string CURRENT_CACHE_KEY = DateTime.Now.Ticks.ToString();
        public const string CUSTOM_KEY = "UmbracoOutputCacheKey";
        public const int CACHE_DURATION = 180;
    }
}
