using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PN.Models
{

    public class FcmNotification
    {
        [JsonProperty("notification")]
        public FcmNotificationPayload Payload { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}
