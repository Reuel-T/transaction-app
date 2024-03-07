using Swashbuckle.AspNetCore.Annotations;
using transaction_api.Models;

namespace transaction_api.DTOs
{

    public class TransactionDTO
    {
        public long TransactionID { get; set; }
        [SwaggerSchema("The Transaction Amount")]
        public decimal Amount { get; set; }
        [SwaggerSchema("A Comment Associated with the Transaction")]
        public string Comment { get; set; } = string.Empty;
        [SwaggerSchema("Transaction Type ID - Refers to associated TransactionType")]
        public short TransactionTypeID { get; set; }
        [SwaggerSchema("ID of associated Client")]
        public int ClientID { get; set; }
    }
}
