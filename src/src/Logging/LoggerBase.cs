using NuGet.Common;
using System.Threading.Tasks;

namespace Ockham.NuGet.Logging
{ 
    public abstract class LoggerBase : ILogger
    {  
        public abstract void Log(LogLevel level, string data);
        public abstract void LogInformationSummary(string data);

        public virtual void Log(ILogMessage message) => Log(message.Level, message.Message);
         
        public void LogDebug(string data) => Log(LogLevel.Debug, data);
        public void LogVerbose(string data) => Log(LogLevel.Verbose, data);
        public void LogInformation(string data) => Log(LogLevel.Information, data);
        public void LogMinimal(string data) => Log(LogLevel.Minimal, data);
        public void LogWarning(string data) => Log(LogLevel.Warning, data);
        public void LogError(string data) => Log(LogLevel.Error, data);

        public Task LogAsync(LogLevel level, string data)
        {
            Log(level, data);
            return Task.FromResult<object>(null);
        }

        public Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.FromResult<object>(null);
        }
    }
}
