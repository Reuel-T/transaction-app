namespace transaction_api.DTOs
{
    public class UpdateTransactionCommentDTO
    {
        public long TransactionID { get; set; }
        public string Comment { get; set; }
    }
}
