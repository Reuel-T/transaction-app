namespace transaction_api.DTOs
{
    public class ClientTransactionDTO
    {
        public long TransactionID { get; set; }
        public string TransactionTypeName { get; set; }
        public string Comment { get; set; }
        public int TransactionTypeID { get; set; }
        public decimal Amount { get; set; }
        public int ClientID { get; set; }
    }
}
