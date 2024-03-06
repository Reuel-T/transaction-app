using Dapper;
using transaction_api.Context;
using transaction_api.DTOs;
using transaction_api.Interfaces;
using transaction_api.Models;
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

        public Task<Transaction> GetTransactionAsync(long TransactionID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves transactions for a client based on the provided client ID.
        /// </summary>
        /// <param name="ClientID">The ID of the client for whom transactions are to be retrieved.</param>
        /// <returns>
        /// A collection of DTOs containing transaction details for the specified client.
        /// </returns>
        public async Task<IEnumerable<ClientTransactionDTO>> GetTransactionsForClient(int ClientID)
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

        public async Task<bool> UpdateCommentForTransaction(long TransactionID, UpdateTransactionDTO transaction)
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

        public Task<bool> UpdateCommentForTransactionAsync(long TransactionID, UpdateTransactionDTO transaction)
        {
            throw new NotImplementedException();
        }
    }
}
