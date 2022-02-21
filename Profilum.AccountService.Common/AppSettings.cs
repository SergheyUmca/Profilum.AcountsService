namespace Profilum.AccountService.Common
{
    public class AppSettings
    {
        public string ConnectionString;
        
        public string Database;

        public int AccountGrpcServerPort;

        public int UserGrpcServerPort;

        public string KafkaServer;

        public string AccountKafkaTopic;
    }
}
