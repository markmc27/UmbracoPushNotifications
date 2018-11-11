using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using WebPushNotification.Core.Models.ViewModels;

namespace WebPushNotification.Core.Controllers
{
    public class PartialController: SurfaceController
    {
        [ChildActionOnly]
        [OutputCache(Duration = 180, VaryByCustom = Caching.CUSTOM_KEY)]
        public PartialViewResult GlobalVariablables()
        {
            var viewModel = new GlobalVariablesViewModel
            {
                VapidPublicKey = ConfigurationManager.AppSettings["Push:VapidPublicKey"] as string
            };

            return PartialView("GlobalVariables", viewModel);
        }
    }
}
