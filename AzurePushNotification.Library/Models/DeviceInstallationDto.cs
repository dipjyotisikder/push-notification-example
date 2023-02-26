using System;
using System.Collections.Generic;

namespace PN.Models
{
    /// <summary>
    /// Class for device installation.
    /// </summary>
    public class DeviceInstallationDto
    {
        public string InstallationId { get; set; }

        public string Platform { get; set; }

        public string PushChannel { get; set; }

        public IList<string> Tags { get; set; } = Array.Empty<string>();
    }
}
