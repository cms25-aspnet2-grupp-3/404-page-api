using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringServiceApi.Data;
using MonitoringServiceApi.Models;

namespace MonitoringServiceApi.Controllers
{
    [ApiController]
    [Route("api/monitoring")]
    public class MonitoringController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IConfiguration _config;

        public MonitoringController(ApiDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogError(
            [FromHeader(Name = "x-api-key")] string? apiKey,
            [FromBody] ErrorLog log)
        {
            var validApiKey = _config["ApiKey"];

            if (string.IsNullOrEmpty(apiKey) || apiKey != validApiKey)
            {
                return Unauthorized(new { message = "Invalid or missing API Key" });
            }

            try
            {
                log.Timestamp = DateTime.UtcNow;
                log.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                _context.ErrorLogs.Add(log);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Database error", error = ex.Message });
            }
        }

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _context.ErrorLogs
                .OrderByDescending(x => x.Timestamp)
                .Take(50)
                .ToListAsync();

            return Ok(logs);
        }
    }
}