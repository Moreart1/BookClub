using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BookClub.DTO.Models;
using BookClub.Tests.Infrastructure;

namespace BookClub.Tests.Api
{
    public class AuthorizationApiTests : IDisposable
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly BookClubWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public AuthorizationApiTests()
        {
            _factory = new BookClubWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_WhenUserExists_ReturnsProfile()
        {
            var response = await _client.GetAsync("/api/BookClub/Login?login=morearti");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var payload = await response.Content.ReadFromJsonAsync<ServiceResponse<UserViewModel>>(JsonOptions);
            Assert.True(payload!.Success);
            Assert.Equal("morearti", payload.Data.Login);
            Assert.Equal("Марк", payload.Data.Name);
        }

        [Fact]
        public async Task Login_WhenUserMissing_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/BookClub/Login?login=ghost");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Registration_WhenLoginIsNew_CreatesUser()
        {
            var response = await _client.PostAsJsonAsync("/api/BookClub/Registration", new UserViewModel
            {
                Login = "anna",
                Name = "Анна"
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var payload = await response.Content.ReadFromJsonAsync<ServiceResponse<UserViewModel>>(JsonOptions);
            Assert.True(payload!.Success);
            Assert.Equal("anna", payload.Data.Login);

            var login = await _client.GetAsync("/api/BookClub/Login?login=anna");
            Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        }

        [Fact]
        public async Task Registration_WhenLoginTaken_ReturnsConflict()
        {
            var response = await _client.PostAsJsonAsync("/api/BookClub/Registration", new UserViewModel
            {
                Login = "morearti",
                Name = "Другой"
            });

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
