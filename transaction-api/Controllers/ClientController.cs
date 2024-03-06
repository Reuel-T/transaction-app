using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                string loggerErrorMessage = $"Error Type: {ex.GetType()} \nErrorMessage: {ex.Message}";
                _logger.Log(LogLevel.Error, loggerErrorMessage);
                return StatusCode(500, "Server Error. Request not completed");
            }
        }
    }
}
