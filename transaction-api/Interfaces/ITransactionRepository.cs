﻿using transaction_api.DTOs;
using transaction_api.Models;

namespace transaction_api.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<TransactionDTO> CreateTransactionAsync(CreateTransactionDTO transactionDTO);
        public Task<ClientTransactionDTO> GetTransactionAsync(long TransactionID);
        public Task<IEnumerable<ClientTransactionDTO>> GetTransactionsForClientAsync(int ClientID);
        public Task<bool> UpdateCommentForTransactionAsync(long TransactionID, UpdateTransactionCommentDTO transaction);
    }
}
