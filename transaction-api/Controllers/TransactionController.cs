using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using transaction_api.DTOs;
using transaction_api.Interfaces;
using transaction_api.Models;

namespace transaction_api.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionRepository _TransactionRepository;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IClientRepository clientRepository, ITransactionRepository transactionRepository, ILogger<TransactionController> logger)
        {
            _clientRepository = clientRepository;
            _TransactionRepository = transactionRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a collection of transaction details for a client based on the provided client ID.
        /// </summary>
        /// <param name="clientID">The ID of the client for whom transactions are to be retrieved.</param>
        /// <returns>
        /// An ActionResult containing a collection of DTOs with transaction details for the specified client.
        /// If the client does not exist, returns NotFound; if the model state is invalid, returns BadRequest.
        /// </returns>
        [HttpGet]
        [Route("/client/{clientID}")]
        [SwaggerOperation(
            Summary = "Get a list of transactions for a single client",
            Description = "Retrieves a list of a client's from transactions the database.",
            Tags = new[] { "Transactions" }
        )]
        [SwaggerResponse(200, "Successfully retrieved the list of ClientTransactions", typeof(IEnumerable<ClientTransactionDTO>))]
        [SwaggerResponse(404, "Client not found", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult<IEnumerable<ClientTransactionDTO>>> GetTransactionsForClient(int clientID) 
        {
            try
            {
                //check model state
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                //if client exists
                if (await _clientRepository.GetClientAsync(clientID) != null)
                {
                    var transactions = await _TransactionRepository.GetTransactionsForClient(clientID);

                    return Ok(transactions);
                }
                else
                {
                    //if client does not exist return 404
                    return NotFound("Check ID Field");
                }
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPut("/comment/{id}")]

        public async Task<ActionResult> UpdateTransactionComment(long id, UpdateTransactionDTO transactionDTO) 
        {
            try
            {
                //check ModelState
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != transactionDTO.TransactionID) return BadRequest("ID Mismatch");

                var transaction = await _TransactionRepository.GetTransactionAsync(id);

                if (transaction == null) return NotFound();

                if (await _TransactionRepository.UpdateCommentForTransactionAsync(id, transactionDTO))
                {
                    transaction.Comment = transactionDTO.Comment;
                    //TODO Create CreateTransactionRoute, Get Single Transaction By ID
                    return CreatedAtRoute("TransactionByID", new { id }, transaction);
                }
                else
                {
                    return StatusCode(304, transaction);
                }
            }
            catch (Exception)
            {

                throw;
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
