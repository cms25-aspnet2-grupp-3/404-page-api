namespace MonitoringServiceApi.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }

        public string InvalidUrl { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string? Referrer { get; set; }

        public string? UserAgent { get; set; }

        public string? IpAddress { get; set; }
    }
}