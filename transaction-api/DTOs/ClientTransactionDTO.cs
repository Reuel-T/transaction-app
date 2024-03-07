﻿namespace transaction_api.DTOs
{
    public class ClientTransactionDTO
    {
        public int TransactionID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TransactionTypeName { get; set; }
        public string Comment { get; set; }
        public int TransactionTypeID { get; set; }
        public decimal Amount { get; set; }
    }
}