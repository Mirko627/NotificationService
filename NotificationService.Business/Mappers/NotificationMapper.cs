using AutoMapper;
using NotificationService.Repository.Entities;
using NotificationService.Shared.dtos;

namespace NotificationService.Business.Mappers
{
    public class VisitMapper : Profile
    {
        public VisitMapper() {
            CreateMap<Notification, NotificationDto>();

            CreateMap<CreateNotificationDto, NotificationDto>();
        }
    }
}
