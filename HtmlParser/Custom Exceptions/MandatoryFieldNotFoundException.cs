namespace HtmlParser.Custom_Exceptions;

/// <summary>
/// Thrown when an error in getting a mandatory field is encountered
/// </summary>
public class MandatoryFieldNotFoundException : HtmlParserException
{
    /// <summary>
    /// Throw when an error in getting a mandatory field is encountered
    /// </summary>
    public MandatoryFieldNotFoundException() : base()
    {
    }

    /// <summary>
    /// Throw when an error in getting a mandatory field is encountered
    /// </summary>
    public MandatoryFieldNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Throw when an error in getting a mandatory field is encountered
    /// </summary>
    public MandatoryFieldNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}