using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzurePushNotification.Library.Models;
using AzurePushNotification.Models;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AzurePushNotification.Services
{
    /// <summary>
    /// Instantiate <see cref="AzurePushNotificationService" /> class.
    /// </summary>
    public class AzurePushNotificationService : IAzurePushNotificationService
    {
        private readonly NotificationHubClient _hub;
        private readonly Dictionary<string, NotificationPlatform> _installationPlatform;
        private readonly ILogger<AzurePushNotificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePushNotificationService"/> class.
        /// </summary>
        /// <param name="options">The notification hub configuration options.</param>
        /// <param name="logger">The logger service.</param>
        public AzurePushNotificationService(IOptionsMonitor<NotificationHubOptions> options, ILogger<AzurePushNotificationService> logger)
        {
            _logger = logger;
            _hub = NotificationHubClient.CreateClientFromConnectionString(options.CurrentValue.ConnectionString, options.CurrentValue.Name);

            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Fcm).ToLower(), NotificationPlatform.Fcm },

                // { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns }
            };
        }

        /// <inheritdoc/>
        public async Task<bool> InstallDeviceAsync(DeviceInstallationDto deviceInstallationDto, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallationDto?.InstallationId)
                || string.IsNullOrWhiteSpace(deviceInstallationDto?.PushChannel)
                || string.IsNullOrWhiteSpace(deviceInstallationDto?.Platform)
                || deviceInstallationDto?.Tags is null)
            {
                return false;
            }

            var deviceInstallation = new Installation()
            {
                InstallationId = deviceInstallationDto.InstallationId,
                PushChannel = deviceInstallationDto.PushChannel,
                Tags = deviceInstallationDto.Tags,
            };

            if (_installationPlatform.TryGetValue(deviceInstallationDto.Platform, out var platform))
            {
                deviceInstallation.Platform = platform;
            }
            else
            {
                return false;
            }

            try
            {
                await _hub.CreateOrUpdateInstallationAsync(deviceInstallation, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteInstalledDeviceByInstallationIdAsync(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
            {
                return false;
            }

            try
            {
                await _hub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SendPushNotificationAsync(NotificationDto notificationDto, CancellationToken token)
        {
            int maximumTagsPerRequest = 5;

            if (string.IsNullOrWhiteSpace(notificationDto?.Body)
                || string.IsNullOrWhiteSpace(notificationDto?.Title)
                || notificationDto?.Tags is null)
            {
                return false;
            }

            try
            {
                var notificationTasks = new List<Task>();
                if (notificationDto.Tags.Length <= maximumTagsPerRequest)
                {
                    notificationTasks = PreparePushNotificationRequests(notificationDto.Title, notificationDto.Body, notificationDto.Tags, token);
                }
                else
                {
                    notificationTasks = notificationDto.Tags
                       .Select((value, index) => (value, index))
                       .GroupBy(g => g.index / maximumTagsPerRequest, i => i.value)
                       .SelectMany(tags => PreparePushNotificationRequests(notificationDto.Title, notificationDto.Body, tags.Select(y => y).ToArray(), token))
                       .ToList();
                }

                await Task.WhenAll(notificationTasks);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending notifications");
                return false;
            }
        }

        private List<Task> PreparePushNotificationRequests(string title, string body, string[] tags, CancellationToken token)
        {
            var tasks = new List<Task>();
            foreach (var platform in _installationPlatform.Values)
            {
                switch (platform)
                {
                    case NotificationPlatform.Fcm:
                        tasks.Add(_hub.SendFcmNativeNotificationAsync(
                            JsonConvert.SerializeObject(new FcmNotificationEventDto
                            {
                                Notification = new FcmNotificationDto
                                {
                                    Title = title,
                                    Body = body,
                                },
                                Data = new Dictionary<string, string>
                                {
                                    { "title", title },
                                    { "body", body },
                                },
                            }),
                            tags,
                            token));
                        break;

                    case NotificationPlatform.Apns:
                        tasks.Add(_hub.SendAppleNativeNotificationAsync(
                            JsonConvert.SerializeObject(new ApnNotificationDto
                            {
                                Aps = new ApsDto
                                {
                                    Alert = body,
                                },
                            }),
                            tags,
                            token));
                        break;

                    default:
                        break;
                }
            }

            return tasks;
        }
    }
}
