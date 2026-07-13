using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using students_mvc.Models;

namespace students_mvc.Messaging;

public interface IStudentMessageBus
{
    Task PublishStudentEvent(Student student, string eventType);
}

public class RabbitMqStudentMessageBus : IStudentMessageBus
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqStudentMessageBus> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public RabbitMqStudentMessageBus(IConfiguration config, ILogger<RabbitMqStudentMessageBus> logger)
    {
        _logger = logger;

        var host = config["RabbitMQ:Host"] ?? "localhost";
        var port = int.Parse(config["RabbitMQ:Port"] ?? "5672");
        var username = config["RabbitMQ:Username"] ?? "guest";
        var password = config["RabbitMQ:Password"] ?? "guest";

        var factory = new ConnectionFactory
        {
            HostName = host,
            Port = port,
            UserName = username,
            Password = password,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(
            exchange: RabbitMqExchange.StudentExchange,
            type: ExchangeType.Fanout,
            durable: true
        ).GetAwaiter().GetResult();

        _logger.LogInformation("RabbitMQ publisher connected to {Host}:{Port}", host, port);
    }

    public async Task PublishStudentEvent(Student student, string eventType)
    {
        var studentEvent = new StudentEvent
        {
            EventType = eventType,
            StudentId = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth,
            EnrollmentDate = student.EnrollmentDate,
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(studentEvent, JsonOptions);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            exchange: RabbitMqExchange.StudentExchange,
            routingKey: string.Empty,
            body: body
        );

        _logger.LogInformation("Published {EventType} for student {StudentId}", eventType, student.Id);
    }
}
