using Confluent.Kafka;
namespace kafka_produce.Services;

public class ProducerService : IProducerService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProducerService> _logger;
    private readonly IProducer<string, string> _producer;

    private const string TopicName = "consumer.new";

    public ProducerService(IConfiguration configuration, ILogger<ProducerService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            EnableDeliveryReports = true
        };

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }
    public async Task<DeliveryResult<string, string>> ProduceAsync(string message)
    {
        var rnd = new Random();
        var key = rnd.Next(1,6);

        var kafkaMessage = new Message<string, string>
        {
            Key = key.ToString(),
            Value = message
        };

        try
        {
            var deliveryResult = await _producer.ProduceAsync(TopicName, kafkaMessage);
            return deliveryResult;
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex.Error.Reason);
            throw;
        }
    }
}

public interface IProducerService
{
    Task<DeliveryResult<string, string>> ProduceAsync(string message);
}
