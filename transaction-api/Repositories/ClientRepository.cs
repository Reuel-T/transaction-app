using Dapper;
using transaction_api.Context;
using transaction_api.DTOs;
using transaction_api.Interfaces;
using transaction_api.Models;
using System.Data;
using static transaction_api.Constants.Constants;

namespace transaction_api.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<ClientRepository> _logger;
        
        public ClientRepository(ILogger<ClientRepository> logger, DapperContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Client> CreateClientAsync(ClientDTO client)
        {
            string query = $@"
                INSERT INTO {Tables.Client}
                ({ClientFields.Name}, {ClientFields.Surname}, {ClientFields.ClientBalance})
                VALUES
                (@Name, @Surname, @ClientBalance)
                SELECT CAST(SCOPE_IDENTITY() AS int)
            ";

            var queryParams = new DynamicParameters();
            queryParams.Add("Name", client.Name, DbType.String);
            queryParams.Add("Surname", client.Surname, DbType.String);
            queryParams.Add("ClientBalance", client.ClientBalance, DbType.Decimal);

            using var connection = _context.CreateConnection();
            //use execute for simple updates, this runs a select after the insert to get the new ID
            int id = await connection.QuerySingleAsync<int>(query, queryParams);

            _logger.Log(LogLevel.Information, $"Created Client with Id :{id} at {DateTime.UtcNow}");

            var newClient = new Client
            {
                ClientID = id,
                Name = client.Name,
                Surname = client.Surname,
                ClientBalance = client.ClientBalance,
            };

            return newClient;
        }

        public async Task<Client?> GetClientAsync(int clientId)
        {
            string query = $@"
                SELECT * FROM {Tables.Client}
                WHERE {ClientFields.ClientID} = @Id
            ";

            using var connection = _context.CreateConnection();
            var client = await connection.QuerySingleOrDefaultAsync<Client>(query, new { Id = clientId });

            return client;
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            string query = $@"SELECT * FROM {Tables.Client}";

            using var connection = _context.CreateConnection();
            var clients = await connection.QueryAsync<Client>(query);

            return clients.ToList();
        }
    }
}
