using LoggingService;

namespace HtmlParser.Tests
{
    /// <summary>
    /// A moc of the logging service used in testing
    /// </summary>
    public class MocLoggingService : ILoggingService
    {
        public int LogLevel { get; set; }
        public void LogError(string message)
        {
            _errorList.Add(message);
        }

        private readonly List<string> _errorList = [];

        public void LogWarning(string message)
        {
            _warningList.Add(message);
        }

        private readonly List<string> _warningList = [];

        public void LogInformation(string message)
        {
            _informationList.Add(message);
        }

        private readonly List<string> _informationList = [];

        public void LogVerbose(string message)
        {
            _verboseList.Add(message);
        }

        private readonly List<string> _verboseList = [];


        public List<string> GetErrorLogs()
        {
            return _errorList;
        }

        public List<string> GetWarningLogs()
        {
            return _warningList;
        }

        public List<string> GetInformationLogs()
        {
            return _informationList;
        }

        public List<string> GetVerboseLogs()
        {
            return _verboseList;
        }
    }
}
