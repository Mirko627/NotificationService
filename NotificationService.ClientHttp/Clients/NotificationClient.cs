using Microsoft.AspNetCore.Http;
using NotificationService.ClientHttp.Interfaces;
using NotificationService.Shared.dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace VisitService.Api.Client
{
    public class NotificationClient : INotificationClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            string? authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task ReadAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/notifications/{id}/read");
            AddAuthorizationHeader(request);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<NotificationDto>> GetAllUnreadAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/notifications/unread");
            AddAuthorizationHeader(request);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<NotificationDto>>() ?? [];
        }
        public async Task<NotificationDto?> GetByIdAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/notifications/{id}");
            AddAuthorizationHeader(request);

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NotificationDto>();
        }

        public async Task<List<NotificationDto>> GetAllAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/notifications");
            AddAuthorizationHeader(request);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<NotificationDto>>() ?? [];
        }
    }
}
