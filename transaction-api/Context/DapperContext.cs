using Dapper;
using System.Data;
using System.Data.SqlClient;
using transaction_api.Interfaces;
using static transaction_api.Constants.Constants;

namespace transaction_api.Context
{
    /// <summary>
    /// Represents a context for managing Dapper connections.
    /// </summary>
    public class DapperContext : IDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DapperContext> _logger;
        private readonly string _connectionString;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperContext"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing connection details.</param>
        /// <exception cref="Exception">Thrown when the connection string is not found in the configuration.</exception>
        public DapperContext (IConfiguration configuration, ILogger<DapperContext> logger)
        {
            _logger = logger;
            _configuration = configuration;

            string? connectionString;

            if (Environment.GetEnvironmentVariable("ISDOCKER") != null)
            {
                connectionString = _configuration.GetConnectionString(DockerConnection);
                _logger.Log(LogLevel.Information, "Using Docker Database");
            }else
            {
                connectionString = _configuration.GetConnectionString(DefaultConnection);
                _logger.Log(LogLevel.Information, "Using Windows Database");
            }

            if (connectionString != null)
            {
                _connectionString = connectionString;
            }
            else
            {
                throw new Exception("Connection String not Found");
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="IDbConnection"/> representing a connection to the database.
        /// </summary>
        /// <returns>An instance of <see cref="IDbConnection"/>.</returns>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using var connection = this.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
