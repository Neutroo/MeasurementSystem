using InfluxDB.Client;
using System.Data.Common;

namespace MeasurementSystemWebAPI.Contexts
{
    public class DeviceContext
    {
        public InfluxDBClient InfluxDBClient { get; }
        public string Bucket { get; }
        public string Org { get; }

        public DeviceContext()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = configuration.GetConnectionString("InfluxDBConnection")
            };

            string host = connectionStringBuilder["Host"].ToString() 
                ?? throw new ArgumentNullException("Host cannot be null. Please check appsettings.json.");
            string token = connectionStringBuilder["Token"].ToString() 
                ?? throw new ArgumentNullException("Token cannot be null. Please check appsettings.json.");
            Bucket = connectionStringBuilder["Bucket"].ToString() 
                ?? throw new ArgumentNullException("Bucket cannot be null. Please check appsettings.json.");
            Org = connectionStringBuilder["Org"].ToString() 
                ?? throw new ArgumentNullException("Org cannot be null. Please check appsettings.json.");

            InfluxDBClient = new InfluxDBClient(host, token);
        }
    }
}
