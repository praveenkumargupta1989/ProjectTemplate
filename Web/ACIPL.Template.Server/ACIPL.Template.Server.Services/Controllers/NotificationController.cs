
using ACIPL.Template.Core.Utilities;
using ACIPL.Template.Server.Models;
using ACIPL.Template.Server.Repositories;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IGadgetDeviceRepository gadgetDeviceRepository;
        private readonly IConfigurationManager configurationManager;

        public NotificationController(INotificationRepository notificationRepository,
                                        IGadgetDeviceRepository gadgetDeviceRepository,
                                        IConfigurationManager configurationManager)
        {
            this.notificationRepository = notificationRepository;
            this.gadgetDeviceRepository = gadgetDeviceRepository;
            this.configurationManager = configurationManager;
        }

        /// <summary>
        /// Send Notification on Device
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>        
        [HttpPost]
        public IHttpActionResult Notify(NotificationBody body)
        {
            Logger.Info("Notification EmployeeId - " + body.EmployeeId);
            Logger.Info("Notification Message - " + body.Message);

            var msg = "Employee Gadget Data Not Available in the system.";

            //Get User Gadget Device Detail By EmployeeId
            var gadgetDetail = gadgetDeviceRepository.GetGadgetDeviceDetail(body.EmployeeId);
            if (gadgetDetail != null)
            {
                if (!string.IsNullOrEmpty(gadgetDetail.DeviceId))
                {
                    Logger.Info("Notification Gadget DeviceId Detail - " + gadgetDetail.DeviceId);

                    //Save Sent Notification Detail
                    NotificationStatus entity = new NotificationStatus();
                    entity.EmployeeId = body.EmployeeId;
                    entity.Command = body.Message;
                    entity.IsAck = false;
                    entity.CreatedDate = DateTime.Now;
                    notificationRepository.SaveNotificationDetail(entity);

                    //Send Notification to the Device
                    var result = SendNotification(gadgetDetail.DeviceId, body.Message, entity.Id);
                    if (result.success)
                    {
                        msg = "Notification Sent Successfully.";
                    }
                    else
                    {
                        msg = "Notification not Sent Successfully. Error Received was " + result.results.First().error;
                    }

                }
                else
                {
                    msg = "Employee Gadget DeviceId Detail not available.";
                }
            }
            return Ok(msg);
        }

        /// <summary>
        /// Update Acknowledgement of Notification
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Ack(NotificationStatus entity)
        {
            notificationRepository.SaveNotificationDetail(entity);
            return Ok(entity.IsAck);
        }

        #region Common Code
        private GcmResult SendNotification(string deviceId, string key, int id)
        {
            RestClient client = new RestClient("https://fcm.googleapis.com/");
            string SERVER_API_KEY = configurationManager.GetConfigurationValue("SERVER_API_KEY");
            var SENDER_ID = configurationManager.GetConfigurationValue("SENDER_ID");
            var request = new RestRequest("/fcm/send");
            request.AddHeader("Authorization", string.Format("key={0}", SERVER_API_KEY));
            request.AddHeader("Sender", string.Format("id={0}", SENDER_ID));
            var a = new
            {
                data = new
                {
                    message = key,
                    Id = id,
                },
                registration_ids = new[] { deviceId }
            };

            request.AddJsonBody(a);
            var resp = client.Post(request);
            var result = JsonConvert.DeserializeObject<GcmResult>(resp.Content);

            return result;
        }
        #endregion

        [HttpGet]

        public IHttpActionResult PendingNotificationToBeDelivered(string id)
        {
            return Ok(notificationRepository.GetNotificationListByEmployeeId(Convert.ToInt32(id)));

        }

    }
}
