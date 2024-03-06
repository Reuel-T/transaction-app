using System.ComponentModel.DataAnnotations;
using transaction_api.Models;

namespace transaction_api.DTOs
{
    public class UpdateTransactionDTO
    {
        [Required(ErrorMessage = "Transaction ID is Required")]
        public long TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public short TransactionTypeID { get; set; }
        public int ClientID { get; set; }
    }
}
