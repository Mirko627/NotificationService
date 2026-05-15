namespace NotificationService.Shared.dtos
{
    public class NotificationDto
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
