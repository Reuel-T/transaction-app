using Dapper;
using transaction_api.Context;
using transaction_api.Interfaces;
using static transaction_api.Constants.Constants;

namespace transaction_api.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(DapperContext context, ILogger<TransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Deletes transactions associated with a specific client.
        /// </summary>
        /// <param name="clientID">The ID of the client for whom transactions should be deleted.</param>
        /// <returns>The number of rows affected (deleted) from the Transactions table.</returns>
        public async Task<int> DeleteTransactionsForClient(int clientID)
        {
            // Construct the SQL query for deleting transactions for the specified client
            string query = $@"DELETE FROM {Tables.Transaction} WHERE {TransactionFields.ClientID} = @Id";

            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();

            // Execute the DELETE query asynchronously and capture the number of rows affected
            int numberDeleted = await connection.ExecuteAsync(query, new{ Id=clientID });

            // Log information about the deletion if any rows were affected
            if (numberDeleted > 0)
            {
                _logger.Log(LogLevel.Information, $"Deleted {numberDeleted} rows from {Tables.Transaction} for ClientID {clientID} at {DateTime.UtcNow}");
            }

            // Return the number of rows affected
            return numberDeleted;
        }
    }
}
