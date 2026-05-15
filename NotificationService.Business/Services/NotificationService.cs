using AutoMapper;
using NotificationService.Business.Interfaces;
using NotificationService.Repository.Entities;
using NotificationService.Repository.Interfaces;
using NotificationService.Shared.dtos;
using System.Threading.Tasks;
using UserService.ClientHttp.Interfaces;
using UserService.Shared.dtos;

namespace NotificationService.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IUserClient _userClient;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository repository, IMapper mapper, IUserClient userClient)
        {
            _repository = repository;
            _mapper = mapper;
            _userClient = userClient;
        }

        public async Task AddAsync(CreateNotificationDto notificationDto)
        {
            UserDto u = await _userClient.GetByIdAsync(notificationDto.UserId) ?? throw new KeyNotFoundException("Utente non esistente");

            Notification notification = _mapper.Map<Notification>(notificationDto);
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            
            await _repository.AddAsync(notification);
        }

        public async Task<List<NotificationDto>> GetAllAsync(int userId)
        {
            List<Notification> notifications = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<List<NotificationDto>>(notifications);
        }
        public async Task<List<NotificationDto>> GetUnreadMessagesAsync(int userId)
        {
            List<Notification> notifications = await _repository.GetByUnreadMessageUserIdAsync(userId);
            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<NotificationDto> GetByIdAsync(int id, int userId)
        {
            Notification notification = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Notifica non esistente");
            if (notification.UserId != userId) throw new UnauthorizedAccessException("Accesso negato alla notifica");
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task Read(int notificationId, int userId)
        {
            Notification notification = await _repository.GetByIdAsync(notificationId) ?? throw new KeyNotFoundException("Notifica non esistente");
            if (notification.UserId != userId) throw new UnauthorizedAccessException("Accesso negato alla notifica");
            notification.IsRead = true;
            await _repository.UpdateAsync(notification);
        }
    }
}
