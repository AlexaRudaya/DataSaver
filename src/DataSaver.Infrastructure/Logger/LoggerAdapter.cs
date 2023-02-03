namespace DataSaver.Infrastructure.Logger
{
    public sealed class LoggerAdapter<T> : IAppLogger<T> // mb I'll remove it later
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            _logger = loggerFactory.AddSerilog(logger).CreateLogger<T>();
        }

        public void LogError(Exception exception, string? message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}
