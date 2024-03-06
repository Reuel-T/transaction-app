using transaction_api.DTOs;
using transaction_api.Models;

namespace transaction_api.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<Transaction> GetTransactionAsync(long TransactionID);
        public Task<IEnumerable<ClientTransactionDTO>> GetTransactionsForClient(int ClientID);
        public Task<bool> UpdateCommentForTransactionAsync(long TransactionID, UpdateTransactionDTO transaction);
    }
}
