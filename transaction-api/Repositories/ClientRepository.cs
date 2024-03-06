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

            // Create parameters for the query using Dapper's DynamicParameters
            var queryParams = new DynamicParameters();
            queryParams.Add("Name", client.Name, DbType.String);
            queryParams.Add("Surname", client.Surname, DbType.String);
            queryParams.Add("ClientBalance", client.ClientBalance, DbType.Decimal);

            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();
            //use execute for simple updates, this runs a select after the insert to get the new ID
            int id = await connection.QuerySingleAsync<int>(query, queryParams);

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
            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();
            connection.Open();
            // Begin a new transaction
            using var transaction = connection.BeginTransaction();
            try
            {
                // Construct the SQL query for deleting transactions associated with the client
                string query = $@"DELETE FROM {Tables.Transaction} WHERE {TransactionFields.ClientID} = @Id";
                // Execute the transaction delete query asynchronously and capture the number of rows affected
                int numberTransactionsDeleted = await connection.ExecuteAsync(query, new { Id = clientId }, transaction);

                // Log information about the deletion of transactions
                if (numberTransactionsDeleted > 0)
                {
                    _logger.Log(LogLevel.Information, $"Deleted {numberTransactionsDeleted} rows from {Tables.Transaction} for ClientID {clientId} at {DateTime.UtcNow}");
                }
                else
                {
                    _logger.Log(LogLevel.Information, $"No Transactions for Client ID - {clientId} deleted");
                }

                // Construct the SQL query for deleting the client
                query = $"DELETE FROM {Tables.Client} WHERE {ClientFields.ClientID} = @Id";
                // Execute the client delete query asynchronously and capture the number of rows affected
                int clientRowsAffected = await connection.ExecuteAsync(query, new { Id = clientId }, transaction);

                // Log information about the deletion of the client
                if (clientRowsAffected > 0)
                {
                    _logger.Log(LogLevel.Information, $"Deleted Client with ID - {clientId} at {DateTime.UtcNow}");
                    // Commit the transaction if successful
                    transaction.Commit();
                    return true;
                }
                else
                {
                    _logger.Log(LogLevel.Information, $"No Rows Removed from Client Table (Attempted to delete Client with ID - {clientId})");
                    // Commit the transaction even if no rows were affected (as it's intentional)
                    transaction.Commit();
                    return false;
                }

            }
            catch (Exception ex)
            {
                // Log an error message and rollback the transaction if an exception occurs
                _logger.Log(LogLevel.Error, $"Unable to Delete Client with ID({clientId}. Rolling Back Transaction)");
                transaction.Rollback();
                throw;
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

            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();
            // Execute the query asynchronously and retrieve a single client, or null if not found
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

            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();

            // Execute the query asynchronously and retrieve a collection of clients
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
            ";//Leave out Balance, this should only be affected when adding new transactions for a client 

            // Create parameters for the query using Dapper's DynamicParameters
            var queryParams = new DynamicParameters();
            queryParams.Add("Id", clientId, DbType.Int64);
            queryParams.Add("Name", client.Name, DbType.String);
            queryParams.Add("Surname", client.Surname, DbType.String);

            // Create a new database connection using the provided context
            using var connection = _context.CreateConnection();
            // Execute the update query asynchronously and capture the number of affected rows
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
