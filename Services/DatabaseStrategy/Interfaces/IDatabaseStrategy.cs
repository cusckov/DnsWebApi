using DnsWebApi.Services.DatabaseStrategy.Strategies;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data.Common;

namespace DnsWebApi.Services.DatabaseStrategy.Interfaces
{
    public interface IDatabaseStrategy
    {
        string Name { get; }
        Task<DbConnection> GetDbConnection();
        Task<SqlResult<bool>> ExecuteSqlCommand(string sqlText,
          IDictionary<string, object> sqlParams);
        Task<SqlResult<bool>> ExecuteProcedure(string sqlText,
          IDictionary<string, object> sqlParams);
        Task<SqlResult<object>> ExecuteProcedureWithReturnParameter(string sqlText,
            IDictionary<string, object> sqlParams);
        Task<SqlResult<IDictionary<int, IDictionary<string, object>>>> SelectData(string sqlText,
          IDictionary<string, object> sqlParams);
        Task<SqlResult<IDictionary<int, IDictionary<string, object>>>> SelectDataFromProcedure(string sqlText,
          IDictionary<string, object> sqlParams);

        // TODO: Сделать еще пару методов с выходными параметрами
    }
}
