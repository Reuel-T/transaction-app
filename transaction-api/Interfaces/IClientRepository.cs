﻿using System.Collections;
using transaction_api.DTOs;
using transaction_api.Models;

namespace transaction_api.Interfaces
{
    public interface IClientRepository
    {
        public Task<IEnumerable<Client>> GetClientsAsync();
        public Task<Client> GetClientAsync(int clientId);
        public Task<Client> CreateClientAsync(CreateClientDTO client);
        public Task<bool> UpdateClientAsync(int clientId, UpdateClientDTO client);
        public Task<bool> DeleteClientAsync(int clientId);
    }
}
