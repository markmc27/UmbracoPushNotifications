using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPushNotification.Core.Push
{
    public interface IPushService
    {
        Task SendNotificationsAsync(string title, string message, string clickUrl);
    }
}
