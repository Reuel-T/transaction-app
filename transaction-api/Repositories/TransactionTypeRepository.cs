using Dapper;
using transaction_api.Context;
using transaction_api.Interfaces;
using transaction_api.Models;
using static transaction_api.Constants.Constants;

namespace transaction_api.Repositories
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionTypeRepository(DapperContext context, ILogger<TransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TransactionType> GetTransactionTypeByIDAsync(short id)
        {
            string query = $@"
            SELECT 
                {TransactionTypeFields.TransactionTypeID}, 
                {TransactionTypeFields.TransactionTypeName} 
            FROM {Tables.TransactionType} 
            WHERE
                {TransactionTypeFields.TransactionTypeID} = @id
            ";
       
            using var connection = _context.CreateConnection();
     
            var transactionType = await connection.QuerySingleOrDefaultAsync<TransactionType>(query, new { id });

            return transactionType;
        }
    }
}
