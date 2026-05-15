using NotificationService.Repository.Entities;

namespace NotificationService.Repository.Interfaces
{
    public interface INotificationRepository
    {
        public Task<List<Notification>> GetAllAsync();
        public Task<Notification?> GetByIdAsync(int id);
        public Task AddAsync(Notification notification);
        public Task UpdateAsync(Notification notification);
        public Task DeleteAsync(int id);
        public Task<List<Notification>> GetByUserIdAsync(int userId);
        public Task<List<Notification>> GetByUnreadMessageUserIdAsync(int userId);

    }
}
