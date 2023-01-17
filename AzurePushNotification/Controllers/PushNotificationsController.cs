using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AzurePushNotification.Library.Models;
using AzurePushNotification.Models;
using AzurePushNotification.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzurePushNotification.Controllers
{
    /// <summary>
    /// Controller for Push Notifications.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PushNotificationsController : Controller
    {
        private readonly IAzurePushNotificationService _notificationService;

        public PushNotificationsController(IAzurePushNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPut]
        [Route("deviceInstallations")]
        public async Task<IActionResult> UpdateDeviceInstallation([Required] DeviceInstallationDto deviceInstallation)
        {
            var success = await _notificationService.InstallDeviceAsync(deviceInstallation, HttpContext.RequestAborted);

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
            var success = await _notificationService.DeleteInstalledDeviceByInstallationIdAsync(installationId, CancellationToken.None);
            if (!success)
            {
                return new UnprocessableEntityResult();
            }

            return new OkResult();
        }

        [HttpPost]
        [Route("requests")]
        public async Task<IActionResult> SendPushNotification([Required] NotificationDto notificationRequest)
        {
            if (string.IsNullOrWhiteSpace(notificationRequest?.Body) || string.IsNullOrWhiteSpace(notificationRequest?.Title))
            {
                return new BadRequestResult();
            }

            var success = await _notificationService.SendPushNotificationAsync(notificationRequest, HttpContext.RequestAborted);

            if (!success)
            {
                return new UnprocessableEntityResult();
            }

            return new OkResult();
        }
    }
}