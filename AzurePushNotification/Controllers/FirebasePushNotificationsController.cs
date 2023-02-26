using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class FirebasePushNotificationsController : Controller
    {
        public FirebasePushNotificationsController()
        {
        }

        [HttpPost]
        [Route("sendNotification")]
        public async Task<IActionResult> SendNotification([Required] SendFirebaseNotificationRequest notification)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            var result = await messaging.SendAsync(new Message
            {
                Token = notification.FcmToken, // "d3aLewjvTNw:APA91bE94LuGCqCSInwVaPuL1RoqWokeSLtwauyK-r0EmkPNeZmGavSG6ZgYQ4GRjp0NgOI1p-OAKORiNPHZe2IQWz5v1c3mwRE5s5WTv6_Pbhh58rY0yGEMQdDNEtPPZ_kJmqN5CaIc",
                Data = new Dictionary<string, string>(),
                Notification = new Notification
                {
                    Title = notification.Title,
                    Body = notification.Body,
                },
            });

            return Ok(result);
        }
    }
}
