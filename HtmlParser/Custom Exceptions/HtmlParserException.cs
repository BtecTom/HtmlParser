namespace HtmlParser.Custom_Exceptions;

/// <summary>
/// Thrown when an error in parsing Html data is encountered
/// </summary>
public abstract class HtmlParserException : Exception
{
    /// <summary>
    /// Throw when an error in parsing Html data is encountered
    /// </summary>
    protected HtmlParserException() : base()
    {
    }

    /// <summary>
    /// Throw when an error in parsing Html data is encountered
    /// </summary>
    protected HtmlParserException(string message) : base(message)
    {
    }

    /// <summary>
    /// Throw when an error in parsing Html data is encountered
    /// </summary>
    protected HtmlParserException(string message, Exception innerException) : base(message, innerException)
    {
    }
}