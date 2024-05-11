namespace LoggingService;

public interface ILoggingService
{
    /// <summary>
    /// The log level of the service
    /// </summary>
    int LogLevel { get; set; }

    /// <summary>
    /// Log an error with a message
    /// </summary>
    /// <param name="message">the error message</param>
    void LogError(string message);

    /// <summary>
    /// Log a warning with a message
    /// </summary>
    /// <param name="message">the warning message</param>
    void LogWarning(string message);

    /// <summary>
    /// log an information message
    /// </summary>
    /// <param name="message">the information message</param>
    void LogInformation(string message);

    /// <summary>
    /// log a verbose message
    /// </summary>
    /// <param name="message">the verbose message</param>
    void LogVerbose(string message);

    /// <summary>
    /// gets all errors
    /// </summary>
    /// <returns>a list of error messages</returns>
    List<string> GetErrorLogs();

    /// <summary>
    /// Gets all warnings 
    /// </summary>
    /// <returns>a list of warning messages</returns>
    List<string> GetWarningLogs();

    /// <summary>
    /// Gets all information messages
    /// </summary>
    /// <returns>a list of all information messages</returns>
    List<string> GetInformationLogs();

    /// <summary>
    /// Gets all verbose messages
    /// </summary>
    /// <returns>a list of all information messages</returns>
    List<string> GetVerboseLogs();
}