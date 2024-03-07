using transaction_api.Models;

namespace transaction_api.Interfaces
{
    public interface ITransactionTypeRepository
    {
        public Task<TransactionType> GetTransactionTypeByIDAsync(short id);

        public Task<IEnumerable<TransactionType>> GetTransactionTypesAsync();
    }
}
