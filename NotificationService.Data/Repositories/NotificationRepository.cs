using Microsoft.EntityFrameworkCore;
using NotificationService.Data.Context;
using NotificationService.Repository.Interfaces;
using NotificationService.Repository.Entities;

namespace NotificationService.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext context;

        public NotificationRepository(NotificationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await context.AddAsync(notification);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Notification? n = await context.notifications.FindAsync(id);
            if (n == null)
                throw new Exception("Notification non esistente");
            context.notifications.Remove(n);
            await context.SaveChangesAsync();
        }
        public async Task<Notification?> GetByIdAsync(int id)
        {
            Notification? n = await context.notifications.FindAsync(id);
            return n;
        }


        public async Task UpdateAsync(Notification notification)
        {
            context.notifications.Update(notification);
            await context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            List<Notification> list = await context.notifications.ToListAsync();
            return list;            
        }
        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await context.notifications.Where( n => n.UserId == userId).ToListAsync();
        }

        public async Task<List<Notification>> GetByUnreadMessageUserIdAsync(int userId)
        {
            return await context.notifications.Where(n => n.UserId == userId && n.IsRead == false).ToListAsync();
        }
    }
}
