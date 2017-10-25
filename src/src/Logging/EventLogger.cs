using NuGet.Common;
using System;

namespace Ockham.NuGet.Logging
{
    /// <summary>
    /// Event args containing data about a LogDataReceived event
    /// </summary>
    public class LogDataEventArgs : EventArgs
    {
        /// <summary>
        /// Whether the data was logged via <see cref="ILogger.LogInformationSummary(string)"/>
        /// </summary>
        public bool IsSummmary { get; private set; }

        /// <summary>
        /// The log information
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        /// The <see cref="global::NuGet.Common.LogLevel"/> specified either directly or indirectly
        /// </summary>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// Construct a new LogDataEventArgs
        /// </summary> 
        public LogDataEventArgs(LogLevel level, string data, bool isSummary)
        {
            Level = level;
            Data = data;
            IsSummmary = isSummary;
        }
    }

    /// <inheritdoc />
    public class EventLogger : LoggerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<LogDataEventArgs> LogDataReceived;

        /// <inheritdoc />
        public override void Log(LogLevel level, string data) => RaiseLogDataReceived(level, data, false);

        /// <inheritdoc />
        public override void LogInformationSummary(string data) => RaiseLogDataReceived(LogLevel.Information, data, true);

        private void RaiseLogDataReceived(LogLevel level, string data, bool isSummary)
        {
            var args = new LogDataEventArgs(level, data, isSummary);
            LogDataReceived?.Invoke(this, args);
        }
    }
}
