using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MonitoringServiceApi.Controllers;
using MonitoringServiceApi.Data;
using MonitoringServiceApi.Models;
using Moq;
using Xunit;

namespace MonitoringServiceApi.Tests
{
    public class MonitoringControllerTests
    {
        private ApiDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApiDbContext(options);
        }

        private IConfiguration GetConfiguration(string apiKey = "min-super-secret-key")
        {
            var inMemorySettings = new Dictionary<string, string?> {
                {"ApiKey", apiKey}
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private DefaultHttpContext GetMockHttpContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
            return httpContext;
        }

        [Fact]
        public async Task LogError_WrongApiKey_ReturnsUnauthorized()
        {
            var context = GetDatabaseContext();
            var config = GetConfiguration();
            var controller = new MonitoringController(context, config);
            var log = new ErrorLog { InvalidUrl = "http://test.com" };

            var result = await controller.LogError("fel-nyckel", log);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task LogError_CorrectApiKey_ReturnsOkAndSavesToDb()
        {
            var context = GetDatabaseContext();
            var config = GetConfiguration("min-super-secret-key");
            var controller = new MonitoringController(context, config)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = GetMockHttpContext()
                }
            };

            var log = new ErrorLog
            {
                InvalidUrl = "http://success.com",
                Referrer = "http://google.com",
                UserAgent = "TestAgent"
            };

            var result = await controller.LogError("min-super-secret-key", log);

            if (result is ObjectResult obj && obj.StatusCode == 500)
            {
                Assert.Fail($"Felet var: {System.Text.Json.JsonSerializer.Serialize(obj.Value)}");
            }

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var count = await context.ErrorLogs.CountAsync();
            Assert.Equal(1, count);
        }
    }
}


/* 
Admin login: lms_admin_shiko
Password: Nackademin2026!
*/