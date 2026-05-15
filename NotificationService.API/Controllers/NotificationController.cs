using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Business.Interfaces;
using NotificationService.Shared.dtos;
using System.Security.Claims;

namespace NotificationService.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpPatch("{id:int}/read")]
        public async Task<IActionResult> Read(int id)
        {
            int userId = GetUserId();

            await _service.Read(id, userId);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationDto>>> GetAll()
        {
            int userId = GetUserId();

            var notifications = await _service.GetAllAsync(userId);
            return Ok(notifications);
        }
        [HttpGet("unread")]
        public async Task<ActionResult<List<NotificationDto>>> GetAllUnread()
        {
            int userId = GetUserId();

            var notifications = await _service.GetUnreadMessagesAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<NotificationDto>> GetById(int id)
        {
            int userId = GetUserId();

            NotificationDto notification = await _service.GetByIdAsync(id, userId);
            return Ok(notification);
        }

        private int GetUserId()
        {
            string? userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("ID utente non trovato nel token");

            return int.Parse(userIdClaim);
        }
    }
}
