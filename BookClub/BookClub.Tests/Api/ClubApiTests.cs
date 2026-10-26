using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BookClub.DTO.Models;
using BookClub.Tests.Infrastructure;

namespace BookClub.Tests.Api
{
    public class ClubApiTests : IDisposable
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly BookClubWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ClubApiTests()
        {
            _factory = new BookClubWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task AllBooks_ReturnsSeededCatalog()
        {
            var response = await _client.GetAsync("/api/Club/AllBooks");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var payload = await response.Content.ReadFromJsonAsync<ServiceResponse<List<BookDto>>>(JsonOptions);
            Assert.NotNull(payload);
            Assert.True(payload!.Success);
            Assert.Equal(8, payload.Data.Count);
        }

        [Fact]
        public async Task AddBookToUser_WhenUserMissing_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Club/AddBookToUser?userId=99&bookId=1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UserBookFlow_AddListAndDelete()
        {
            var addResponse = await _client.GetAsync("/api/Club/AddBookToUser?userId=1&bookId=3");
            Assert.Equal(HttpStatusCode.OK, addResponse.StatusCode);

            var listResponse = await _client.GetAsync("/api/Club/AllUserBooks?userId=1");
            var listPayload = await listResponse.Content.ReadFromJsonAsync<ServiceResponse<List<BookDto>>>(JsonOptions);
            Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
            Assert.Contains(listPayload!.Data, book => book.Id == 3);

            var deleteResponse = await _client.GetAsync("/api/Club/DeleteUserBook?userId=1&bookId=3");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var emptyResponse = await _client.GetAsync("/api/Club/AllUserBooks?userId=1");
            var emptyPayload = await emptyResponse.Content.ReadFromJsonAsync<ServiceResponse<List<BookDto>>>(JsonOptions);
            Assert.DoesNotContain(emptyPayload!.Data, book => book.Id == 3);
        }

        [Fact]
        public async Task AllUserBooks_WhenUserMissing_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Club/AllUserBooks?userId=99");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        private sealed class BookDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
