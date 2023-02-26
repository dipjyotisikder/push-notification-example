using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PN.Library.Models;
using PN.Models;
using PN.Services;

namespace PN.Controllers
{
    /// <summary>
    /// Controller for Push Notifications.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AzureNotificationsController : Controller
    {
        private readonly IAzurePushNotificationService _notificationService;

        public AzureNotificationsController(IAzurePushNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPut]
        [Route("deviceInstallations")]
        public async Task<IActionResult> UpdateDeviceInstallation([Required] DeviceInstallationDto deviceInstallation)
        {
            var success = await _notificationService.InstallDevice(deviceInstallation, default);

            if (!success)
            {
                return new UnprocessableEntityResult();
            }

            return new OkResult();
        }

        [HttpDelete]
        [Route("deviceInstallations/{installationId}")]
        public async Task<ActionResult> DeleteDeviceInstallation([Required][FromRoute] string installationId)
        {
            var success = await _notificationService.DeleteInstalledDeviceByInstallationId(installationId, CancellationToken.None);
            if (!success)
            {
                return new UnprocessableEntityResult();
            }

            return new OkResult();
        }

        [HttpPost]
        [Route("sendNotification")]
        public async Task<IActionResult> SendPushNotification([Required] NotificationDto notificationRequest)
        {
            if (string.IsNullOrWhiteSpace(notificationRequest?.Body) || string.IsNullOrWhiteSpace(notificationRequest?.Title))
            {
                return new BadRequestResult();
            }

            var success = await _notificationService.SendPushNotification(notificationRequest, default);

            if (!success)
            {
                return new UnprocessableEntityResult();
            }

            return new OkResult();
        }
    }
}