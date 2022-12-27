namespace Starfish.Web.Extensions;

public static class LoggerMessageDefinitions
{
    private static readonly Action<ILogger, string, Exception?> DatabaseConnectionErrorDefinition =
        LoggerMessage.Define<string>(LogLevel.Error, 0, "Database connection error: {message}");

    public static void DatabaseConnectionError(this ILogger logger, string message)
    {
        DatabaseConnectionErrorDefinition(logger, message, null);
    }
}