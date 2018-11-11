using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Editors;
using WebPushNotification.Core.Database;
using WebPushNotification.Core.Models.Api;
using WebPushNotification.Core.Push;

namespace WebPushNotification.Core.Controllers.Api
{
    [Umbraco.Web.Mvc.PluginController("Communication")]
    public class CommunicationController : UmbracoAuthorizedJsonController
    {
        public CommunicationController()
        {
            TempFolderPath = Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/"), "CommunicationHistory");
            _pushService = new PushService();
        }

        private readonly string TempFolderPath;
        private static readonly string LogFileName = "Log.json";

        private readonly IPushService _pushService;

        public IList<CommunicationEntry> GetHistory()
        {
            EnsureLogFile();
            return GetHistoryLog(Path.Combine(TempFolderPath, LogFileName));
        }

        public int GetSubscriptionCount()
        {
            return PushSubsciptions.GetSubscriptionCount();
        }

        [HttpPost]
        public async Task<object> NewAnnouncement(NewAnnouncementRequest request)
        {
            var nodeId = CreateAnnouncementPage(request);
            var announcementPageUrl = UmbracoContext.ContentCache.GetById(nodeId).UrlWithDomain();
            foreach (var commType in request.Types)
            {
                switch (commType)
                {
                    //add other announcement streams here
                    case "Push":
                        await SendPushNotifications(request, announcementPageUrl);
                        break;
                }
            }

            WriteCommunicationLogEntry(request, nodeId, Security.CurrentUser.Name);

            return new { url = announcementPageUrl };
        }

        private IList<CommunicationEntry> GetHistoryLog(string filePath)
        {
            IList<CommunicationEntry> returnList = null;

            if (File.Exists(filePath))
            {
                var logText = File.ReadAllText(filePath);

                try
                {
                    returnList = JsonConvert.DeserializeObject<IList<CommunicationEntry>>(logText);
                }
                catch (Exception e)
                {
                    LogHelper.Error<Exception>("Error deserialising communication log", e);
                }
            }

            return returnList ?? new List<CommunicationEntry>();
        }

        private void EnsureLogFile()
        {
            if (!Directory.Exists(TempFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(TempFolderPath);
                }
                catch (Exception e)
                {
                    LogHelper.Error<Exception>($"Error creating communication folder at: {TempFolderPath}", e);

                }
            }

            var filePath = Path.Combine(TempFolderPath, LogFileName);
            if (!File.Exists(filePath))
            {
                try
                {
                    File.AppendAllText(filePath, "");
                }
                catch (Exception e)
                {
                    LogHelper.Error<Exception>($"Error creating communication log file at: {filePath}", e);

                }
            }
        }

        private int CreateAnnouncementPage(NewAnnouncementRequest request)
        {
            var contentService = Services.ContentService;
            var homepage = UmbracoContext.ContentCache.GetByRoute("/");
            var announcements = homepage.DescendantOrSelf("announcementsFolder");

            var parent = contentService.GetById(announcements.Id);

            var newContent = contentService.CreateContent($"{DateTime.Now.ToString("yyyyMMdd")}-{request.ShortName}", parent, "newsPage");

            newContent.SetValue("headerTitle", request.Heading);
            newContent.SetValue("introduction", request.Text);
            newContent.SetValue("offsetHeader", true);
            newContent.SetValue("noIndex", true);
            newContent.SetValue("hideFromSitemap", true);

            var result = contentService.SaveAndPublishWithStatus(newContent);

            if (result.Success)
            {
                return newContent.Id;
            }
            else
            {
                var exception = result.Exception;
            }

            return -1;
        }

        private void WriteCommunicationLogEntry(NewAnnouncementRequest request, int nodeId, string user)
        {
            var newEntry = new CommunicationEntry
            {
                Title = request.Heading,
                SentDate = DateTime.Now,
                NodeId = nodeId,
                User = user
            };

            var filePath = Path.Combine(TempFolderPath, LogFileName);
            var existingLog = GetHistoryLog(filePath);

            existingLog.Add(newEntry);

            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(existingLog.OrderByDescending(x => x.SentDate).ToList()));
            }
            catch (Exception e)
            {
                LogHelper.Error<Exception>("Error serialising communication log", e);
            }
        }

        private async Task SendPushNotifications(NewAnnouncementRequest request, string url)
        {
            await _pushService.SendNotificationsAsync("Push Notification Demo", $"{DateTime.Now.ToString("dd MMM")} {request.Heading}", url);
        }
    }
}
