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
