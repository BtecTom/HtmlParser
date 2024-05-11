namespace SchemaManager;

public static class SchemaManager
{
    /// <summary>
    /// Running this will automatically publish a new version of the schema
    /// </summary>
    public static void Main()
    {
        var version = Schema.GetSchemaVersion();

        Console.WriteLine($@"publishing version {version} of the parser schema");
        Schema.PublishSchema();
        Console.WriteLine(@"Published");
        Console.WriteLine();
    }
}