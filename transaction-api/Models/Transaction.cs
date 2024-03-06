namespace transaction_api.Models
{
    public class Transaction
    {
        public long TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; } = string.Empty;

        public short TransactionTypeID { get; set; }
        public required TransactionType TransactionType { get; set; }

        public int ClientID { get; set; }
        public required Client Client { get; set; }
    }

}
