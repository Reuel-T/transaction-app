using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}", Name = "ClientById")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var client = await _clientRepository.GetClientAsync(id);
                if (client != null)
                {
                    return Ok(client);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
               return LogError(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient(ClientDTO client) 
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

        private ObjectResult LogError(Exception ex)
        {
            _logger.Log(LogLevel.Error, $"Error Type: {ex.GetType()}");
            _logger.Log(LogLevel.Debug, $"ErrorMessage: {ex.Message}");
            return StatusCode(500, "Server Error. Request not completed");
        }
    }
}
