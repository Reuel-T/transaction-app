using transaction_api.DTOs;
using transaction_api.Models;

namespace transaction_api.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO transactionDTO);
        public Task<Transaction> GetTransactionAsync(long TransactionID);
        public Task<IEnumerable<TransactionDTO>> GetTransactionsForClientAsync(int ClientID);
        public Task<bool> UpdateCommentForTransactionAsync(long TransactionID, UpdateTransactionDTO transaction);
    }
}
