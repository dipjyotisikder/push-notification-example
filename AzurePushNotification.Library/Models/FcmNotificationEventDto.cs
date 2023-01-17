namespace AzurePushNotification.Library.Models
{
    public class FcmNotificationEventDto
    {
        public FcmNotificationDto Notification { get; set; }

        public object Data { get; set; }
    }

}
