using System.Text;
using System.Text.Json;
using grades_mvc.Data;
using grades_mvc.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace grades_mvc.Messaging;

public class StudentEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StudentEventConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public StudentEventConsumer(IServiceProvider serviceProvider, ILogger<StudentEventConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var config = new RabbitMqConfig();
        using var scope = _serviceProvider.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        configuration.GetSection("RabbitMQ").Bind(config);

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = config.Host,
                Port = config.Port,
                UserName = config.Username,
                Password = config.Password,
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

            var queueName = _channel.QueueDeclareAsync().GetAwaiter().GetResult().QueueName;

            _channel.QueueBindAsync(
                queue: queueName,
                exchange: RabbitMqExchange.StudentExchange,
                routingKey: string.Empty
            ).GetAwaiter().GetResult();

            _logger.LogInformation("RabbitMQ consumer connected to {Host}:{Port}", config.Host, config.Port);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ. Will retry...");
        }

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_channel is null || !_channel.IsOpen)
            {
                await Task.Delay(5000, stoppingToken);
                continue;
            }

            try
            {
                var queueName = _channel.QueueDeclareAsync().GetAwaiter().GetResult().QueueName;

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    try
                    {
                        var studentEvent = JsonSerializer.Deserialize<StudentEvent>(json, JsonOptions);
                        if (studentEvent is not null)
                        {
                            await ProcessEvent(studentEvent);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing student event");
                    }
                };

                _channel.BasicConsumeAsync(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer
                ).GetAwaiter().GetResult();

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Consumer error, retrying in 5s...");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private async Task ProcessEvent(StudentEvent studentEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GradesDbContext>();

        switch (studentEvent.EventType)
        {
            case StudentEventTypes.Created:
            case StudentEventTypes.Updated:
                var existing = await context.StudentCache.FindAsync(studentEvent.StudentId);
                if (existing is not null)
                {
                    existing.FirstName = studentEvent.FirstName;
                    existing.LastName = studentEvent.LastName;
                    existing.Email = studentEvent.Email;
                    existing.DateOfBirth = studentEvent.DateOfBirth;
                    existing.EnrollmentDate = studentEvent.EnrollmentDate;
                    existing.LastUpdated = DateTime.UtcNow;
                }
                else
                {
                    context.StudentCache.Add(new StudentCache
                    {
                        Id = studentEvent.StudentId,
                        FirstName = studentEvent.FirstName,
                        LastName = studentEvent.LastName,
                        Email = studentEvent.Email,
                        DateOfBirth = studentEvent.DateOfBirth,
                        EnrollmentDate = studentEvent.EnrollmentDate,
                        LastUpdated = DateTime.UtcNow
                    });
                }

                await context.SaveChangesAsync();
                _logger.LogInformation("Cached student {StudentId} ({EventType})", studentEvent.StudentId, studentEvent.EventType);
                break;

            case StudentEventTypes.Deleted:
                var student = await context.StudentCache.FindAsync(studentEvent.StudentId);
                if (student is not null)
                {
                    context.StudentCache.Remove(student);
                    await context.SaveChangesAsync();
                    _logger.LogInformation("Removed student {StudentId} from cache", studentEvent.StudentId);
                }
                break;
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
