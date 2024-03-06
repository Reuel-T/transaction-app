using System.Data;
using System.Data.SqlClient;
using static transaction_api.Constants.Constants;

namespace transaction_api.Context
{
    /// <summary>
    /// Represents a context for managing Dapper connections.
    /// </summary>
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperContext"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing connection details.</param>
        /// <exception cref="Exception">Thrown when the connection string is not found in the configuration.</exception>
        public DapperContext (IConfiguration configuration)
        {
            _configuration = configuration;

            string? connectionString = _configuration.GetConnectionString(DefaultConnection);

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

    }
}
