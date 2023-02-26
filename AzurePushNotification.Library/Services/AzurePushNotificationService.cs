using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PN.Library.Models;
using PN.Models;

namespace PN.Services
{
    /// <summary>
    /// Instantiate <see cref="AzurePushNotificationService" /> class.
    /// </summary>
    public class AzurePushNotificationService : IAzurePushNotificationService
    {
        private readonly NotificationHubClient _notificationHub;
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
            _notificationHub = NotificationHubClient.CreateClientFromConnectionString(options.CurrentValue.ConnectionString, options.CurrentValue.Name);

            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Fcm).ToLower(), NotificationPlatform.Fcm },
              /*{ nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
                { nameof(NotificationPlatform.Wns).ToLower(), NotificationPlatform.Wns },*/
            };
        }

        /// <inheritdoc/>
        public async Task<bool> InstallDevice(DeviceInstallationDto deviceInstallationDto, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallationDto.InstallationId)
                || string.IsNullOrWhiteSpace(deviceInstallationDto.PushChannel)
                || string.IsNullOrWhiteSpace(deviceInstallationDto.Platform)
                || !Enum.TryParse(deviceInstallationDto.Platform, ignoreCase: true, out NotificationPlatform result)
                || deviceInstallationDto.Tags is null
                || deviceInstallationDto.Tags.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                return false;
            }

            var deviceInstallation = new Installation()
            {
                InstallationId = deviceInstallationDto.InstallationId,
                PushChannel = deviceInstallationDto.PushChannel,
                Tags = deviceInstallationDto.Tags,
                Platform = result,
            };

            try
            {
                await _notificationHub.CreateOrUpdateInstallationAsync(deviceInstallation, token);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteInstalledDeviceByInstallationId(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
            {
                return false;
            }

            try
            {
                await _notificationHub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> SendPushNotification(NotificationDto notificationDto, CancellationToken token)
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

        private List<Task> PreparePushNotificationRequests(string title, string body, string[] tags, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var platform in _installationPlatform.Values)
            {
                switch (platform)
                {
                    case NotificationPlatform.Fcm:
                        var json = JsonConvert.SerializeObject(
                        new FcmNotificationEventDto
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
                        },
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        });

                        tasks.Add(_notificationHub.SendFcmNativeNotificationAsync(
                            json,
                            tags,
                            cancellationToken));
                        break;

                    case NotificationPlatform.Apns:
                        var iosJson = JsonConvert.SerializeObject(
                        new ApnNotificationDto
                        {
                            Aps = new ApsDto
                            {
                                Alert = new ApsAlertDto
                                {
                                    Title = title,
                                    Body = body,
                                },
                            },
                        },
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        });

                        tasks.Add(_notificationHub.SendAppleNativeNotificationAsync(
                            iosJson,
                            tags,
                            cancellationToken));
                        break;

                    case NotificationPlatform.Wns:
                        tasks.Add(_notificationHub.SendWindowsNativeNotificationAsync(
                            @$"<toast launch='payload=%7B%22test%22%3A%22value%22%7D'>
                                <visual lang='en-US'>
                                <binding template='ToastGeneric'>
                                <text>{title}</text>
                                <text>{body}</text>
                                </binding>
                                </visual>
                              </toast>",
                            tags,
                            cancellationToken));
                        break;

                    default:
                        break;
                }
            }

            return tasks;
        }
    }
}
