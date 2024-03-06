using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using transaction_api.DTOs;
using transaction_api.Interfaces;
using transaction_api.Models;

namespace transaction_api.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientController> _logger;

        public ClientController(ILogger<ClientController> logger, IClientRepository clientRepository)
        {
            _logger = logger;
            _clientRepository = clientRepository;
        }

        [HttpGet]
        [Route("api/clients")]
        [SwaggerOperation(
            Summary = "Get a list of clients",
            Description = "Retrieves a list of clients from the database.",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "Successfully retrieved the list of clients", typeof(IEnumerable<Client>))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            try
            {
                var clients = await _clientRepository.GetClientsAsync();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        /// <summary>
        /// Gets a client by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the client.</param>
        /// <returns>An action result representing the client with the specified identifier.</returns>
        [HttpGet("{id}", Name = "ClientById")]
        [SwaggerOperation(
            Summary = "Get a client by ID",
            Description = "Retrieves a client from the database based on its unique identifier.",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "Successfully retrieved the client", typeof(Client))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid", typeof(void))]
        [SwaggerResponse(404, "Client not found", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            try
            {
                // Check if the ModelState is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Retrieve the client from the repository based on the provided ID
                var client = await _clientRepository.GetClientAsync(id);

                // Check if the client is found
                if (client != null)
                {
                    return Ok(client);
                }
                else
                {
                    // Return a 404 Not Found response if the client is not found
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions and return a 500 Internal Server Error response
                return LogError(ex);
            }
        }

        /// <summary>
        /// Creates a new client.
        /// </summary>
        /// <param name="client">The data for creating a new client.</param>
        /// <returns>
        /// An action result representing the response. 
        /// </returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new client",
            Description = "Creates a new client based on the provided data.",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(201, "Client created successfully", typeof(Client))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult> CreateClient(CreateClientDTO client)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdClient = await _clientRepository.CreateClientAsync(client);

                return CreatedAtRoute("ClientById", new { id = createdClient.ClientID }, createdClient);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        /// <summary>
        /// Updates an existing client based on the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the client to be updated.</param>
        /// <param name="updateClient">The data for updating the client.</param>
        /// <returns>
        /// An action result representing the response. 
        /// </returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update an existing client",
            Description = "Updates an existing client based on the provided data.",
            Tags = new[] { "Clients" }
            )]
        [SwaggerResponse(201, "Client updated successfully", typeof(Client))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid or ID mismatch", typeof(void))]
        [SwaggerResponse(404, "Client not found", typeof(void))]
        [SwaggerResponse(304, "Client not modified", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult> UpdateClient(int id, UpdateClientDTO updateClient) 
        {
            try
            {
                // Check if the ModelState is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if the provided ID matches the ID in the updateClient data
                if (id == updateClient.ClientID)
                {
                    // Retrieve the existing client from the repository
                    var client = await _clientRepository.GetClientAsync(id);
                    // Check if the client exists
                    if (client != null)
                    {
                        // Attempt to update the client in the repository
                        if (await _clientRepository.UpdateClientAsync(id, updateClient))
                        {
                            // Return a 201 Created response with the updated client details
                            return CreatedAtRoute("ClientById", new { id }, new {
                                client.ClientID,
                                updateClient.Name,
                                updateClient.Surname,
                                client.ClientBalance
                            });
                        }
                        else
                        {
                            // Return a 304 Not Modified response if the client has not been updated
                            return StatusCode(304, "Client has not been updated");
                        }
                    }
                    else
                    {   // Return a 404 Not Found response if the client with the specified ID is not found
                        return NotFound();
                    }

                }
                else
                {
                    // Return a 400 Bad Request response if the provided ID does not match the client ID
                    return BadRequest("Please ID Mismatch");
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions and return a 500 Internal Server Error response
                return LogError(ex);
            }
        }

        /// <summary>
        /// Deletes a client based on the provided client ID.
        /// </summary>
        /// <param name="id">The ID of the client to be deleted.</param>
        /// <returns>
        /// An ActionResult representing the result of the delete operation.
        /// </returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete an existing client",
            Description = "Deletes an Existing client based on the provided id",
            Tags = new[] { "Clients" }
            )]
        [SwaggerResponse(200, "Client Deleted successfully", typeof(void))]
        [SwaggerResponse(400, "Bad request - ModelState is not valid", typeof(void))]
        [SwaggerResponse(404, "Client not found", typeof(void))]
        [SwaggerResponse(304, "Client not modified", typeof(void))]
        [SwaggerResponse(500, "Internal server error", typeof(void))]
        public async Task<ActionResult> DeleteClient(int id) 
        {
            try
            {
                // If model is invalid, return BadRequest
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                // Check if the client exists
                var client = await _clientRepository.GetClientAsync(id);
                if (client != null)
                {

                    bool deleteSuccess = await _clientRepository.DeleteClientAsync(id);
                    // If client deleted successfully, return Ok
                    if (deleteSuccess)
                    {
                        return Ok();
                    }
                    else
                    {
                        // If no change occurred during deletion, return StatusCode 304
                        return StatusCode(304, "No Change");
                    }
                }
                else
                {
                    // If client does not exist, return NotFound
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log and handle exceptions
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
