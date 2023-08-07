using DnsWebApi.Services.DatabaseStrategy.Interfaces;

namespace DnsWebApi.Services.DatabaseStrategy
{
    public class DatabaseStrategyContext
    {
        private readonly IEnumerable<IDatabaseStrategy> dbStrategies;
        private readonly IConfiguration configuration;

        public DatabaseStrategyContext(IEnumerable<IDatabaseStrategy> dbStrategies, IConfiguration configuration)
        {
            this.dbStrategies = dbStrategies;
            this.configuration = configuration;
        }

        public IDatabaseStrategy GetStrategy()
        {
            if (dbStrategies is null)
                throw new NullReferenceException(nameof(dbStrategies));

            return dbStrategies.SingleOrDefault(item => item.Name == configuration["DbName"]);
        }
    }
}
