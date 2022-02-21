using Confluent.Kafka;
using Profilum.AccountService.Common;
using Profilum.AccountService.Common.BaseModels;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.DAL.MessageBrokers;

public sealed class MessageBus
{
    private readonly ConsumerConfig _consumerConfig;

    private readonly string _topicName;
    

    public MessageBus(AppSettings settings)
    {
        _topicName = settings.AccountKafkaTopic;
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.KafkaServer,
            GroupId = "custom-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            AllowAutoCreateTopics = true
        };
    }

    public Response SubscribeOnTopic<T>( Action<T> action, CancellationToken cancellationToken) where T: class
    {
        try
        {
            using (var consumer = new ConsumerBuilder<Ignore, T>(_consumerConfig).Build())
            {
                consumer.Subscribe(_topicName);
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                
                    var consumeResult = consumer.Consume(cancellationToken);
                    if (consumeResult.Message is { Value: { } message })
                        action(message);
                    else
                    {
                        
                    }
                }

                consumer.Close();
            }

            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
}