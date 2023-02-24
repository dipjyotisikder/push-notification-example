using System.Threading;
using System.Threading.Tasks;
using AzurePushNotification.Library.Models;
using AzurePushNotification.Models;

namespace AzurePushNotification.Services
{
    public interface IAzurePushNotificationService
    {
        /// <summary>
        /// Installs or Updates new device installation in Azure.
        /// </summary>
        /// <param name="deviceInstallationDto">The <see cref="DeviceInstallationDto"/> DTO for installing the device.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Installation was successful or not.</returns>
        Task<bool> InstallDevice(DeviceInstallationDto deviceInstallationDto, CancellationToken token);

        /// <summary>
        /// Removes the device installed in Azure.
        /// </summary>
        /// <param name="installationId">Unique installation ID, provided by device.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Deletion was successful or not.</returns>
        Task<bool> DeleteInstalledDeviceByInstallationId(string installationId, CancellationToken token);

        /// <summary>
        /// Sends the notification to Azure.
        /// </summary>
        /// <param name="notificationDto">The notification DTO model.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>Notification was successfully sent or not.</returns>
        Task<bool> SendPushNotification(NotificationDto notificationDto, CancellationToken token);
    }
}
