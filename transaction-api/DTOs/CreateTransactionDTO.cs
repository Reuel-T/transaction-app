using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace transaction_api.DTOs
{
    public class CreateTransactionDTO
    {
        [Required]
        [SwaggerSchema(Description = "Transaction amount", Format = "number")]
        public decimal Amount { get; set; }

        [SwaggerSchema(Description = "Comment on associated transaction", Format = "string")]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [SwaggerSchema("ID of associated Transaction", Format = "number")]
        public short TransactionTypeID { get; set; }

        [Required]
        [SwaggerSchema("ID of associated Client", Format = "number")]
        public int ClientID { get; set; }
    }
}
