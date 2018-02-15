using Microsoft.Extensions.Logging;

namespace QIQO.Business.Api
{
    public class LogTesting
    {
        private readonly ILogger<LogTesting> _logger;
        public LogTesting(ILogger<LogTesting> logger)
        {
            _logger = logger;
            _logger.LogCritical($"LogTesting of type {typeof(LogTesting)}");
        }
    }
}
