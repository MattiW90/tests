namespace Day10_NotificationSystem;

internal class Program
{
    static void Main(string[] args)
    {
        var listNotifiable = new List<INotifiable>()
         {
             new EmailNotification("abc@abc.com"),
             new SmsNotification("777-777-777"),
             new PushNotification("galaxys24"),
             new ConsoleLogger()
         };
        var listLoggable = new List<ILoggable>()
        {
            new FileLogger("c://test/test.abc"),
            new DatabaseLogger("(localdb)\\mssqllocaldb"),
            new ConsoleLogger()
        };
        Console.WriteLine("=== SENDING NOTIFICATIONS ===");
        foreach (var item in listNotifiable)
        {
            item.SendNotification("System update available");
        }

        Console.WriteLine("=== LOGGING ===");
        foreach (var item in listLoggable)
        {
            item.Log("User logged in");
        }

        Console.WriteLine($"Count of list Notifiable: {listNotifiable.Count}");
        Console.WriteLine($"Count of list loggable: {listLoggable.Count}");

        Console.WriteLine("CONSOLE LOGGER DEMO");
        var consoleLogger = new ConsoleLogger();
        consoleLogger.SendNotification("Console logger notification test");
        consoleLogger.Log("Console logger log test");
        Console.WriteLine("Same object implements both interfaces");
    }
}

public interface INotifiable
{
    void SendNotification(string message);
}

public interface ILoggable
{
    void Log(string message);
}

public class EmailNotification : INotifiable
{
    public string EmailAddress { get; }
    public EmailNotification(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new ArgumentException("Email address cannot be null or empty");
        EmailAddress = emailAddress;
    }
    public void SendNotification(string message)
    {
        Console.WriteLine($"Email to adres: {EmailAddress}: {message}");
    }
}

public class SmsNotification : INotifiable
{
    public string PhoneNumber { get;  }
    public SmsNotification(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty");
        PhoneNumber = phoneNumber;
    }
    public void SendNotification(string message)
    {
        Console.WriteLine($"Sms to: {PhoneNumber}: {message}");
    }
}

public class PushNotification : INotifiable
{
    public string DeviceId { get; }
    public PushNotification(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("DeviceId cannot be null or empty");
        DeviceId = deviceId;
    }
    public void SendNotification(string message)
    {
        Console.WriteLine($"Push to DeviceId: {DeviceId}: {message}");
    }
}

public class FileLogger : ILoggable
{
    public string FilePath { get; }
    public FileLogger(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("FilePath cannot be null or empty");
        FilePath = filePath;
    }
    public void Log(string message)
    {
        Console.WriteLine($"Logged to {FilePath}: {message}");
    }
}

public class DatabaseLogger : ILoggable
{
    public string ConnectionString { get; }
    public DatabaseLogger(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string cannot be empty or null");
        ConnectionString = connectionString;
    }
    public void Log(string message)
    {
        Console.WriteLine($"Logged to database: {ConnectionString}: {message}");
    }
}

public class ConsoleLogger : ILoggable, INotifiable
{
    
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }

    public void SendNotification(string message)
    {
        Console.WriteLine($"[NOTIFICATION] {message}");
    }
}