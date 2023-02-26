using Newtonsoft.Json;

namespace PN.Models
{

    public class SendFirebaseNotificationRequest
    {
        [JsonProperty("fcmToken")]
        public string FcmToken { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
