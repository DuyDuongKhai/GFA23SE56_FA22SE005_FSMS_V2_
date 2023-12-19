using FSMS.Service.ViewModels.Gardens;
using FSMS.Service.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Services.NotificationServices
{
    public interface INotificationService
    {
        Task<List<GetNotification>> GetAllAsync(string? message = null, bool activeOnly = false);
        Task<GetNotification> GetAsync(int key);
        Task CreateNotificationAsync(CreateNotification createNotification);
        Task UpdateNotificationAsync(int key, UpdateNotification updateNotification);
        Task DeleteNotificationAsync(int key);
    }
}
