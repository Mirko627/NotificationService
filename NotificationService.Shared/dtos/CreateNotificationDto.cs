namespace NotificationService.Shared.dtos
{
    public class CreateNotificationDto
    {
        public required int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
