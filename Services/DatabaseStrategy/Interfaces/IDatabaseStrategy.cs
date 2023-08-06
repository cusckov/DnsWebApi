using System.Data.Common;

namespace DnsWebApi.Services.DatabaseStrategy.Interfaces
{
    public interface IDatabaseStrategy
    {
        string Name { get; }
        DbConnection GetDbConnection();
        bool ExecuteSqlCommand(string sqlText,
          IDictionary<string, object> sqlParams,
          out string errorMessage);
        IDictionary<int, IDictionary<string, object>> SelectData(
          string sqlText,
          IDictionary<string, object> sqlParams,
          out string errorMessage);
    }
}
