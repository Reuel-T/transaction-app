using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using transaction_api.Interfaces;
using transaction_api.Models;

namespace transaction_api.Controllers
{
    [Route("api/transaction-types")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly ILogger<TransactionController> _logger;

        public TransactionTypeController(ITransactionTypeRepository transactionTypeRepository, ILogger<TransactionController> logger)
        {
            _transactionTypeRepository = transactionTypeRepository;
            _logger = logger;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<TransactionType>>> GetTransactions() 
        {
            try
            {
                var transactionTypes = await _transactionTypeRepository.GetTransactionTypesAsync();
                return Ok(transactionTypes);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        /// <summary>
        /// Logs the error and returns a 500 Internal Server Error response.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        /// <returns>An action result representing a 500 Internal Server Error response.</returns>
        private ObjectResult LogError(Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Error Type: {ex.GetType()}");
            _logger.Log(LogLevel.Debug, $"ErrorMessage: {ex.Message}");
            return StatusCode(500, "Server Error. Request not completed");
        }
    }
}
