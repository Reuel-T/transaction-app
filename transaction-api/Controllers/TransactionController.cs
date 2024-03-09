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
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionTypeRepository _transactionTypeRepository;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IClientRepository clientRepository,
                                     ITransactionRepository transactionRepository,
                                     ITransactionTypeRepository transactionTypeRepository,
                                     ILogger<TransactionController> logger)
        {
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _logger = logger;
        }

        [HttpGet("{TransactionID}", Name = "TransactionByID")]
        [SwaggerOperation(
            Summary = "Gets a single transaction by its ID",
            Tags = new[] { "Transactions"}
        )]
        [SwaggerResponse(200, "Found and returned requested Transaction", typeof(TransactionDTO))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid", typeof(void))]
        [SwaggerResponse(404, "Can't find transaction with specified ID", typeof(void))]
        [SwaggerResponse(500, "Internal Server Error", typeof(void))]
        public async Task<ActionResult<TransactionDTO>> GetTransactionByID(long TransactionID) 
        {
            try
            {
                //check modelstate
                if (!ModelState.IsValid) return BadRequest("Invalid Model State");

                //get transaction
                var transaction = await _transactionRepository.GetTransactionAsync(TransactionID);

                //if transaction does not exist
                if (transaction == null) return NotFound();

                //return the transaction
                return Ok(new ClientTransactionDTO
                {
                    TransactionID = transaction.TransactionID,
                    Amount = transaction.Amount,
                    Comment = transaction.Comment,
                    ClientID = transaction.ClientID,
                    TransactionTypeID = transaction.TransactionTypeID,
                    TransactionTypeName = transaction.TransactionTypeName
                });
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
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
        [Route("client/{clientID}")]
        [SwaggerOperation(
            Summary = "Get a list of transactions for a single client",
            Description = "Retrieves a list of a client's from transactions the database.",
            Tags = new[] { "Transactions" }
        )]
        [SwaggerResponse(200, "Successfully retrieved the list of ClientTransactions", typeof(IEnumerable<ClientTransactionDTO>))]
        [SwaggerResponse(400, "Invalid ModelState", typeof(void))]
        [SwaggerResponse(404, "Transaction not found", typeof(void))]
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
                    var transactions = await _transactionRepository.GetTransactionsForClientAsync(clientID);

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

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a Transaction",
            Description = "Creates a single transaction for a client",
            Tags = new[] { "Transactions" }
        )]
        [SwaggerResponse(201, "Transaction created successfully", typeof(TransactionDTO))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult> CreateTransaction([FromBody]CreateTransactionDTO createTransaction) 
        {
            try
            {
                //check modelstate
                if (!ModelState.IsValid) return BadRequest("Invalid Model");

                TransactionType transactionType = await _transactionTypeRepository.GetTransactionTypeByIDAsync(createTransaction.TransactionTypeID);

                //check if transactionType exists
                if (transactionType == null)
                {
                    return NotFound("Transaction Type not found");
                }

                //check client exists
                if (await _clientRepository.GetClientAsync(createTransaction.ClientID) == null) 
                {
                    return NotFound("Client Not Found");
                }

                //create the transaction
                TransactionDTO newTransaction = await _transactionRepository.CreateTransactionAsync(createTransaction);

                //if transaction successfully created
                if (newTransaction != null)
                {
                    ClientTransactionDTO clientTransactionDTO = new ClientTransactionDTO
                    {
                        Amount = newTransaction.Amount,
                        ClientID = newTransaction.ClientID,
                        Comment = newTransaction.Comment,
                        TransactionID = newTransaction.TransactionID,
                        TransactionTypeID = newTransaction.TransactionTypeID,
                        TransactionTypeName = transactionType.TransactionTypeName
                    };

                    return CreatedAtRoute("TransactionByID", new { newTransaction.TransactionID }, clientTransactionDTO);
                }
                else 
                {
                    //no transaction returned, update failed
                    return StatusCode(304);
                }
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPut]
        [Route("comment/{id}")]
        [SwaggerOperation(
            Summary = "Updates a comment associated with a transaction",
            Description = "Updates the comment of a transaction.",
            Tags = new[] { "Transactions" }
        )]
        [SwaggerResponse(201, "Transaction comment updated successfully", typeof(TransactionDTO))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult> UpdateTransactionComment(long id, [FromBody]UpdateTransactionCommentDTO transactionDTO) 
        {
            try
            {
                //check ModelState
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                //ensure correct transaction is being updated
                if (id != transactionDTO.TransactionID) return BadRequest("ID Mismatch");

                //check if transaction exists
                var transaction = await _transactionRepository.GetTransactionAsync(id);

                //if it does not
                if (transaction == null) return NotFound();

                //if the update happens
                if (await _transactionRepository.UpdateCommentForTransactionAsync(id, transactionDTO))
                {
                    transaction.Comment = transactionDTO.Comment;
                    //TODO Create CreateTransactionRoute, 
                    return CreatedAtRoute("TransactionByID", 
                        new { TransactionID = id }, 
                        new ClientTransactionDTO 
                        { 
                            TransactionID = transaction.TransactionID,
                            ClientID = transaction.ClientID,
                            Amount = transaction.Amount,
                            Comment = transactionDTO.Comment,
                            TransactionTypeID = transaction.TransactionTypeID,
                            TransactionTypeName = transaction.TransactionTypeName
                        });
                }
                else
                {
                    //update did not happen (data did not change)
                    return StatusCode(304);
                }
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
