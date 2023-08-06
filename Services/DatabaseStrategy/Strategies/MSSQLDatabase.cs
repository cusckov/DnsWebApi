using DnsWebApi.Services.DatabaseStrategy.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace DnsWebApi.Services.DatabaseStrategy.Strategies
{
    public class MSSQLDatabase : IDatabaseStrategy
    {
        private readonly string connectionString;
        private readonly ILogger<MSSQLDatabase> logger;

        public string Name => nameof(MSSQLDatabase);

        public MSSQLDatabase(IConfiguration configuration,
            ILogger<MSSQLDatabase> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }

        public DbConnection GetDbConnection()
        {
            DbConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();

                if (connection.State == ConnectionState.Broken || connection.State == ConnectionState.Closed)
                    throw new Exception("Подключение повреждено или закрыто.");

                logger.LogInformation(string.Format("Успешное подключение к базе данных. Строка подключения: {0}", connectionString));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Ошибка при подключении базы данных: {0}. Строка подключения: {1}", ex, connectionString));
                throw;
            }

            return connection;

        }

        public bool ExecuteSqlCommand(string sqlText,
            IDictionary<string, object> sqlParams,
            out string errorMessage)
        {
            using (DbConnection connection = GetDbConnection())
            {
                using (DbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        DbCommand command = connection.CreateCommand();

                        command.Transaction = dbTransaction;

                        command.CommandText = sqlText;

                        if (sqlParams != null)
                        {
                            foreach (string key in sqlParams.Keys)
                            {
                                DbParameter parameter = command.CreateParameter();
                                parameter.ParameterName = key;
                                parameter.Value = sqlParams[key];
                                command.Parameters.Add(parameter);
                            }
                        }

                        command.ExecuteNonQuery();

                        dbTransaction.Commit();

                        errorMessage = "";

                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();

                        errorMessage = ex.ToString();

                        return false;
                    }
                }
            }
        }

        public IDictionary<int, IDictionary<string, object>> SelectData(string sqlText,
            IDictionary<string, object> sqlParams,
            out string errorMessage)
        {
            Dictionary<int, IDictionary<string, object>> dictionary1 =
                new Dictionary<int, IDictionary<string, object>>();

            using (DbConnection connection = GetDbConnection())
            {
                using (DbTransaction dbTransaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        DbCommand command = connection.CreateCommand();
                        command.Transaction = dbTransaction;
                        command.CommandText = sqlText;
                        if (sqlParams != null)
                        {
                            foreach (string key in (IEnumerable<string>)sqlParams.Keys)
                            {
                                DbParameter parameter = command.CreateParameter();
                                parameter.ParameterName = key;
                                parameter.Value = sqlParams[key];
                                command.Parameters.Add(parameter);
                            }
                        }
                        using (DbDataReader dbDataReader = command.ExecuteReader())
                        {
                            int num = 0;
                            while (dbDataReader.HasRows)
                            {
                                while (dbDataReader.Read())
                                {
                                    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                                    for (int ordinal = 0; ordinal < dbDataReader.FieldCount; ++ordinal)
                                        dictionary2.Add(dbDataReader.GetName(ordinal), dbDataReader[ordinal]);
                                    dictionary1.Add(num++, dictionary2);
                                }
                                dbDataReader.NextResult();
                            }
                            dbDataReader.Close();
                        }
                        dbTransaction.Commit();
                        errorMessage = "";
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        errorMessage = ex.ToString();
                    }
                }
                return dictionary1;
            }
        }
    }
}
