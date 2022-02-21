using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using Profilum.AccountService.Api.AutoFacModules;
using Profilum.AccountService.Common;

var builder = WebApplication.CreateBuilder(args);
var _grpcPortDefault = 7001;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b =>
    b.RegisterConfiguredModulesFromAssemblyContaining<DiModule>(new AppSettings
    {
        ConnectionString =  builder.Configuration.GetSection("PostgresConnection:ConnectionString").Value,
        Database = builder.Configuration.GetSection("PostgresConnection:Database").Value,
        KafkaServer = builder.Configuration.GetSection("Application:KafkaServerAddress").Value,
        AccountKafkaTopic = builder.Configuration.GetSection("Application:KafkaServerAccountServiceTopic").Value,
        AccountGrpcServerPort = int.TryParse(builder.Configuration.GetSection("Application:GrpcServerPort")?.Value, out var grpcPort)
            ? grpcPort
            : _grpcPortDefault
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();