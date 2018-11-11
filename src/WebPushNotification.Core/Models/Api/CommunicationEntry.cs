using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPushNotification.Core.Models.Api
{
    public class CommunicationEntry
    {
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public string User { get; set; }
        public int NodeId { get; set; }
    }
}
