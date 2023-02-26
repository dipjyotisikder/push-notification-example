using System;

namespace PN.Library.Models
{
    public class NotificationDto
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
