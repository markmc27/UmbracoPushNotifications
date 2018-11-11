using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPushNotification.Core.Models.Api
{
    public class ThrottleResponse
    {
        private string _errorTemplate;

        public int Seconds { get; set; }

        public string Error
        {
            get
            {
                return String.Format(_errorTemplate, Seconds);
            }

            set
            {
                _errorTemplate = value;
            }
        }
    }

}
