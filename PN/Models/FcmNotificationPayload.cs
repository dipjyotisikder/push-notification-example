using Newtonsoft.Json;

namespace PN.Models
{

    public class FcmNotificationPayload
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
