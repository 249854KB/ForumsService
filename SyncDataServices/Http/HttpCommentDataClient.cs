using System.Text;
using System.Text.Json;
using ForumsService.Dtos;

namespace ForumsService.SyncDataServices.Http
{
    public class HttpCommentDataClient : ICommentDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommentDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task SendForumToComment(ForumReadDto forum)
        {
           var httpContent = new StringContent(
            JsonSerializer.Serialize(forum),
            Encoding.UTF8,
            "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["CommentService"]}" , httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommentService was OK");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommentService was ERROR");
            }
        }
    }
}