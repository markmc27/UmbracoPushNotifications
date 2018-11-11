using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPushNotification.Core.Models.Api
{
    public class AddPushSubscriptionRequest
    {
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }

        public string Token { get; set; }
    }
}
