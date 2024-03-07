using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using transaction_api.Models;

namespace transaction_api.DTOs
{
    public class UpdateTransactionDTO
    {
        [Required(ErrorMessage = "Transaction ID is Required")]
        [SwaggerSchema(Description = "ID of Transaction", Format = "number")]
        public long TransactionID { get; set; }
        
        [SwaggerSchema(Description = "Transaction amount", Format = "number" )]
        public decimal Amount { get; set; }
        
        [SwaggerSchema(Description = "Comment on associated transaction", Format = "string")]
        public string Comment { get; set; }
        
        [SwaggerSchema("ID of associated Transaction", Format = "number")]
        public short TransactionTypeID { get; set; }
        
        [SwaggerSchema("ID of associated Client", Format = "number")]
        public int ClientID { get; set; }
    }
}
