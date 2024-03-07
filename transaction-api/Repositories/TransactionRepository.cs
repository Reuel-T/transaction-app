using Dapper;
using transaction_api.Context;
using transaction_api.DTOs;
using transaction_api.Interfaces;
using transaction_api.Models;
using System.Data;
using static transaction_api.Constants.Constants;
using System.Transactions;

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
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                //create transaction query
                string transactionQuery = $@"
                INSERT INTO {Tables.Transaction} (
                    {TransactionFields.Amount}, 
                    {TransactionFields.Comment},
                    {TransactionFields.TransactionTypeID},
                    {TransactionFields.ClientID} 
                )
                VALUES (@Amount, @Comment, @TransactionTypeID, @ClientID);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);
                ";

                //params for adding new transaction
                var transactionQueryParams = new DynamicParameters();
                transactionQueryParams.Add("Amount", transactionDTO.Amount, DbType.Decimal);
                transactionQueryParams.Add("Comment", transactionDTO.Comment, DbType.String);
                transactionQueryParams.Add("TransactionTypeID", transactionDTO.TransactionTypeID, DbType.Int64);
                transactionQueryParams.Add("ClientID", transactionDTO.ClientID, DbType.Int64);

                //getting ID of new transaction
                long newTransactionID = await connection.ExecuteScalarAsync<long>(transactionQuery, transactionQueryParams, transaction);

                //updating the client's balance
                string updateClientQuery = $@"
                UPDATE {Tables.Client} 
                SET {ClientFields.ClientBalance} = {ClientFields.ClientBalance} + @Amount 
                WHERE {ClientFields.ClientID} = @ClientID;
                ";

                //params for updating client balance
                var updateBalanceQueryParams = new DynamicParameters();
                updateBalanceQueryParams.Add("Amount", transactionDTO.Amount, DbType.Decimal);
                updateBalanceQueryParams.Add("ClientID", transactionDTO.ClientID, DbType.Int64);
            
                await connection.ExecuteAsync(updateClientQuery, updateBalanceQueryParams, transaction);

                _logger.Log(LogLevel.Information, $"Created new Transaction {newTransactionID}(ID) and updated balance For Client {transactionDTO.ClientID}(ID) with at {DateTime.UtcNow}.");

                transaction.Commit();

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
                _logger.Log(LogLevel.Error, $"Unable to Create Transaction Client. Rolling Back Transaction)");
                transaction.Rollback();
                connection.Close();
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
            //Join Query, aliases fields and joins 
            //Just three tables with required fields

            string query = $@"
                SELECT 
                    T.{TransactionFields.TransactionID}, 
                    C.{ClientFields.Name}, 
                    C.{ClientFields.Surname}, 
                    T.{TransactionFields.Amount}, 
                    T.{TransactionFields.Comment}, 
                    TT.{TransactionTypeFields.TransactionTypeName}, 
                    TT.{TransactionTypeFields.TransactionTypeID} 
                FROM {Tables.Client} C 
                JOIN {Tables.Transaction} T ON C.{ClientFields.ClientID} = T.{TransactionFields.ClientID} 
                JOIN {Tables.TransactionType} TT ON T.{TransactionFields.TransactionTypeID} = TT.{TransactionTypeFields.TransactionTypeID} 
                WHERE T.{TransactionFields.ClientID} = @ClientID
            ";

            using var connection = _context.CreateConnection();
            var clientTransactions = await connection.QueryAsync<ClientTransactionDTO>(query, new { ClientID });

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
