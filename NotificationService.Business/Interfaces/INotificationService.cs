using NotificationService.Shared.dtos;

namespace NotificationService.Business.Interfaces
{
    public interface INotificationService
    {
        public Task<List<NotificationDto>> GetAllAsync(int userId);
        public Task<NotificationDto> GetByIdAsync(int id, int userId);
        public Task AddAsync(CreateNotificationDto notificationDto);
        public Task<List<NotificationDto>> GetUnreadMessagesAsync(int userId);
        public Task Read(int notificationId, int userId);
    }
}
