namespace transaction_api.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<int> DeleteTransactionsForClient(int clientID);
    }
}
