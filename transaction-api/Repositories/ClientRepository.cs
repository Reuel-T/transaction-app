using Dapper;
using transaction_api.Context;
using transaction_api.Interfaces;
using transaction_api.Models;
using static transaction_api.Constants.Constants;

namespace transaction_api.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DapperContext _context;
        
        public ClientRepository(DapperContext context)
        {
            _context = context;
        }

        public Task<Client> GetClientAsync(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            string query = $@"SELECT * FROM {Tables.Client}";

            using (var connection = _context.CreateConnection()) 
            {
                var clients = await connection.QueryAsync<Client>(query);

                return clients.ToList();
            }
        }
    }
}
