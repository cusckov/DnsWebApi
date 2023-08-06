using DnsWebApi.Services.DatabaseStrategy.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace DnsWebApi.Services.DatabaseStrategy.Strategies
{
    public class MsSqlDatabase : IDatabaseStrategy
    {
        private readonly string connectionString;
        private readonly ILogger<MsSqlDatabase> logger;

        public string Name => nameof(MsSqlDatabase);

        public MsSqlDatabase(IConfiguration configuration,
            ILogger<MsSqlDatabase> logger)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.logger = logger;
        }

        public async Task<DbConnection> GetDbConnection()
        {
            var connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();

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

        public async Task<SqlResult<bool>> ExecuteSqlCommand(string sqlText,
            IDictionary<string, object> sqlParams)
        {
            try
            {
                using (var connection = await GetDbConnection())
                {
                    using (var dbTransaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted))
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
                                    var parameter = command.CreateParameter();
                                    parameter.ParameterName = key;
                                    parameter.Value = sqlParams[key];
                                    command.Parameters.Add(parameter);
                                }
                            }

                            await command.ExecuteNonQueryAsync();

                            await dbTransaction.CommitAsync();

                            return new SqlResult<bool>
                            {
                                Result = true,
                                ErrorMessage = null
                            };
                        }
                        catch (Exception ex)
                        {
                            await dbTransaction.RollbackAsync();

                            return new SqlResult<bool>
                            {
                                Result = false,
                                ErrorMessage = ex.ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new SqlResult<bool>
                {
                    Result = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }

        public async Task<SqlResult<bool>> ExecuteProcedure(string sqlText,
            IDictionary<string, object> sqlParams)
        {
            try
            {
                using (var connection = await GetDbConnection())
                {
                    using (var dbTransaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            DbCommand command = connection.CreateCommand();

                            command.Transaction = dbTransaction;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = sqlText;

                            if (sqlParams != null)
                            {
                                foreach (string key in sqlParams.Keys)
                                {
                                    var parameter = command.CreateParameter();
                                    parameter.ParameterName = key;
                                    parameter.Value = sqlParams[key];
                                    command.Parameters.Add(parameter);
                                }
                            }

                            await command.ExecuteNonQueryAsync();

                            await dbTransaction.CommitAsync();

                            return new SqlResult<bool>
                            {
                                Result = true,
                                ErrorMessage = null
                            };
                        }
                        catch (Exception ex)
                        {
                            await dbTransaction.RollbackAsync();

                            return new SqlResult<bool>
                            {
                                Result = false,
                                ErrorMessage = ex.ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new SqlResult<bool>
                {
                    Result = false,
                    ErrorMessage = ex.ToString()
                };
            }
        }

        public async Task<SqlResult<IDictionary<int, IDictionary<string, object>>>> SelectDataFromProcedure(string sqlText,
            IDictionary<string, object> sqlParams)
        {
            Dictionary<int, IDictionary<string, object>> dictionary1 =
                new Dictionary<int, IDictionary<string, object>>();
            try
            {
                using (var connection = await GetDbConnection())
                {
                    using (var dbTransaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            var command = connection.CreateCommand();
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
                            using (DbDataReader dbDataReader = await command.ExecuteReaderAsync())
                            {
                                int num = 0;

                                while (dbDataReader.HasRows)
                                {
                                    while (await dbDataReader.ReadAsync())
                                    {
                                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                                        for (int ordinal = 0; ordinal < dbDataReader.FieldCount; ++ordinal)
                                            dictionary2.Add(dbDataReader.GetName(ordinal), dbDataReader[ordinal]);
                                        dictionary1.Add(num++, dictionary2);
                                    }

                                    await dbDataReader.NextResultAsync();
                                }
                                dbDataReader.Close();
                            }
                            await dbTransaction.CommitAsync();

                            return new SqlResult<IDictionary<int, IDictionary<string, object>>>
                            {
                                Result = dictionary1,
                                ErrorMessage = null
                            };
                        }
                        catch (Exception ex)
                        {
                            await dbTransaction.RollbackAsync();

                            return new SqlResult<IDictionary<int, IDictionary<string, object>>>
                            {
                                Result = dictionary1,
                                ErrorMessage = ex.ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new SqlResult<IDictionary<int, IDictionary<string, object>>>
                {
                    Result = dictionary1,
                    ErrorMessage = ex.ToString()
                };
            }
        }

        public async Task<SqlResult<IDictionary<int, IDictionary<string, object>>>> SelectData(string sqlText,
            IDictionary<string, object> sqlParams)
        {
            Dictionary<int, IDictionary<string, object>> dictionary1 =
                new Dictionary<int, IDictionary<string, object>>();
            try
            {
                using (var connection = await GetDbConnection())
                {
                    using (var dbTransaction = await connection.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            var command = connection.CreateCommand();
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
                            using (DbDataReader dbDataReader = await command.ExecuteReaderAsync())
                            {
                                int num = 0;

                                while (dbDataReader.HasRows)
                                {
                                    while (await dbDataReader.ReadAsync())
                                    {
                                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                                        for (int ordinal = 0; ordinal < dbDataReader.FieldCount; ++ordinal)
                                            dictionary2.Add(dbDataReader.GetName(ordinal), dbDataReader[ordinal]);
                                        dictionary1.Add(num++, dictionary2);
                                    }

                                    await dbDataReader.NextResultAsync();
                                }
                                dbDataReader.Close();
                            }
                            await dbTransaction.CommitAsync();

                            return new SqlResult<IDictionary<int, IDictionary<string, object>>>
                            {
                                Result = dictionary1,
                                ErrorMessage = null
                            };
                        }
                        catch (Exception ex)
                        {
                            await dbTransaction.RollbackAsync();

                            return new SqlResult<IDictionary<int, IDictionary<string, object>>>
                            {
                                Result = dictionary1,
                                ErrorMessage = ex.ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new SqlResult<IDictionary<int, IDictionary<string, object>>>
                {
                    Result = dictionary1,
                    ErrorMessage = ex.ToString()
                };
            }
        }
    }
}
