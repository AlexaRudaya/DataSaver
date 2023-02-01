namespace DataSaver.ApplicationCore.Interfaces.ILogger
{
    public interface IAppLogger<T>
    {
        public void LogInformation(string message, params object[] args);

        public void LogWarning(string message, params object[] args);

        public void LogError(Exception exception, string? message, params object[] args);
    }
}
