using AutoMapper;
using FSMS.Entity.Models;
using FSMS.Entity.Repositories.GardenRepositories;
using FSMS.Entity.Repositories.NotificationRepositories;
using FSMS.Entity.Repositories.UserRepositories;
using FSMS.Service.Enums;
using FSMS.Service.Services.Notifications;
using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Notifications;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private IUserRepository _userRepository;
        private INotificationRepository _notificationRepository;
        private IMapper _mapper;


        public NotificationService(IUserRepository userRepository, IMapper mapper, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task CreateNotificationAsync(CreateNotification createNotification)
        {
            try
            {
                User existedUser = (await _userRepository.GetByIDAsync(createNotification.UserId));
                if (existedUser == null)
                {
                    throw new Exception("UserId does not exist in the system.");
                }
                int lastId = (await _notificationRepository.GetAsync()).Max(x => x.NotificationId);
                Notification notification = new Notification()
                {
                    Message = createNotification.Message,
                    NotificationType = createNotification.NotificationType,
                    UserId = createNotification.UserId,
                    IsRead = false,
                    Status = StatusEnums.Active.ToString(),
                    CreatedDate = DateTime.Now,
                    UpdateDate = createNotification.UpdateDate,
                    NotificationId = lastId + 1
                };
                
                await _notificationRepository.InsertAsync(notification);
                await _notificationRepository.CommitAsync();

                TimeSpan time = createNotification.UpdateDate.Value.TimeOfDay;
                /*                Hangfire.BackgroundJob.Schedule(() => SendNotificationAsync(notification.NotificationId), TimeSpan.FromMinutes(timeDifference.TotalMinutes));
                */
                RecurringJob.AddOrUpdate(notification.NotificationId.ToString(),
                                      () => SendNotificationAsync(notification.NotificationId),
                                      cronExpression: ConvertTimeSpanToCron(time),
                                      new RecurringJobOptions
                                      {
                                          // sync time(utc +7)
                                          TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"),
                                      });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string ConvertTimeSpanToCron(TimeSpan timeSpan)
        {
            string minute = timeSpan.Minutes == 0 ? "*" : timeSpan.Minutes.ToString();

            return $"{minute} {timeSpan.Hours} * * *";
        }
        public static string ConvertDaysToCron(int days)
        {
            // Handle edge case of 0 days
            if (days == 0)
            {
                return "* * * * *";
            }

            // Calculate the interval between executions in minutes
            int minutesPerDay = 24 * 60;
            int intervalMinutes = minutesPerDay * days;

            // Convert minutes to Cron format
            string minute = intervalMinutes == 0 ? "*" : intervalMinutes.ToString();

            // Return the Cron expression
            return $"{minute} * * * *";
        }
        public async Task SendNotificationAsync(int notificationId)
        {

            Notification notification = await _notificationRepository.GetByIDAsync(notificationId);
            Dictionary<string, string> data = new Dictionary<string, string>()
                {
                    { "type", "notification" },
                    { "notificationId", notification.NotificationId.ToString() }
                };
            await PushNotification.SendMessage(notification.UserId.ToString(), $"{notification.NotificationType}",
                $"{notification.Message} ", data);


            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }
        public async Task DeleteNotificationAsync(int key)
        {
            try
            {
                Notification existedNotification = await _notificationRepository.GetByIDAsync(key);

                if (existedNotification == null)
                {
                    throw new Exception("Notification ID does not exist in the system.");
                }

                existedNotification.Status = StatusEnums.InActive.ToString();

                await _notificationRepository.UpdateAsync(existedNotification);
                await _notificationRepository.CommitAsync();
                RecurringJob.RemoveIfExists(key.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<GetNotification> GetAsync(int key)
        {
            try
            {
                Notification notification = await _notificationRepository.GetByIDAsync(key);

                if (notification == null)
                {
                    throw new Exception("Notification ID does not exist in the system.");
                }

                List<GetNotification> notifications = _mapper.Map<List<GetNotification>>(
                    await _notificationRepository.GetAsync(includeProperties: "User")
                );

                GetNotification result = _mapper.Map<GetNotification>(notification);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetNotification>> GetAllAsync(string? message = null, bool activeOnly = false)
        {
            try
            {
                List<GetNotification> notifications = _mapper.Map<List<GetNotification>>(
                    (await _notificationRepository.GetAsync(includeProperties: "User"))
                    .Where(notification =>
                        (string.IsNullOrEmpty(message) || notification.Message.Contains(message, StringComparison.OrdinalIgnoreCase)) &&
                        (!activeOnly || notification.Status == StatusEnums.Active.ToString())
                    )
                );

                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task UpdateNotificationAsync(int key, UpdateNotification updateNotification)
        {
            try
            {
                Notification existedNotification = await _notificationRepository.GetByIDAsync(key);

                if (existedNotification == null)
                {
                    throw new Exception("Notification Id does not exist in the system.");
                }


                if (!string.IsNullOrEmpty(updateNotification.Message))
                {
                    existedNotification.Message = updateNotification.Message;
                }


                existedNotification.IsRead = updateNotification.IsRead;
                

                if (!string.IsNullOrEmpty(updateNotification.NotificationType))
                {
                    existedNotification.NotificationType = updateNotification.NotificationType;
                }

                if (!string.IsNullOrEmpty(updateNotification.Status))
                {
                    if (updateNotification.Status != "Active" && updateNotification.Status != "InActive")
                    {
                        throw new Exception("Status must be 'Active' or 'InActive'.");
                    }
                    existedNotification.Status = updateNotification.Status;
                }
                existedNotification.UpdateDate = updateNotification.UpdateDate;


                await _notificationRepository.UpdateAsync(existedNotification);
                await _notificationRepository.CommitAsync();

                RecurringJob.RemoveIfExists(key.ToString());
                TimeSpan time = updateNotification.UpdateDate.Value.TimeOfDay;
                /*                Hangfire.BackgroundJob.Schedule(() => SendNotificationAsync(notification.NotificationId), TimeSpan.FromMinutes(timeDifference.TotalMinutes));
                */
                RecurringJob.AddOrUpdate(existedNotification.NotificationId.ToString(),
                                      () => SendNotificationAsync(existedNotification.NotificationId),
                                      cronExpression: ConvertTimeSpanToCron(time),
                                      new RecurringJobOptions
                                      {
                                          // sync time(utc +7)
                                          TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"),
                                      });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
