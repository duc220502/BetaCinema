using BetaCinema.Application.DTOs.Notification;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using BetaCinema.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController(INotificationService notificationService , IUserNotificationService userNotification) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly IUserNotificationService _userNotificationService = userNotification;


        [HttpPost]
        [Authorize(Policy = "AdminApi")]
        public async Task<IActionResult> CreateNotification([FromBody] Request_CreateNotification request, CancellationToken ct = default)
        {
            var result = await _notificationService.CreateUserNotificationAsync(request, ct);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotifications( CancellationToken ct = default)
        {

            var result = await _userNotificationService.GetUserNotificationsAsync(ct);
            return Ok(result);
        }

        [HttpGet("unread-count")]
        [Authorize]
        public async Task<IActionResult> GetUnreadCount(CancellationToken ct = default)
        {
            var result = await _userNotificationService.GetUnreadCountAsync( ct);
            return Ok(result);
        }

        [HttpPut("{userNotificationId:guid}/read")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid userNotificationId, CancellationToken ct = default)
        {
            var result = await _userNotificationService.MarkAsReadAsync(userNotificationId, ct);
            return Ok(result);
        }


    }
}
