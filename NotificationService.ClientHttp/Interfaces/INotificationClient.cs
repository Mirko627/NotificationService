using NotificationService.Shared.dtos;

namespace NotificationService.ClientHttp.Interfaces
{
    public interface INotificationClient
    {
        public Task ReadAsync(int id);
        public Task<List<NotificationDto>> GetAllUnreadAsync();
        public Task<NotificationDto?> GetByIdAsync(int id);
        public Task<List<NotificationDto>> GetAllAsync();
    }
}
