using Profilum.AccountService.BLL.Handlers.Interfaces;
using Profilum.AccountService.BLL.Models;
using Profilum.AccountService.Common;
using Profilum.AccountService.DAL.MessageBrokers;

namespace Profilum.AccountService.Api;

internal class KafkaConsumerServer : IHostedService
{
    private readonly ILogger<KafkaConsumerServer> _logger;
    private readonly IAccountHandler _accountHandler;
    private readonly MessageBus _messageBus;

    public KafkaConsumerServer(IAccountHandler accountHandler, ILogger<KafkaConsumerServer> logger, AppSettings settings)
    {
        _accountHandler = accountHandler;
        _logger = logger;
        _messageBus = new MessageBus(settings);
    }
    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var subscribeOnTopic = _messageBus.SubscribeOnTopic<string>(msg =>
            _accountHandler.Create(new AccountRequest
            {
                UserId = Guid.Parse(msg),
                AccountNumber = Guid.NewGuid().ToString()
            }) , cancellationToken);
        if(!subscribeOnTopic.IsSuccess)
            _logger.LogInformation($"{subscribeOnTopic.ResultCode} >> {subscribeOnTopic.LastResultMessage}");
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(KafkaConsumerServer)} stopped");
        
        return Task.CompletedTask;
    }
}