using Grpc.Core;
using Profilum.AccountService.Common;

namespace Profilum.AccountService.Api;

internal class GrpcServer : IHostedService
{
    private readonly ILogger<GrpcServer> _logger;
    private readonly Server _server;

    public GrpcServer(AccountService.AccountServiceBase accountService, ILogger<GrpcServer> logger, AppSettings settings)
    {
        _logger = logger;
        _server = new Server
        {
            Ports =
            {
                new ServerPort("localhost", settings.AccountGrpcServerPort, ServerCredentials.Insecure)
            },
            Services =
            {
                AccountService.BindService(accountService)
            }
        };            
    }

    /// <summary>
    /// Вспомогательный метод генерации серверных кредов из сертификата
    /// </summary>
    private ServerCredentials BuildSSLCredentials()
    {
        var cert = File.ReadAllText("cert\\server.crt");
        var key = File.ReadAllText("cert\\server.key");

        var keyCertPair = new KeyCertificatePair(cert, key);
        return new SslServerCredentials(new[] { keyCertPair });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start GRPC Server");
        _server.Start();
        _logger.LogInformation("GRPC Server Started");
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
       _logger.LogInformation("Stopped GRPC server");
        await _server.ShutdownAsync();
        _logger.LogInformation("GRPC server is stopped");
    }
}