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
        
        private record DeleteClientResponse 
        {
            public int ClientsRemoved { get; set; }
            public int TransactionsRemoved { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging information, warnings, and errors.</param>
        /// <param name="context">The Dapper context providing database connections.</param>
        public ClientRepository(ILogger<ClientRepository> logger, DapperContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Creates a new client based on the provided data asynchronously.
        /// </summary>
        /// <param name="client">The data for creating a new client.</param>
        /// <returns>The newly created client with the assigned ID.</returns>
        public async Task<Client> CreateClientAsync(CreateClientDTO client)
        {
            // Construct the SQL query for inserting a new client and retrieving the new ID
            string query = $@"
                INSERT INTO {Tables.Client}
                ({ClientFields.Name}, {ClientFields.Surname}, {ClientFields.ClientBalance})
                VALUES
                (@Name, @Surname, @ClientBalance)
                SELECT CAST(SCOPE_IDENTITY() AS int)
            ";

            // Create params
            var queryParams = new DynamicParameters();
            queryParams.Add("Name", client.Name, DbType.String);
            queryParams.Add("Surname", client.Surname, DbType.String);
            queryParams.Add("ClientBalance", client.ClientBalance, DbType.Decimal);

            //run query
            using var connection = _context.CreateConnection();
            int id = await connection.QuerySingleAsync<int>(query, queryParams);//use execute for simple updates, this runs a select after the insert to get the new ID

            // Log information about the newly created client
            _logger.Log(LogLevel.Information, $"Created Client with Id :{id} at {DateTime.UtcNow}");

            // Create a new Client object with the assigned ID and other details
            var newClient = new Client
            {
                ClientID = id,
                Name = client.Name,
                Surname = client.Surname,
                ClientBalance = client.ClientBalance,
            };
            // Return the newly created client
            return newClient;
        }

        /// <summary>
        /// Deletes a client and associated transactions asynchronously.
        /// </summary>
        /// <param name="clientId">The ID of the client to be deleted.</param>
        /// <returns>
        /// A boolean indicating whether the client and associated transactions were successfully deleted.
        /// </returns>
        public async Task<bool> DeleteClientAsync(int clientId)
        {
            //param for stored proc
            var parameters = new DynamicParameters();
            parameters.Add("Id", clientId, DbType.Int64);

            using var connection = _context.CreateConnection();

             var response = await connection.QueryFirstAsync<DeleteClientResponse>(
                        StoredProcedures.DeleteClient, parameters, commandType: CommandType.StoredProcedure
                    );

            int numberTransactionsDeleted = response.TransactionsRemoved;
            int numberClientsRemoved = response.ClientsRemoved;

            // Log information about the deletion of transactions
            if (numberTransactionsDeleted > 0)
            {
                _logger.Log(LogLevel.Information, $"Deleted {numberTransactionsDeleted} rows from {Tables.Transaction} for ClientID {clientId} at {DateTime.UtcNow}");
            }
            else
            {
                _logger.Log(LogLevel.Information, $"No Transactions for Client ID - {clientId} deleted");
            }

            // Log information about the deletion of the client
            if (numberClientsRemoved > 0)
            {
                _logger.Log(LogLevel.Information, $"Deleted Client with ID - {clientId} at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                _logger.Log(LogLevel.Information, $"No Rows Removed from Client Table (Attempted to delete Client with ID - {clientId})");
                return false;
            }
        }

        /// <summary>
        /// Retrieves a client based on the provided client ID asynchronously.
        /// </summary>
        /// <param name="clientId">The ID of the client to be retrieved.</param>
        /// <returns>The client with the specified ID, or null if not found.</returns>
        public async Task<Client> GetClientAsync(int clientId)
        {
            // Construct the SQL query for retrieving a client by ID
            string query = $@"
                SELECT * FROM {Tables.Client}
                WHERE {ClientFields.ClientID} = @Id
            ";

            //Get the client
            using var connection = _context.CreateConnection();
            var client = await connection.QuerySingleOrDefaultAsync<Client>(query, new { Id = clientId });

            // Return the retrieved client
            return client;
        }

        /// <summary>
        /// Retrieves all clients asynchronously.
        /// </summary>
        /// <returns>A collection of all clients in the database.</returns>
        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            // Construct the SQL query for retrieving all clients
            string query = $@"SELECT * FROM {Tables.Client}";

            //get all clients
            using var connection = _context.CreateConnection();
            var clients = await connection.QueryAsync<Client>(query);

            // Convert the result to a list and return
            return clients.ToList();
        }

        /// <summary>
        /// Updates client information asynchronously.
        /// </summary>
        /// <param name="clientId">The ID of the client to be updated.</param>
        /// <param name="client">The updated client data.</param>
        /// <returns>
        /// A boolean indicating whether the client information was successfully updated.
        /// </returns>
        public async Task<bool> UpdateClientAsync(int clientId, UpdateClientDTO client)
        {
            // Construct the SQL query for updating client information (excluding balance)
            string query = $@"
                UPDATE {Tables.Client}
                SET  
                    {ClientFields.Name} = @Name,
                    {ClientFields.Surname} = @Surname
                WHERE {ClientFields.ClientID} = @Id
            ";

            // Create parameters for the query using Dapper's DynamicParameters
            var queryParams = new DynamicParameters();
            queryParams.Add("Id", clientId, DbType.Int64);
            queryParams.Add("Name", client.Name, DbType.String);
            queryParams.Add("Surname", client.Surname, DbType.String);

            //run query
            using var connection = _context.CreateConnection();
            int affectedRows = await connection.ExecuteAsync(query, queryParams);

            // Log information about the update if successful
            if (affectedRows > 0)
            {
                _logger.Log(LogLevel.Information, $"Updated Client with ID : {clientId}");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
