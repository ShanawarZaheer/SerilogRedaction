namespace SerilogRedaction.Common
{
    public class Logger
    {
        private readonly ILogger<Logger> _logger;
        public Logger(ILogger<Logger> logger)
        {
            _logger = logger;
        }

        public static void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }





    }
}
