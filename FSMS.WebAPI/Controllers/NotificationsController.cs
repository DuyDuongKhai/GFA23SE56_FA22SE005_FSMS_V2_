using FSMS.Service.Services.NotificationServices;
using FSMS.Service.Utility;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using FSMS.Service.ViewModels.Notifications;
using FSMS.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FSMS.WebAPI.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private INotificationService _notificationService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public NotificationsController(INotificationService notificationService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _notificationService = notificationService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpGet]
        [Cache(1000)]
        //[PermissionAuthorize("Admin", "Customer", "Supplier", "Farmer", "Expert")]
        public async Task<IActionResult> GetAllNotifications(string? message = null, bool activeOnly = false)
        {
            try
            {
                List<GetNotification> notifications = await _notificationService.GetAllAsync(message, activeOnly);
                return Ok(new
                {
                    Data = notifications
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }



        [HttpGet("{id}")]
        [Cache(1000)]

        [PermissionAuthorize("Admin", "Customer", "Supplier", "Farmer", "Expert")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                GetNotification notification = await _notificationService.GetAsync(id);
                return Ok(new
                {
                    Data = notification
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost]

        [PermissionAuthorize("Admin", "Customer", "Supplier", "Farmer", "Expert")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotification createNotification)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _notificationService.CreateNotificationAsync(createNotification);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}")]

        [PermissionAuthorize("Admin", "Customer", "Supplier", "Farmer", "Expert")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] UpdateNotification updateNotification)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _notificationService.UpdateNotificationAsync(id, updateNotification);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]

        [PermissionAuthorize("Admin", "Customer", "Supplier", "Farmer", "Expert")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                await _notificationService.DeleteNotificationAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
