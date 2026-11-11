using Joseco.Communication.External.RabbitMQ.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace GestionClinicaNutricional.WorkerService
{
    public sealed class DefaultQueueRegistrar : BackgroundService
    {
        private const int DefaultPort = 5672;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(10);
        private const bool ExchangesAreDurable = false;
        private const bool ExchangesAutoDelete = false;
        private const bool QueueIsDurable = true;
        private static readonly IDictionary<string, object?> NoArguments = new Dictionary<string, object?>();

        private readonly RabbitMqSettings _settings;
        private readonly ILogger<DefaultQueueRegistrar> _logger;

        public DefaultQueueRegistrar(RabbitMqSettings settings, ILogger<DefaultQueueRegistrar> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public const string QueueName = "gestion-clinica-nutricional.default";

        public static IReadOnlyList<string> BoundExchanges { get; } = new[]
        {
            "paciente-published"
        };

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DeclareTopologyAsync(stoppingToken);
                    return;
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(
                        exception,
                        "Could not declare queue {Queue} bound to {ExchangeCount} exchange(s). Retrying in {RetryDelay}.",
                        QueueName,
                        BoundExchanges.Count,
                        RetryDelay);
                }

                try
                {
                    await Task.Delay(RetryDelay, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
        }

        private async Task DeclareTopologyAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_settings.Host)
                || string.IsNullOrWhiteSpace(_settings.UserName)
                || string.IsNullOrWhiteSpace(_settings.Password)
                || string.IsNullOrWhiteSpace(_settings.VirtualHost))
            {
                throw new InvalidOperationException(
                    "RabbitMqSettings must define Host, UserName, Password and VirtualHost before the " +
                    $"'{QueueName}' queue can be declared.");
            }

            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port > 0 ? _settings.Port : DefaultPort,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            await using var connection = await factory.CreateConnectionAsync("gestion-clinica-default-queue", cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                QueueName,
                durable: QueueIsDurable,
                exclusive: false,
                autoDelete: false,
                arguments: NoArguments,
                cancellationToken: cancellationToken);

            foreach (var exchange in BoundExchanges)
            {
                await channel.ExchangeDeclareAsync(
                    exchange,
                    ExchangeType.Fanout,
                    durable: ExchangesAreDurable,
                    autoDelete: ExchangesAutoDelete,
                    arguments: NoArguments,
                    cancellationToken: cancellationToken);

                await channel.QueueBindAsync(
                    QueueName,
                    exchange,
                    routingKey: string.Empty,
                    arguments: NoArguments,
                    cancellationToken: cancellationToken);
            }

            _logger.LogInformation(
                "Queue {Queue} is declared and bound to {ExchangeCount} exchange(s): {Exchanges}.",
                QueueName,
                BoundExchanges.Count,
                string.Join(", ", BoundExchanges));
        }
    }
}