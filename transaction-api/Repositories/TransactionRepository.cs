using Dapper;
using transaction_api.Context;
using transaction_api.DTOs;
using transaction_api.Interfaces;
using System.Data;
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

        public async Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO transactionDTO)
        {
            //opens connection and begins transaction
            using var connection = _context.CreateConnection();
            try
            {
                //query params
                var parameters = new DynamicParameters();
                parameters.Add("Amount", transactionDTO.Amount, DbType.Decimal);
                parameters.Add("Comment", transactionDTO.Comment, DbType.String);
                parameters.Add("TransactionTypeID", transactionDTO.TransactionTypeID, DbType.Int64);
                parameters.Add("ClientID", transactionDTO.ClientID, DbType.Int64);

                long newTransactionID = await connection.QueryFirstAsync<long>(
                    StoredProcedures.CreateTransaction, parameters, commandType: CommandType.StoredProcedure);

                _logger.Log(LogLevel.Information, $"Created new Transaction {newTransactionID}(ID) and updated balance For Client {transactionDTO.ClientID}(ID) with at {DateTime.UtcNow}.");

                //return the new transaction
                return new TransactionDTO
                {
                    TransactionID = newTransactionID,
                    Amount = transactionDTO.Amount,
                    ClientID = transactionDTO.ClientID,
                    Comment = transactionDTO.Comment,
                    TransactionTypeID = transactionDTO.TransactionTypeID,
                };
            }
            catch (Exception)
            {
                _logger.Log(LogLevel.Error, $"Unable to Create Transaction. Rolling Back Transaction. No Changes have been made)");
                throw;
            }
        }

        public async Task<Models.Transaction> GetTransactionAsync(long TransactionID)
        {
            // Construct the SQL query for retrieving a client by ID
            string query = $@"
                SELECT * FROM {Tables.Transaction}
                WHERE {TransactionFields.TransactionID} = @Id
            ";

            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();
            // Execute the query asynchronously and retrieve a single client, or null if not found
            var transaction = await connection.QuerySingleOrDefaultAsync<Models.Transaction>(query, new { Id = TransactionID });

            // Return the retrieved client
            return transaction;
        }

        /// <summary>
        /// Retrieves transactions for a client based on the provided client ID.
        /// </summary>
        /// <param name="ClientID">The ID of the client for whom transactions are to be retrieved.</param>
        /// <returns>
        /// A collection of DTOs containing transaction details for the specified client.
        /// </returns>
        public async Task<IEnumerable<ClientTransactionDTO>> GetTransactionsForClientAsync(int ClientID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ClientID", ClientID, DbType.Int64);


            using var connection = _context.CreateConnection();
            var clientTransactions = await connection.QueryAsync<ClientTransactionDTO>(
                StoredProcedures.GetClientTransactions,
                parameters,
                commandType: CommandType.StoredProcedure);

            return clientTransactions.ToList();
        }

        public async Task<bool> UpdateCommentForTransactionAsync(long TransactionID, UpdateTransactionDTO transaction)
        {
            //Update Query
            string query = $@"
                UPDATE {Tables.Transaction} 
                SET 
                    {TransactionFields.Comment} = @Comment 
                WHERE {TransactionFields.TransactionID} = @TransactionID
            ";

            //Query Params
            var queryParams = new DynamicParameters();
            queryParams.Add("Comment", transaction.Comment, DbType.String);
            queryParams.Add("TransactionID", TransactionID, DbType.Int64);

            //Run Query
            using var connection = _context.CreateConnection();
            int affectedRows = await connection.ExecuteAsync(query, queryParams);

            //logs update info if successful
            if (affectedRows > 0)
            {
                _logger.Log(LogLevel.Information, $"Updated Transaction {TransactionID} Comment to {transaction.Comment} at {DateTime.UtcNow} ");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
