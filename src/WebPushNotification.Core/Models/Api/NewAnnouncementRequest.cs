using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPushNotification.Core.Models.Api
{
    public class NewAnnouncementRequest
    {
        public string ShortName { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }
        public string[] Types { get; set; }
    }
}
