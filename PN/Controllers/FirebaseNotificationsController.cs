using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PN.Models;

namespace PN.Controllers
{
    /// <summary>
    /// Controller for Firebase Push Notifications.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FirebaseNotificationsController : Controller
    {
        public FirebaseNotificationsController()
        {
        }

        [HttpPost]
        [Route("sendNotification")]
        public async Task<IActionResult> SendNotification([Required] SendFirebaseNotificationRequest notification)
        {
            var messaging = FirebaseMessaging.DefaultInstance;
            var tokens = new List<string> { notification.FcmToken };
            var response = await messaging.SendMulticastAsync(new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification
                {
                    Title = notification.Title,
                    Body = notification.Body,
                },
                Data = new Dictionary<string, string>()
                {
                    { "fcmToken", notification.FcmToken },
                },
            });

            if (response.FailureCount > 0)
            {
                var failedFcmTokens = response.Responses
                    .Where(x => !x.IsSuccess && x.Exception.MessagingErrorCode == MessagingErrorCode.Unregistered)
                    .Select((x, index) => tokens[index]).ToList();

                // Remove failed tokens from Database.

            }

            return Ok(response);
        }
    }
}
